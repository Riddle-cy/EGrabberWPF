using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGrabberWPF
{
    class MyWatch
    {
        public double TotalTime { get => paused || done ? time : time + (DateTime.Now - startTime).TotalMilliseconds; }

        bool paused = false;
        bool done = false;
        double time = 0;
        DateTime startTime;

        /// <summary>
        /// 开始计时。
        /// </summary>
        public void Start()
        {
            time = 0;
            paused = false;
            done = false;
            startTime = DateTime.Now;
        }

        /// <summary>
        /// 停止计时，此后才可以读取时间。
        /// </summary>
        public void Stop()
        {
            time += (DateTime.Now - startTime).TotalMilliseconds;
            done = true;
        }

        /// <summary>
        /// 暂停计时。
        /// </summary>
        public void Pause()
        {
            if (!done)
            {
                paused = true;
                time += (DateTime.Now - startTime).TotalMilliseconds;
            }
        }

        /// <summary>
        /// 恢复暂停，只有已经暂停了才有效果。
        /// </summary>
        public void Resume()
        {
            if (paused && !done)
                startTime = DateTime.Now;
        }

        /// <summary>
        /// 返回正在计时中的时间。
        /// </summary>
        /// <returns></returns>
        public double GetCurrentTime()
        {
            return paused ? time : time + (DateTime.Now - startTime).TotalMilliseconds;
        }

        public void Show(string title)
        {
            Console.WriteLine($"{title}: {TotalTime} ms.");
        }
    }
}
