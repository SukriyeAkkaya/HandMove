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

using AForge.Video;
using AForge.Video.DirectShow;

namespace Bitirme
{
    //find fingerprints
    class FingerPrints
    {

   
        //find fingertip countour
        public Bitmap finger_tip(Bitmap otsut, int x_max, int x_min, int y_max, int y_min)
        {

            int count_pixel;
            int deger;
            int limit_first;
            int limit_second;
            int aralik;

            BitmapSettings otsus = new BitmapSettings(otsut);
            otsus.LockBits();
            for (int x = x_min; x <= x_max; x++)
            {
                count_pixel = 0;

                aralik = 1;
                limit_first = 0;
                limit_second = 0;


                for (int y = y_max; y >= y_min; y--)//aşagıdan yukarı tarama
                {
                    if (otsus.GetPixel(x, y) == Color.FromArgb(255, 255, 255))
                    {
                        limit_first = y;
                        break;
                    }

                }

                for (int y = y_min; y <= y_max; y++)//yukarıdan aşağı tarama
                {
                    if (otsus.GetPixel(x, y) == Color.FromArgb(255, 255, 255))
                    {
                        limit_second = y;
                        break;
                    }
                }

                count_pixel = (limit_first - limit_second) + 1;



                for (int y = limit_first; y >= limit_second; y--)
                {

                    deger = (int)Math.Round(aralik * (255 / (double)count_pixel));
                    otsus.SetPixel(x, y, Color.FromArgb(deger, deger, deger));

                    aralik++;
                }

            }


            for (int y = y_min; y <= y_max; y = y + 1)
            {
                for (int x = x_min; x <= x_max; x = x + 1)
                {
                    if (otsus.GetPixel(x, y).R != 255)
                    {
                        otsus.SetPixel(x, y, Color.FromArgb(0, 0, 0));

                    }

                }
            }
            otsus.UnlockBits();
            return otsut;
        }

   
        
        
        public int[] FingersFind(Bitmap otsu, int x_max, int x_min, int y_max, int y_min)//parmak uçları olan bitmap
        {


              int[] parmak_xy = new int[10];//5 olacak
          
         
            Array.Clear(parmak_xy, 0, parmak_xy.Length);
          



            BitmapSettings otsus = new BitmapSettings(otsu);
            otsus.LockBits();

            int dizi_boyutu = Math.Abs(x_max - x_min);
         
         



            int[] beyazPiksel_Y = new int[dizi_boyutu + 10];
            int[] beyazPiksel_X = new int[dizi_boyutu + 10];
            int t = 0;

            for (int x = x_min; x < x_max; x++)
            {
                for (int y = y_min; y < y_max; y++)
                {
                    if (otsus.GetPixel(x, y).R == 255)
                    {
                        if(t<beyazPiksel_Y.Length){
                        beyazPiksel_Y[t] = y;
                        beyazPiksel_X[t] = x;
                        //otsus.SetPixel(x, y, Color.Blue);
                        t++;
                    }
                }
                 
                    for (int p = 0; p < beyazPiksel_Y.Length; p++)
                    {
                        if (beyazPiksel_Y[p] == 0) { beyazPiksel_Y[p] = 1000; beyazPiksel_X[p] = 1000; }
                    }

                }
            }
            beyazPiksel_Y[0] = 1000;
            beyazPiksel_X[0] = 1000;

            beyazPiksel_Y[1] = 1000;
            beyazPiksel_X[1] = 1000;

            
            
                parmak_xy[0] = beyazPiksel_Y.Min();
                parmak_xy[5] = beyazPiksel_X[beyazPiksel_Y.ToList().IndexOf(beyazPiksel_Y.Min())];

                int d = 25;
                int s = beyazPiksel_Y.ToList().IndexOf(beyazPiksel_Y.Min());
            for (int n = s -d; n < s + d; n++)
            {
                if (n > 0 && n < beyazPiksel_Y.Length)
                {
                   
                    beyazPiksel_Y[n] = 1000;
                    beyazPiksel_X[n] = 1000;

                }
            }

            
                parmak_xy[1] = beyazPiksel_Y.Min();  //y
                parmak_xy[6] = beyazPiksel_X[beyazPiksel_Y.ToList().IndexOf(beyazPiksel_Y.Min())];//x
            

            s = beyazPiksel_Y.ToList().IndexOf(beyazPiksel_Y.Min());
            for (int n = s - d; n < s + d; n++)
            {
                if (n > 0 && n < beyazPiksel_Y.Length)
                {
                    
                    beyazPiksel_Y[n] = 1000;
                    beyazPiksel_X[n] = 1000;

                }
            }

          


                parmak_xy[2] = beyazPiksel_Y.Min();
                parmak_xy[7] = beyazPiksel_X[beyazPiksel_Y.ToList().IndexOf(beyazPiksel_Y.Min())];
            
            s = beyazPiksel_Y.ToList().IndexOf(beyazPiksel_Y.Min());
            for (int n = s - d; n < s + d; n++)
            {
                if (n > 0 && n < beyazPiksel_Y.Length)
                {
                    
                    beyazPiksel_Y[n] = 1000;
                    beyazPiksel_X[n] = 1000;

                }
            }


           
                parmak_xy[3] = beyazPiksel_Y.Min();
                parmak_xy[8] = beyazPiksel_X[beyazPiksel_Y.ToList().IndexOf(beyazPiksel_Y.Min())];
            

            s = beyazPiksel_Y.ToList().IndexOf(beyazPiksel_Y.Min());
            for (int n = s - d; n < s + d; n++)
            {
                if (n > 0 && n < beyazPiksel_Y.Length)
                {
                    
                    beyazPiksel_Y[n] = 1000;
                    beyazPiksel_X[n] = 1000;

                }
            }

          
                parmak_xy[4] = beyazPiksel_Y.Min();
                parmak_xy[9] = beyazPiksel_X[beyazPiksel_Y.ToList().IndexOf(beyazPiksel_Y.Min())];
            
            s = beyazPiksel_Y.ToList().IndexOf(beyazPiksel_Y.Min());
            for (int n = s - d; n < s + d; n++)
            {
                if (n > 0 && n < beyazPiksel_Y.Length)
                {
                    beyazPiksel_Y[n] = 1000;
                    beyazPiksel_X[n] = 1000;

                }
            }


            otsus.UnlockBits();


            return parmak_xy;

        }

     

      

