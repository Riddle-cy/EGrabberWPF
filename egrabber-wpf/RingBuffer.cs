using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EGrabberWPF
{
    class RingBuffer
    {
        public int picSize;
        public int FIFOSize;
        public int front;
        public int rear;
        public long count;
        public bool exit;
        public long emptyTime, fullTime;

        public byte[][] buffer;

        public RingBuffer(int NUM)
        {
            picSize = 2304 * 1720;
            FIFOSize = NUM;

            buffer = new byte[NUM][];
            for (int i = 0; i < NUM; ++i)
            {
                buffer[i] = new byte[picSize];
            }

            count = 0;
            exit = false;
            emptyTime = fullTime = 0;
        }

        public bool isEmpty()
        {
            return front == rear;
        }

        public bool isFull()
        {
            return (rear + 1) % FIFOSize == front;
        }

        public bool isExit()
        {
            return exit;
        }

        public void addEmpty()
        {
            ++emptyTime;
        }

        public void addFull()
        {
            ++fullTime;
        }
    }
}
