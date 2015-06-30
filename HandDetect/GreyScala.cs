using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace Bitirme
{

    class GreyScala
    {



        public Bitmap change_gray(Bitmap video)
        {

            Rectangle rect = new Rectangle(0, 0, video.Width, video.Height);
            System.Drawing.Imaging.BitmapData bmpData =
                video.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
               video.PixelFormat);

            IntPtr ptr = bmpData.Scan0;

            int bytes = Math.Abs(bmpData.Stride) * video.Height;
            byte[] rgbValues = new byte[bytes];

            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

            for (int counter = 0; counter < rgbValues.Length - 3; counter += 3)
            {

                int renk = (int)((rgbValues[counter] + (rgbValues[counter + 1]) + (rgbValues[counter + 2]))) / 3;
                rgbValues[counter] = (byte)renk;
                rgbValues[counter + 1] = (byte)renk;
                rgbValues[counter + 2] = (byte)renk;

            }

            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);
            video.UnlockBits(bmpData);

            return video;
        }

          ~GreyScala () // Destructor
        {
            GC.Collect(); // GC(Garbage collector)'in zorunlu olarak bellekteki gereksiz objeleri kaldırması istenebilir.
           
           
        }

    }

}