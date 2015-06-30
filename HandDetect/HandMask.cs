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

using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Bitirme
{
    //find hand area and hand direction
    class HandMask
    {

        Bitmap bmp4;
        Bitmap bmp1;
        Bitmap bmp2;
        Bitmap bmp3;

       // public Bitmap img;

        public int[] Koordinat;

        int[] RightHis;
        int[] DownHis;
        int[] UpHis;
        int[] LeftHis;



        int setoff_x;
        int first_xmin = 0;
        int first_xmax = 0;

        int setoff_y;
        int first_ymin = 0;
        int first_ymax = 0;
        int setoff_bilek;
        public int yon;

        private static object _LOCK;


        public HandMask( Bitmap Otsu)
        { //resmin kendisi

           // this.img = (Bitmap)bmp.Clone();

            _LOCK = new object();

            bmp1 = (Bitmap)Otsu.Clone();
            bmp2 = (Bitmap)Otsu.Clone();
            bmp3 = (Bitmap)Otsu.Clone();
            bmp4 = (Bitmap)Otsu.Clone();

            LeftHis = new int[Otsu.Width];
            RightHis = new int[Otsu.Width];
            DownHis = new int[Otsu.Height];
            UpHis = new int[Otsu.Height];

            Koordinat = new int[4];

            Array.Clear(LeftHis, 0, LeftHis.Length);
            Array.Clear(DownHis, 0, DownHis.Length);
            Array.Clear(RightHis, 0, RightHis.Length);
            Array.Clear(UpHis, 0, UpHis.Length);

            setoff_bilek = Otsu.Width / 20;
            setoff_y = Otsu.Height / 100;
            setoff_x = Otsu.Width / 100;


        }

        public void HandSelect()
        {

            Task[] tasks = new Task[4];
            tasks[0] = new Task(Left);
            tasks[1] = new Task(Right);
            tasks[2] = new Task(Up);
            tasks[3] = new Task(Down);


            tasks[0].Start();
            tasks[1].Start();
            tasks[2].Start();
            tasks[3].Start();

            Task.WaitAll(tasks);
           

        }

        public void Left()
        {// soldan tarama

            BitmapSettings bmps = new BitmapSettings(bmp1);
            bmps.LockBits();
            
                for (int x = 1; x < bmp1.Width - 1; x++)
                {
                    for (int y = 0; y < bmp1.Height; y++)
                    {
                        lock (_LOCK)
                        {
                           

                        
                           if (bmps.GetPixel(x, y) == Color.FromArgb(255, 255, 255))
                           {
                            LeftHis[x] += 1;
                           }
                         
                        }

                    }

                    if (LeftHis[x] > setoff_x && LeftHis[x - 1] <= setoff_x)
                    {
                        Koordinat[1] = x;//x_min
                    }

                
            }

                bmps.UnlockBits();

            if (LeftHis[2] >= setoff_bilek) { /* bilek fonksiyonunu çağır*/   Koordinat[1] = bilek_bul(LeftHis, 0); yon = 1; }
        }

        public void Right()
        {   ///sağdan tarama
            BitmapSettings bmps = new BitmapSettings(bmp2);
            bmps.LockBits();

                for (int x = bmp2.Width - 2; x >= 0; x--)
                {
                    for (int y = 0; y < bmp2.Height; y++)
                    {
                        lock (_LOCK)
                        {
                          if (bmps.GetPixel(x, y) == Color.FromArgb(255, 255, 255))
                          {
                            RightHis[x] += 1;
                          }
                        }
                    }

                    if (first_xmax == 0 && RightHis[x] > setoff_x && RightHis[x + 1] <= setoff_x)
                    {
                        first_xmax = 1;
                        Koordinat[0] = x;//x_max


                    }


                
            }

                bmps.UnlockBits();


            if (RightHis[bmp2.Width - 3] >= setoff_bilek)
            { /* bilek fonksiyonunu çağır*/    Koordinat[0] = bilek_bul(RightHis, 1); yon = 2; }


        }

        public void Up()
        {   ///Yukarıdan tarama
            ///
            BitmapSettings bmps = new BitmapSettings(bmp3);
            bmps.LockBits();
           
                for (int y = 1; y < bmp3.Height - 1; y++)
                {
                    for (int x = 0; x < bmp3.Width; x++)
                    {
                        lock (_LOCK)
                        {

                          if (bmps.GetPixel(x, y) == Color.FromArgb(255, 255, 255))
                          {
                             UpHis[y] += 1;
                           }
                        }
                    }

                    if (first_ymin == 0 && UpHis[y] > setoff_y && UpHis[y - 1] <= setoff_y)
                    {
                        first_ymin = 1;
                        Koordinat[3] = y;//y_min

                    }


                
            }
                bmps.UnlockBits();

            if (UpHis[2] >= setoff_bilek) { /* bilek fonksiyonunu çağır*/  Koordinat[3] = bilek_bul(UpHis, 0); yon = 4; }


        }

        public void Down()
        {   ///Aşağıdan tarama
            BitmapSettings bmps = new BitmapSettings(bmp4);
            bmps.LockBits();
                for (int y = bmp4.Height - 2; y >= 0; y--)
                {
                    for (int x = 0; x < bmp4.Width; x++)
                    {

                        lock (_LOCK)
                        {
                          if (bmps.GetPixel(x, y) == Color.FromArgb(255, 255, 255))
                           {
                            DownHis[y] += 1;
                           }
                        }
                    }

                    if (first_ymax == 0 && DownHis[y] > setoff_y && DownHis[y + 1] <= setoff_y)
                    {
                        first_ymax = 1;
                        Koordinat[2] = y;//ymax
                    }

                
            }

                bmps.UnlockBits();

            if (DownHis[bmp4.Height - 3] >= 30)
            { /* bilek fonksiyonunu çağır*/  Koordinat[2] = bilek_bul(DownHis, 1); yon = 3; }


        }

        public int bilek_bul(int[] histogram, int yon)
        {

            int kesim_noktasi = 20;
            int t = 0;
            if (yon == 1)
            {
                for (int i = histogram.Length - 3; i > 0; i--)/////aşağı ve sağ için
                {
                    t = histogram[i] - histogram[histogram.Length - 2];
                    if (histogram[i] - histogram[histogram.Length - 2] > 4)//4 değişebilir
                    {

                        kesim_noktasi = i;
                        break;
                    }

                }
            }

            if (yon == 0)
            {
                for (int i = 1; i < histogram.Length; i++)///sol ve yukarı
                {

                    if (histogram[i] - histogram[2] > 4)//4 değişebilir
                    {

                        kesim_noktasi = i;
                        break;
                    }

                }

            }
            return kesim_noktasi;
        }

          ~HandMask () // Destructor
        {
            
            GC.Collect(); // GC(Garbage collector)'in zorunlu olarak bellekteki gereksiz objeleri kaldırması istenebilir.
           
           
        }
    }


}
