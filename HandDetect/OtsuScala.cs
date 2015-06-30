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


using AForge.Video;
using AForge.Video.DirectShow;

namespace Bitirme
{
    class OtsuScala
    {

          GreyScala Gri;
     

           public int OtsuEsiginiBelirle(int [] dizi_histogram, Bitmap video) {

                 double minWCV = 1000000;
                 int esik=-1;
                 double yeniWCV = 0;
                 double wb=0,mb=0, vb=0, wf=0,mf=0, vf=0;
                 int toplampixel_sayisi = video.Height * video.Width;

                for (int i = 0; i < dizi_histogram.Length; i++){
                    wb = Weight1(i, toplampixel_sayisi, dizi_histogram);
                    mb = Mean1(i, dizi_histogram);
                    vb = Variance1(i, mb, dizi_histogram);
                    wf = Weight2(i, toplampixel_sayisi, dizi_histogram);
                  mf = Mean2(i, dizi_histogram);
                  vf = Variance2(i, mf, dizi_histogram);
                 yeniWCV = WithinClassVariance(wb, vb, wf,vf);

                 if (yeniWCV<minWCV){
                 minWCV = yeniWCV;
                 esik = i;
                 }

                 }
                 return esik;
                 }

                #region Background W1,M1,V1

                public double Weight1(int pikselNo, int toplam_piksel_sayisi, int[] dizi_histogram) 
                 {
                 double sonuc = 0;
                 double toplam=0;

                 for (int i = 0; i < pikselNo; i++){
                 toplam += dizi_histogram[i];
                 }

                 sonuc = toplam / toplam_piksel_sayisi;
                 return sonuc;
                 }

                 public double Mean1(int pikselNo, int[] dizi_histogram)
                 {
                 double sonuc=0;
                 double toplam = 0;
                 double payToplam = 0;

                 for (int i = 0; i < pikselNo; i++){
                  toplam += dizi_histogram[i];
                  payToplam += i * dizi_histogram[i];
                 }

                 if (toplam!=0){
                  sonuc = payToplam / toplam;
                 }

                 return sonuc;
                 }

                 public double Variance1(int pikselNo, double mean, int[] dizi_histogram) 
                 {
                 double sonuc=0;
                 double toplam = 0;
                 double payToplam = 0;

                 for (int i = 0; i < pikselNo; i++){
                  toplam += dizi_histogram[i];
                  payToplam += Math.Pow((i-mean),2) * dizi_histogram[i];
                 }

                 if (toplam != 0){
                  sonuc = payToplam / toplam;
                 }
                 return sonuc;
                 }

                #endregion

                #region Foreground W2,M2,V2

                 public double Weight2(int pikselNo, int toplamPikselSayisi, int[] dizi_histogram)
                 {
                 double sonuc = 0;
                 double toplam = 0;

                for (int i = pikselNo; i < dizi_histogram.Length; i++){
                 toplam += dizi_histogram[i];
                }

                sonuc = toplam / toplamPikselSayisi;
                return sonuc;
                 }

                 public double Mean2(int pikselNo, int[] dizi_histogram)
                 {
                 double sonuc = 0;
                 double toplam = 0;
                 double payToplam = 0;

                 for (int i = pikselNo; i < dizi_histogram.Length; i++){
                 toplam += dizi_histogram[i];
                 payToplam += i * dizi_histogram[i];
                 }

                sonuc = payToplam / toplam;
                 return sonuc;
                 }

                 public double Variance2(int pikselNo, double mean, int[] dizi_histogram)
                 {
                 double sonuc = 0;
                 double toplam = 0;
                 double payToplam = 0;

                 for (int i = pikselNo; i < dizi_histogram.Length; i++){
                  toplam += dizi_histogram[i];
                  payToplam += Math.Pow((i - mean), 2) * dizi_histogram[i];
                 }
                 sonuc = payToplam / toplam;
                 return sonuc;
                 }
                 #endregion

                 public double WithinClassVariance(double w1, double v1, double w2, double v2 ) {

                  return((w1*v1 + w2*v2)); 
                 }

                 
        
                  public Bitmap otsu_uygulama(Bitmap video) {
                      Gri=new GreyScala();
                    //  Hsvv = new Hsv_();
                    //  int[] histogram1 = histogram(Gri.change_gray(Hsvv.ColorToHSV(video)));

                      int[] histogram1 = histogram(Gri.change_gray((video)));
                      int i = OtsuEsiginiBelirle(histogram1 , video);


                      Rectangle rect = new Rectangle(0, 0, video.Width, video.Height);
                      System.Drawing.Imaging.BitmapData bmpData =
                          video.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                         video.PixelFormat);

