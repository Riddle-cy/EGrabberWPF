using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Linq.Expressions;
using EGrabberWPF.ViewModel;
using System.Windows.Forms;

namespace EGrabberWPF
{
    /// <summary>
    /// Interaction logic for EGrabberWindow.xaml
    /// </summary>
    public partial class EGrabberWindow : Window
    {

        #region member
        private Euresys.EGenTL genTL;
        private Euresys.FormatConverter.FormatConverter converter;
        private Euresys.EGrabberCallbackSingleThread myEGrabber;
        private volatile bool grabbing;
        private volatile bool stopping;
        private volatile bool disposed;
        private volatile bool rendering;
        UInt64 width;
        UInt64 height;
        String format;
        private WriteableBitmap imageBitmapSource;
        private Int32Rect dirtRect;
        private volatile int grabberCount;
        private RingBuffer[] FIFO;
        private MyWatch watch;
        private String[] Path;
        //private string BmpPath;
        private int FileIndex = 0;

        // 缓存设置
        static int FIFO_NUM = 2;
        static int bufferSize = 512;
        static int picSize = 3962880;

        static Thread FileWriteThread_0;
        static Thread FileWriteThread_1;

        //目录根节点
        private TreeItemViewModel rootNode = null;

        #endregion member

        #region Constructor
        public EGrabberWindow()
        {
            InitializeComponent();

            var node0 = new TreeItemViewModel(null, false) { DisplayName="node0"};

            rootNode = new TreeItemViewModel(node0, false,true) { DisplayName = "rootNode" };

            node0.Children.Add(rootNode);

            DataContext = rootNode;

            //rootNode.BmpPath = DecodePath.Text;

            try
            {
                genTL = new Euresys.EGenTL();
                converter = new Euresys.FormatConverter.FormatConverter(genTL);
                myEGrabber = new Euresys.EGrabberCallbackSingleThread(genTL);
                //if (System.IO.File.Exists("config.js"))
                //{
                //    myEGrabber.runScript("config.js");
                //}
                //myEGrabber.RemotePort.set
                myEGrabber.reallocBuffers(256);
                width = myEGrabber.getWidth();
                height = myEGrabber.getHeight();
                format = myEGrabber.getPixelFormat();
                initBitmapSource();
                grabberCount = 0;
                watch = new MyWatch();
            }
            catch (Exception)
            {
                DisposeUnmanagedResources();
                throw;
            }
        }

        #endregion Constructor

        #region Destroy

        ~EGrabberWindow()
        {
            genTL.Dispose();
        }

        #endregion Destroy

        private void TheTreeView_PreviewSelectionChanged(object sender, PreviewSelectionChangedEventArgs e)
        {
            if (LockSelectionCheck.IsChecked == true)
            {
                // The current selection is locked by user request (Lock CheckBox is checked).
                // Don't allow any changes to the selection at all.
                e.CancelThis = true;
            }
            else
            {
                // Selection is not locked, apply other conditions.
                // Require all selected items to be of the same type. If an item of another data
                // type is already selected, don't include this new item in the selection.
                if (e.Selecting && TheTreeView.SelectedItems.Count > 0)
                {
                    e.CancelThis = e.Item.GetType() != TheTreeView.SelectedItems[0].GetType();
                }
            }

            //if (e.Selecting)
            //{
            //    System.Diagnostics.Debug.WriteLine("Preview: Selecting " + e.Item + (e.Cancel ? " - cancelled" : ""));
            //}
            //else
            //{
            //    System.Diagnostics.Debug.WriteLine("Preview: Deselecting " + e.Item + (e.Cancel ? " - cancelled" : ""));
            //}
        }

        private void NodeMouseClick(object sender, RoutedEventArgs e)
        {
            foreach(TreeItemViewModel node in TheTreeView.SelectedItems)
            {
                
                //string fileName = node.DisplayName.Substring(4,7);
                FileStream BmpRead = new FileStream(node.DisplayName, FileMode.Open);
                BinaryReader biread = new BinaryReader(BmpRead);
                unsafe
                {
                    BMP myBmp = new BMP(width, height);
                    byte[] data = biread.ReadBytes(myBmp.BmpHeader.Length);
                    data = biread.ReadBytes(picSize);
                    IntPtr bufferPtr = Marshal.UnsafeAddrOfPinnedArrayElement(data, 0);
                    System.Windows.Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        try
                        {
                            imageBitmapSource.Lock();
                            converter.toBGR8(imageBitmapSource.BackBuffer, bufferPtr, format, width, height);
                            imageBitmapSource.AddDirtyRect(dirtRect);
                            statusFileID.Text = FileIndex.ToString();
                            ++FileIndex;
                        }
                        finally
                        {
                            imageBitmapSource.Unlock();
                        }
                    }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
                }
                biread.Close();
                BmpRead.Close();
            }
        }

