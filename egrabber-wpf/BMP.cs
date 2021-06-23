using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.InteropServices;

namespace EGrabberWPF
{
    class BMP
    {
        public struct BmpFileHeader
        {
            public UInt32 bfSize;
            public ushort bfReserved1;
            public ushort bfReserved2;
            public UInt32 bfOffBits;
        };

        public struct BmpInfoHeader
        {
            public UInt32 biSize;
            public Int32 biWidth;
            public Int32 biHeight;
            public ushort biPlanes;
            public ushort biBitCount;          //表示像素深度
            public UInt32 biCompression;            //4字节，压缩类型，设置为0，不压缩
            public UInt32 biSizeImage;              //4字节，说明位图数据大小，不压缩时设置为0
            public Int32 biXPelsPerMeter;                   //水平分辨率
            public Int32 biYPelsPerMeter;                   //垂直分辨率
            public UInt32 biClrUsed;                //说明位图使用的调色板中的颜色索引数，为0说明使用所有
            public UInt32 biClrImportant;			//说明对图像显示有重要影响的颜色索引数，为0说明都重要
        };


        public struct RgbQuad
        {      //调色板
            public byte rgbBlue;                  //该颜色的蓝色分量  
            public byte rgbGreen;                 //该颜色的绿色分量  
            public byte rgbRed;                   //该颜色的红色分量  
            public byte rgbReserved;              //保留值，alpha
        };

        struct ClImage
        {      //图像数据
            public int width;
            public int height;
            public int channels;                   //通道数
            public byte[] imageData;               //像素数据
        };

        public ushort fileType = 0x4D42;

        private UInt64 width;
        private UInt64 height;

        public byte[] BmpHeader;
        public BmpFileHeader bmpFileHeader;
        public BmpInfoHeader bmpInfoHeader;
        public RgbQuad[] quad;


        private static byte[] StructToBytes(object obj)
        {
           
            int size = Marshal.SizeOf(obj);
            byte[] result = new byte[size];
            IntPtr buffer = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(obj, buffer, false);
            Marshal.Copy(buffer, result, 0, size);
            Marshal.FreeHGlobal(buffer);
            return result;
        }

        public BMP(UInt64 w,UInt64 h)
        {
            width = w;
            height = h;
            ulong step = width;
            ulong offset = step % 4;
            if (offset != 4) step += 4 - offset;

            //文件头信息的建立
            bmpFileHeader.bfSize = (UInt32)(54 + 256 * 4 + width);
            bmpFileHeader.bfReserved1 = 0;
            bmpFileHeader.bfReserved2 = 0;
            bmpFileHeader.bfOffBits = 54 + 256 * 4;

            //位图头信息的建立
            
            bmpInfoHeader.biSize = 40;
            bmpInfoHeader.biWidth = (Int32)width;
            bmpInfoHeader.biHeight = (Int32)height;
            bmpInfoHeader.biPlanes = 1;
            bmpInfoHeader.biBitCount = 8;
            bmpInfoHeader.biCompression = 0;
            bmpInfoHeader.biSizeImage = (UInt32)(height * step);
            bmpInfoHeader.biXPelsPerMeter = 0;
            bmpInfoHeader.biYPelsPerMeter = 0;
            bmpInfoHeader.biClrUsed = 256;
            bmpInfoHeader.biClrImportant = 256;

            //调色板信息
            quad = new RgbQuad[256];
            for (int i = 0; i < 256; i++)
            {
                quad[i].rgbBlue = (byte)i;
                quad[i].rgbGreen = (byte)i;
                quad[i].rgbRed = (byte)i;
                quad[i].rgbReserved = 0;
            }

            //文件头合成
            int InfoHeaderLen = 0;
            unsafe
            {
                InfoHeaderLen = sizeof(ushort);
                InfoHeaderLen += sizeof(BmpFileHeader);
                InfoHeaderLen += sizeof(BmpInfoHeader);
                InfoHeaderLen += sizeof(RgbQuad) * 256;

                BmpHeader = new byte[InfoHeaderLen];

                int dstOffset = 0;

                byte[] fileTypeByte = BitConverter.GetBytes(fileType);
                Buffer.BlockCopy(fileTypeByte, 0, BmpHeader, dstOffset, fileTypeByte.Length);
                dstOffset += 2;
                byte[] BmpFileHeaderByte = StructToBytes(bmpFileHeader);
                Buffer.BlockCopy(BmpFileHeaderByte, 0, BmpHeader, dstOffset, BmpFileHeaderByte.Length);
                dstOffset += sizeof(BmpFileHeader);
                byte[] BmpInfoHeaderByte = StructToBytes(bmpInfoHeader);
                Buffer.BlockCopy(BmpInfoHeaderByte, 0, BmpHeader, dstOffset, BmpInfoHeaderByte.Length);
                dstOffset += sizeof(BmpInfoHeader);
                for(int i = 0; i < 256; ++i)
                {
                    byte[] QuadByte = StructToBytes(quad[i]);
                    Buffer.BlockCopy(QuadByte, 0, BmpHeader, dstOffset, QuadByte.Length);
                    dstOffset += sizeof(RgbQuad);
                }
            }
            
        }
    }
}
