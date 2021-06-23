using System;
using System.Windows;

namespace EGrabberWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        private EGrabberWindow myEGrabberWin;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new EGrabberWindowModels();
            
            try
            {
                //mySingleGrabWindow = new EGrabberWPF.SingleGrabWindow();
                myEGrabberWin = new EGrabberWPF.EGrabberWindow();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Grabber Initialization Error");
            }
        }

        private void Button_Start(object sender, RoutedEventArgs e)
        {
            if (myEGrabberWin != null)
            {
                //相机相关配置参数赋值到静态变量，传递给采集窗口
                CameraSetting.FrameRate = TxtFR.Text;
                CameraSetting.Path_1 = Path1.Text;
                CameraSetting.Path_2 = Path2.Text;
                CameraSetting.Bmp_Path = DecodePath.Text;
                CameraSetting.CapNum = Convert.ToInt32(CapNum.Text);
                CameraSetting.CapTime = Convert.ToInt32(CapTime.Text);
                CameraSetting.ExposureTime = ExposureTime.Text;
                double ExposureMaxTime = (double)(1000 / Convert.ToDouble(CameraSetting.FrameRate) * 1000)-4.0;
                if (Convert.ToDouble(ExposureTime.Text) - ExposureMaxTime > 0.0000)
                {
                    CameraSetting.ExposureTime = ExposureMaxTime.ToString();
                    ExposureTime.Text = ExposureMaxTime.ToString() + "(Max)";
                }
                

                myEGrabberWin.startStream();
                myEGrabberWin.Show();
            }
        }

        private void Button_Stop(object sender, RoutedEventArgs e)
        {
            if (myEGrabberWin != null)
            {
                //myEGrabberWin.Hide();
                myEGrabberWin.stopStream();
            }
        }

        private void featuresID(object sender, RoutedEventArgs e)
        {
            if (myEGrabberWin == null)
            {
                interfaceID.Text = "<no grabber>";
                cameraID.Text = "";
            }
            else
            {
                interfaceID.Text = myEGrabberWin.getInterfaceID();
                cameraID.Text = myEGrabberWin.getCameraID();
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (myEGrabberWin != null)
            {
                myEGrabberWin.stopStream();
                myEGrabberWin.DisposeUnmanagedResources();
            }
        }

        private void IfCapAccordingTime_Checked(object sender, RoutedEventArgs e)
        {
            CameraSetting.CapByTime = true;
        }
    }
}