        private void ExpandMenuItem_Click(object sender, RoutedEventArgs e)
        {
            foreach (TreeItemViewModel node in TheTreeView.SelectedItems)
            {
                node.IsExpanded = true;
            }
        }

        private void RenameMenuItem_Click(object sender, RoutedEventArgs e)
        {
            foreach (TreeItemViewModel node in TheTreeView.SelectedItems)
            {
                node.IsEditing = true;
                break;
            }
        }


        public void DisposeUnmanagedResources()
        {
            if (myEGrabber != null)
            {
                myEGrabber.Dispose();
                myEGrabber = null;
            }
            if (converter != null)
            {
                converter.Dispose();
                converter = null;
            }
            if (genTL != null)
            {
                genTL.Dispose();
                genTL = null;
            }
            disposed = true;
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Hide();
            stopStream();
        }

        private void EGrabberChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            EGrabberChildWindow.Width = (width > System.Windows.SystemParameters.WorkArea.Width) ?
                System.Windows.SystemParameters.WorkArea.Width : (width + 8);
            EGrabberChildWindow.Height = (height > System.Windows.SystemParameters.WorkArea.Height) ?
                System.Windows.SystemParameters.WorkArea.Height : (height + 35);
            statusPixelFormat.Text = format;
            statusResolution.Text = width + "x" + height;
        }

        private void initBitmapSource()
        {
            dirtRect = new Int32Rect(0, 0, (int)width, (int)height);
            imageBitmapSource = new WriteableBitmap((int)width, (int)height, 96, 96, System.Windows.Media.PixelFormats.Bgr24, null);
            EGrabberImage.Source = imageBitmapSource;
        }

        public void updateImageBitmap(Euresys.EGrabberCallbackSingleThread g, Euresys.NewBufferData data)
        {
            if (stopping || disposed)
            {
                // when stopping: the grabber is busy stopping or flushing events
                // when disposed: the grabber and converter objects have been destroyed
                return;
            }
            using (Euresys.ScopedBuffer buffer = new Euresys.ScopedBuffer(g, data))
            {
                IntPtr bufferPtr;
                buffer.getInfo(Euresys.gc.BUFFER_INFO_CMD.BUFFER_INFO_BASE, out bufferPtr);
                try
                {
                    imageBitmapSource.Lock();
                    converter.toBGR8(imageBitmapSource.BackBuffer, bufferPtr, format, width, height);
                    imageBitmapSource.AddDirtyRect(dirtRect);
                }
                finally
                {
                    imageBitmapSource.Unlock();
                }
            }
            try
            {
                statusFrameRate.Text = g.getFloatStreamModule("StatisticsFrameRate").ToString("f1");
                statusImageID.Text = g.getFloatStreamModule("StatisticsFrameRate").ToString("f1");
            }
            catch
            {
            }
        }

        //public void onNewBuffer(Euresys.EGrabberCallbackSingleThread g, Euresys.NewBufferData data)
        //{
        //    if (stopping || disposed)
        //    {
        //        // the Main UI thread is running the stopStream function while we
        //        // receive a new buffer event; in this case we don't ask the Main
        //        // UI thread to process the image because if we are closing the
        //        // application, the DisposeUnmanagedResources function will be
        //        // called and the grabber could have been destroyed in the mean time
        //        return;
        //    }
        //    else if (rendering)
        //    {
        //        // we already asked the Main UI thread to display another buffer,
        //        // so we discard any incoming buffer until the Main UI thread has
        //        // displayed the other buffer...
        //        g.push(data);
        //    }
        //    else
        //    {
        //        // we prevent any other incoming buffer to be displayed
        //        rendering = true;
        //        // we post the updateImageBitmap function in the context of the Main UI thread
        //        Application.Current.Dispatcher.InvokeAsync(() =>
        //        {
        //            try
        //            {
        //                updateImageBitmap(g, data);
        //            }
        //            finally
        //            {
        //                // we make sure to reset the flag for the next buffer event
        //                rendering = false;
        //            }
        //        }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
        //        // Note: we post the function with ApplicationIdle priority to avoid overloading
        //        // the Main UI thread
        //        //   - if the grabber delivers new buffer events at a high frame rate
        //        //   - if the images are big (WPF framework performance decreases)
        //    }
        //}

        public void onNewBuffer(Euresys.EGrabberCallbackSingleThread g, Euresys.NewBufferData data)
        {
            int index = (int)grabberCount % FIFO_NUM;
            if (FIFO[index].isFull())
            {
                FIFO[index].addFull();
            }
            else
            {
                Euresys.ScopedBuffer buffer = new Euresys.ScopedBuffer(g, data);
                IntPtr imgPtr;
                buffer.getInfo(Euresys.gc.BUFFER_INFO_CMD.BUFFER_INFO_BASE, out imgPtr);
                Marshal.Copy(imgPtr, FIFO[index].buffer[FIFO[index].rear], 0, picSize);
                FIFO[index].rear = (FIFO[index].rear + 1) % bufferSize;

                if (!stopping && !rendering)
                {
                    rendering = true;
                    System.Windows.Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        DisplayNUM(g);
                    }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);
                }
                g.push(data);
            }
            
            ++grabberCount;
            if(!CameraSetting.CapByTime && grabberCount >= CameraSetting.CapNum)
            {
                stopStream();
            }
            if(CameraSetting.CapByTime && watch.GetCurrentTime() - CameraSetting.CapTime >= 0.00000)
            {
                stopStream();
            }
        }

        public void startStream()
        {
            if (!grabbing && myEGrabber != null)
            {
                try
                {
                    //InitFIFO
                    FIFO = new RingBuffer[FIFO_NUM];
                    Path = new string[2];
                    Path[0] = CameraSetting.Path_1;
                    Path[1] = CameraSetting.Path_2;
                    for (int i = 0; i < FIFO_NUM; ++i)
                        FIFO[i] = new RingBuffer(bufferSize);


                    FileStore fileStore_0 = new FileStore(FIFO, 0, Path[0]);
                    FileStore fileStore_1 = new FileStore(FIFO, 1, Path[1]);
                    FileWriteThread_0 = new Thread(new ThreadStart(fileStore_0.StoreData));
                    FileWriteThread_1 = new Thread(new ThreadStart(fileStore_1.StoreData));
                    FileWriteThread_0.IsBackground = true;
                    FileWriteThread_1.IsBackground = true;
                    FileWriteThread_0.Priority = ThreadPriority.Highest;
                    FileWriteThread_1.Priority = ThreadPriority.Highest;
                    FileWriteThread_0.Start();
                    FileWriteThread_1.Start();

                    stopping = false;

                    myEGrabber.setStringRemoteModule("AcquisitionFrameRate",CameraSetting.FrameRate);

                    //if(CameraSetting.ExposureTime)

                    myEGrabber.setStringRemoteModule("ExposureTime", CameraSetting.ExposureTime);
                    myEGrabber.onNewBufferEvent = onNewBuffer;
                    watch.Start();
                    myEGrabber.start();
                    grabbing = true;
                }
                catch(System.Exception e)
                {
                  
                }
            }
        }

        public void stopStream()
        {
            if (grabbing && myEGrabber != null)
            {
                stopping = true;
                myEGrabber.stop();
                myEGrabber.flushAllEvent();
                myEGrabber.onNewBufferEvent = null;
                grabbing = false;

                
                for (int i = 0; i < FIFO_NUM; ++i) {
                    FIFO[i].exit = true;
                }
            }
        }

        public String getInterfaceID()
        {
            if (myEGrabber == null)
            {
                return "<no grabber>";
            }
            return myEGrabber.getStringInterfaceModule("InterfaceID");
        }

        public string getCameraID()
        {
            if (myEGrabber == null)
            {
                return "<no grabber>";
            }
            try
            {
                // This feature is not always available
                return myEGrabber.getStringRemoteModule("DeviceModelName");
            }
            catch (Euresys.gentl_error)
            {
                return "N/A";
            }
        }

        public void DisplayNUM(Euresys.EGrabberCallbackSingleThread g)
        {
            try
            {
                statusFrameRate.Text = g.getFloatStreamModule("StatisticsFrameRate").ToString("f1");
                statusImageID.Text = grabberCount.ToString();
                long overTimes = 0;
                for (int i = 0; i < FIFO_NUM; ++i)
                    overTimes += FIFO[i].fullTime;
                statusOverflowTimes.Text = overTimes.ToString();
            }
            catch
            {
            }

            rendering = false;
        }

        private class FileStore
        {
            private RingBuffer[] myFIFO;
            private int id;
            private string path;

            public FileStore(RingBuffer[] fifo,int index, string str)
            {
                myFIFO = fifo;
                id = index;
                path = str;
            }

            public void StoreData()
            {
                FileStream fsDat = new FileStream(path, FileMode.OpenOrCreate);
                BinaryWriter biwrite = new BinaryWriter(fsDat);

                while(!myFIFO[id].isEmpty() || !myFIFO[id].isExit())
                {
                    if (!myFIFO[id].isEmpty())
                    {
                        byte[] data = myFIFO[id].buffer[myFIFO[id].front];
                        biwrite.Write(data, 0, picSize);
                        myFIFO[id].front = (myFIFO[id].front + 1) % bufferSize;
                    }
                }

                biwrite.Close();
                fsDat.Close();
            }
        }

        public class FileID : INotifyPropertyChanged
        {
            private string index = string.Empty;

            public string TheValue
            {
                get { return index; }
                set
                {
                    if (string.IsNullOrEmpty(value) && value == index)
                        return;

                    index = value;
                    NotifyPropertyChanged(() => TheValue);
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            public void NotifyPropertyChanged<T>(Expression<Func<T>> property)
            {
                if (PropertyChanged == null)
                    return;

                var memberExpression = property.Body as MemberExpression;
                if (memberExpression == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(memberExpression.Member.Name));
            }
        }

        private void Button_Show(object sender, RoutedEventArgs e)
        {
            if (stopping)
            {
                IntPtr bufferPtr;
                FileStream fsDat = new FileStream(Path[FileIndex % 2], FileMode.Open);
                BinaryReader biread = new BinaryReader(fsDat);

               
                byte[] data = biread.ReadBytes(picSize);
                bufferPtr = Marshal.UnsafeAddrOfPinnedArrayElement(data, 0);

                System.Windows.Application.Current.Dispatcher.InvokeAsync(() =>
                       {
                           try
                           {
                               imageBitmapSource.Lock();
                               converter.toBGR8(imageBitmapSource.BackBuffer, bufferPtr, format, width, height);
                               imageBitmapSource.AddDirtyRect(dirtRect);
                               statusFileID.Text = FileIndex.ToString();
                               ++FileIndex;
                           }
                           finally
                           {
                               imageBitmapSource.Unlock();
                           }
                       }, System.Windows.Threading.DispatcherPriority.ApplicationIdle);

               
                biread.Close();
                fsDat.Close();
                
            }
        }

        private static BitmapImage ByteToImage(byte[] byteArray)
        {
            BitmapImage bmp = null;
            try
            {
                bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.StreamSource = new MemoryStream(byteArray);
                bmp.EndInit();
            }
            catch
            {
                bmp = null;
            }

            return bmp;
        }


        private void Button_Decode(object sender, RoutedEventArgs e)
        {
            if (stopping)
            {
                FileStream CaptureData_0 = new FileStream(Path[0], FileMode.Open);
                FileStream CaptureData_1 = new FileStream(Path[1], FileMode.Open);
                BinaryReader[] biread = new BinaryReader[2];
                biread[0] = new BinaryReader(CaptureData_0);
                biread[1] = new BinaryReader(CaptureData_1);

                FileStream bmp_Out;
                BMP myBmp = new BMP(width, height);
                for (int i = 0; i < grabberCount; ++i)
                {
                    byte[] data = biread[i&1].ReadBytes(picSize);
                    

                    string path = "G:\\Decode\\Cap_";
                    int zeroCount = i, j = 6;
                    while (zeroCount >= 10)
                    {
                        --j;
                        zeroCount /= 10;
                    }

                    while (j!=0)
                    {
                        path += 0.ToString();
                        --j;
                    }
                    path += i.ToString() + ".bmp";

                    bmp_Out = new FileStream(path, FileMode.OpenOrCreate);
                    //bmp包头
                    bmp_Out.SetLength(myBmp.BmpHeader.Length);
                    bmp_Out.Write(myBmp.BmpHeader, 0, myBmp.BmpHeader.Length);

                    bmp_Out.SetLength(picSize);
                    //bmp_Out.Write(data, myBmp.BmpHeader.Length, data.Length);
                    bmp_Out.Write(data, 0, data.Length);
                    bmp_Out.Close();
                    //BitmapImage bmp = ByteToImage(data);
                    //Stream pixelStream = new MemoryStream(data);
                    //Bitmap myBitmap = new Bitmap(pixelStream, false);
                    //myBitmap.Save(path);
                }

                biread[0].Close();
                biread[1].Close();
                CaptureData_0.Close();
                CaptureData_1.Close();
            }


            //end decode, show file treeview
            for(int i = 0; i < 100 && i<=grabberCount; ++i)
            {
                string fileName = "G:\\Decode\\Cap_";
                int zeroCount = i, j = 6;
                while (zeroCount >= 10)
                {
                    --j;
                    zeroCount /= 10;
                }

                while (j != 0)
                {
                    fileName += 0.ToString();
                    --j;
                }
                fileName += i.ToString() + ".bmp";
                rootNode.Children.Add(new TreeItemViewModel(rootNode, false) { DisplayName = fileName, IsEditable = true });
            }
        }

        private void LockSelectionCheck_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