        public int[] finger_degree(Bitmap otsu1,int [] parmak_x, int [] parmak_y,int x_max, int x_min, int y_max, int y_min)
        {
            int x;
            int y;
            int[] parmak_bölge = new int[5];
   

            for (int t = 360; t > 0; t = t - 1)
            {
                //////////
                x = (int)(x_min + ((x_max - x_min) / 2) + (Math.Cos(t) * ((x_max - x_min) /2-5)));//((x_max - x_min) /2)-10));
                y = (int)((y_max -(80) - Math.Abs(Math.Sin(t) * (x_max - x_min) / 3)));//değiştir
                ///////////////
            

                BitmapSettings otsu2 = new BitmapSettings(otsu1);
                otsu2.LockBits();
               
                if (y  > y_min) { otsu2.SetPixel(x, y, Color.Green); }
              
                if (y - 20 > y_min) { otsu2.SetPixel(x, y-30 , Color.Green); }
                              
               otsu2.UnlockBits();

                if (y_max - y_min <= 120) {//tamamen kapalı el
                    parmak_bölge[0] = 2;

                    parmak_bölge[1] = 2;

                    parmak_bölge[2] = 2;

                    parmak_bölge[3] = 2;

                    parmak_bölge[4] = 2;
                }


                if (parmak_y[0] == 1000) { parmak_bölge[0] = 2; }
                if (parmak_y[1] == 1000) { parmak_bölge[1] = 2; }
                if (parmak_y[2] == 1000) { parmak_bölge[2] = 2; }
                if (parmak_y[3] == 1000) { parmak_bölge[3] = 2; }
                if (parmak_y[4] == 1000) { parmak_bölge[4] = 2; }
             
                if (x == parmak_x[0])
                {

                     if ((parmak_y[0] >= y)) { parmak_bölge[0] = 2; }  //kapalı parmak
                    else if ((parmak_y[0] < y) && (parmak_y[0] >= y -40)) { parmak_bölge[0] = 1; }            
                    else if( parmak_y[0] < y - 40 ){ parmak_bölge[0] = 0; } // (açık parmak dereceyi 0 ile çarpacak motor dönmeyecek)
                }

                else if (x == parmak_x[1])
                {
                     if ((parmak_y[1] >= y)) { parmak_bölge[1] = 2; }  //kapalı parmak
                   else if ((parmak_y[1] < y) && (parmak_y[1] >= y - 40)) { parmak_bölge[1] = 1; }                 
                   else if ((parmak_y[1] < y - 40)) { parmak_bölge[1] = 0; } 
                }

                else if (x == parmak_x[2])
                {
                    
                 if ((parmak_y[2] >= y)) { parmak_bölge[2] = 2; }  //kapalı parmak
                   else if ((parmak_y[2] < y) && (parmak_y[2] >= y - 40)) { parmak_bölge[2] = 1; }
                   else if ((parmak_y[2] < y - 40)) { parmak_bölge[2] = 0; } 
                }

                else if (x == parmak_x[3])
                {
                     if ((parmak_y[3] >= y)) { parmak_bölge[3] = 2; }  //kapalı parmak
                   else if ((parmak_y[3] < y) && (parmak_y[3] >= y -40)) { parmak_bölge[3] = 1; }
                   else if ((parmak_y[3] < y - 40)) { parmak_bölge[3] = 0; } 
                }

                else if (x == parmak_x[4])
                {
                    if ((parmak_y[4] >= y)) { parmak_bölge[4] = 2; }  //kapalı parmak
                   else if ((parmak_y[4] < y) && (parmak_y[4] >= y - 40)) { parmak_bölge[4] = 1; }
                   else if ((parmak_y[4] < y - 40)) { parmak_bölge[4] = 0; } 
                }
            }


            return parmak_bölge;
            //çıkan değerleri dereceyle çarpıp motora gönder.  örn: parmak_bölge[0]*10; motor 10 un katlarıyla döner
        }

        ~FingerPrints() // Destructor
        {

            GC.Collect(); // GC(Garbage collector)'in zorunlu olarak bellekteki gereksiz objeleri kaldırması istenebilir.


        }





    }
}

