using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace EGrabberWPF
{
    public class EGrabberWindowModels
    {
        private string _frameRate;
        private string _exposureTime;
        private string _path_1;
        private string _path_2;
        private string _bmpPath;
        private int _capNum;
        private int _capTime;//秒

        public string BmpPath
        {
            get { return _bmpPath; }
            set
            {
                _bmpPath = value;
            }
        }
        public string FrameRate
        {
            get { return _frameRate; }
            set
            {
                _frameRate = value;
            }
        }

        public string ExposureTime
        {
            get { return _exposureTime; }
            set
            {
                _exposureTime = value;
            }
        }

        public string Path_1
        {
            get { return _path_1; }
            set
            {
                _path_1 = value;
            }
        }

        public string Path_2
        {
            get { return _path_2; }
            set
            {
                _path_2 = value;
            }
        }

        public int CapNum
        {
            get { return _capNum; }
            set
            {
                _capNum = value;
            }
        }

        public int CapTime
        {
            get { return _capTime; }
            set
            {
                _capTime = value;
            }
        }

        //Constructor
        public EGrabberWindowModels()
        {
            //Default
            _frameRate = "506";
            _exposureTime = 1972.ToString();
            _path_1 = "F:\\WPF\\test_0.dat";
            _path_2 = "G:\\WPF\\test_1.dat";
            _bmpPath = "G:\\WPF";
            _capNum = 1;
            _capTime = 0;
        }
    }
}
