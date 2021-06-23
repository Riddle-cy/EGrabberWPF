using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGrabberWPF
{
    public class CameraSetting
    {
        //static private CameraSetting sc;
        //static public CameraSetting GetInstance()
        //{
        //    return sc;
        //}

        static public string FrameRate;
        static public string ExposureTime;
        static public string Path_1;
        static public string Path_2;
        static public string Bmp_Path;
        static public int CapNum; //采集数量
        static public int CapTime;//采集时长
        static public bool CapByTime = false;//是否采集指定时长，若为false，则采集指定图像帧数

        private CameraSetting()
        {
            //私有化构造函数 静态生成单例
        }


        
    }
}