                      IntPtr ptr = bmpData.Scan0;

         
                      int bytes = Math.Abs(bmpData.Stride) * video.Height;
                      byte[] rgbValues = new byte[bytes];

                      System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

                      for (int counter = 0; counter < rgbValues.Length; counter += 3)
                      {
                          if (rgbValues[counter] < i)
                          {
                              rgbValues[counter] = 0;
                              rgbValues[counter + 1] =0;
                              rgbValues[counter + 2] = 0;
                          }
                          else {
                              rgbValues[counter] = 255;
                              rgbValues[counter + 1] = 255;
                              rgbValues[counter + 2] = 255;
                          }

                      }

                      System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);
                      video.UnlockBits(bmpData);

                      GC.Collect();
 
                      return video;
                 
                 
                 
                 }
        




        public int[] histogram(Bitmap video)
        {

            Rectangle rect = new Rectangle(0, 0, video.Width, video.Height);
            System.Drawing.Imaging.BitmapData bmpData =
                video.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
               video.PixelFormat);

            IntPtr ptr = bmpData.Scan0;

            int bytes = Math.Abs(bmpData.Stride) * video.Height;
            byte[] histogramValues = new byte[bytes];
            int[] histogram = new int[256];


            System.Runtime.InteropServices.Marshal.Copy(ptr, histogramValues, 0, bytes);

            for (int counter = 0; counter < histogramValues.Length; counter += 3)
            {
                histogram[histogramValues[counter]] =+1;    
                //System.Console.WriteLine(histogram[counter]);

            }

            



            System.Runtime.InteropServices.Marshal.Copy(histogramValues, 0, ptr, bytes);
            video.UnlockBits(bmpData);

            


            return histogram;
        }

        //int[] HistogramRed = new int[256];
        //int[] HistogramGreen = new int[256];
        //int[] HistogramBlue = new int[256];
        //int[] HistogramRedK = new int[256];
        //int[] HistogramGreenK = new int[256];
        //int[] HistogramBlueK = new int[256];
        //int[] YüzdelikRed = new int[256];
        //int[] YüzdelikGreen = new int[256];
        //int[] YüzdelikBlue = new int[256];    



        //public Bitmap histogramEşitleme(Bitmap video)
        //{
        //    Bitmap renderedImage = video;

        //    uint pixels = (uint)renderedImage.Height * (uint)renderedImage.Width;
        //    decimal Const = 255 / (decimal)pixels;

        //    int x, y, R, G, B;


        //    int[] HistogramRed2 = new int[256];
        //    int[] HistogramGreen2 = new int[256];
        //    int[] HistogramBlue2 = new int[256];


        //    for (var i = 0; i < renderedImage.Width; i++)
        //    {
        //        for (var j = 0; j < renderedImage.Height; j++)
        //        {
        //            var piksel = renderedImage.GetPixel(i, j);

        //            HistogramRed2[(int)piksel.R]++;
        //            HistogramGreen2[(int)piksel.G]++;
        //            HistogramBlue2[(int)piksel.B]++;

        //        }
        //    }

        //    int[] cdfR = HistogramRed2;
        //    int[] cdfG = HistogramGreen2;
        //    int[] cdfB = HistogramBlue2;

        //    for (int r = 1; r <= 255; r++)
        //    {
        //        cdfR[r] = cdfR[r] + cdfR[r - 1];
        //        cdfG[r] = cdfG[r] + cdfG[r - 1];
        //        cdfB[r] = cdfB[r] + cdfB[r - 1];
        //    }

        //    for (y = 0; y < renderedImage.Height; y++)
        //    {
        //        for (x = 0; x < renderedImage.Width; x++)
        //        {
        //            Color pixelColor = renderedImage.GetPixel(x, y);

        //            R = (int)((decimal)cdfR[pixelColor.R] * Const);
        //            G = (int)((decimal)cdfG[pixelColor.G] * Const);
        //            B = (int)((decimal)cdfB[pixelColor.B] * Const);

        //            Color newColor = Color.FromArgb(R, G, B);
        //            renderedImage.SetPixel(x, y, newColor);
        //        }
        //    }
        //    return renderedImage;
        //}

          ~OtsuScala () // Destructor
        {
            
            GC.Collect(); // GC(Garbage collector)'in zorunlu olarak bellekteki gereksiz objeleri kaldırması istenebilir.
           
           
        }
    }
}