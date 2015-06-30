using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Collections.Concurrent;
using System.Threading.Tasks;

using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Vision.Motion;

using System.Threading;

using System.IO.Ports;





namespace Bitirme
{
    public partial class Form1 : Form
    {

        private FilterInfoCollection VideoCaptureDevices;
        private VideoCaptureDevice FinalVideo;
        private Bitmap frame;
        private Bitmap frame_al;
        private Bitmap frame_al1;
        private int i;
       
        private MotionDetector detector = new MotionDetector( new TwoFramesDifferenceDetector(), new MotionAreaHighlighting());//Hareketi algılar

        ConcurrentQueue<Bitmap> ImageQueue;
        ConcurrentQueue<Bitmap> ImageQueue_reel;
        ConcurrentQueue<Bitmap> ImageQueue_reel1;//izohips çizmek için
        ConcurrentQueue<Bitmap> ImageQueue_otsu;
        ConcurrentQueue<Bitmap> ImageQueue_otsu1;
        ConcurrentQueue<Bitmap> ImageQueue_finger;
        ConcurrentQueue<int[]> ImageQueue_hand;
        ConcurrentQueue<int[]> ImageQueue_hand1;
        ConcurrentQueue<int[]> ImageQueue_bolge;

        private GreyScala Gri;
        private OtsuScala Otsu;
        private HandMask Mask;
        private FingerPrints Finger;
   
        private static object _LOCK;


        private SerialPort port;

        int[] parmak_bolge1; 

        public Form1()
        {
            InitializeComponent();



            Gri = new GreyScala();
            Otsu = new OtsuScala();

            _LOCK = new object();

            Finger = new FingerPrints();
           parmak_bolge1 = new int[5];

            ImageQueue = new ConcurrentQueue<Bitmap>();
            ImageQueue_reel = new ConcurrentQueue<Bitmap>();
            ImageQueue_reel1 = new ConcurrentQueue<Bitmap>();
            ImageQueue_otsu = new ConcurrentQueue<Bitmap>();
            ImageQueue_otsu1 = new ConcurrentQueue<Bitmap>();
            ImageQueue_hand = new ConcurrentQueue<int[]>();
            ImageQueue_hand1 = new ConcurrentQueue<int[]>();
            ImageQueue_bolge = new ConcurrentQueue<int[]>();
            ImageQueue_finger = new ConcurrentQueue<Bitmap>();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            port = new SerialPort("COM13", 9600);//Com portunu doğru gir
            try
            {
                if (port.IsOpen == false)
                    port.Open();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);

            }

            //kamera isimlerini alir combobox a atar.
            VideoCaptureDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
           
            foreach (FilterInfo VideoCaptureDevice in VideoCaptureDevices)
            {

                combo_dev.Items.Add(VideoCaptureDevice.Name);
            }
            combo_dev.SelectedIndex = 0;
             i = 0;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Kamerayi Baslatma
            FinalVideo = new VideoCaptureDevice(VideoCaptureDevices[combo_dev.SelectedIndex].MonikerString);
            FinalVideo.VideoResolution = FinalVideo.VideoCapabilities[2];
            FinalVideo.NewFrame += new NewFrameEventHandler(FinalVideo_NewFrame);

            FinalVideo.Start();


        }

        void FinalVideo_NewFrame(object sender, NewFrameEventArgs eventArgs)//event func
        {
            frame = (Bitmap)eventArgs.Frame.Clone();
            Bitmap otsu1;

            //Elinizi isaretli bölgeye yerlestirdikten sonra isleme tusuna basiniz
            //parmak ucu belirlerken +-30 siliniyor

            otsu1 = (Bitmap)frame.Clone();


            Otsu.otsu_uygulama(otsu1);
            otsu_pcbx.Image = otsu1;

            Graphics gx = Graphics.FromImage(frame);
            Pen mypen = new Pen(Color.Yellow, 2);

            gx.DrawEllipse(mypen, 60, 135, 2, 2);
            gx.DrawEllipse(mypen, 80, 106, 2, 2);
            gx.DrawEllipse(mypen, 125, 90, 2, 2);
            gx.DrawEllipse(mypen, 160, 92, 2, 2);
            gx.DrawEllipse(mypen, 200, 142,2, 2);

         
            video_box.Image = frame;
      }

   
        void FinalVideo_Kuyruk(object sender, NewFrameEventArgs eventArgs)//event func
        {
            frame_al = (Bitmap)eventArgs.Frame.Clone();
            frame_al1 =(Bitmap)frame_al.Clone();
            i++;
            if ((detector.ProcessFrame(frame_al) > 0.001) && i%10==0)////
            {
                
                    ImageQueue.Enqueue(frame_al1);
                
            }
            
                
            
        }
        private void button3_Click(object sender, EventArgs e)
        {

            //tasklari tanimla
            //1. task kuyruga frame ekler
            Task take_frame = Task.Factory.StartNew(() =>
            {
                FinalVideo.NewFrame += new NewFrameEventHandler(FinalVideo_Kuyruk);

            });

            //2.task otsu-gri
            Task otsu_gri = Task.Factory.StartNew(() =>
            {
                Bitmap reel;
                Bitmap otsu;
               
                while (true)
                {
                    if (!(ImageQueue.IsEmpty))
                    {

                       

                        lock (_LOCK)
                        {
                            ImageQueue.TryDequeue(out reel);
                            otsu = (Bitmap)reel.Clone();
                            ImageQueue_reel.Enqueue(reel);
                            Otsu.otsu_uygulama(otsu);
                            ImageQueue_otsu.Enqueue(otsu);
                        }

                     


                    }
                }
            });
            //3.task hand area
            Task handselect = Task.Factory.StartNew(() =>
            {
                Bitmap otsu;
                Bitmap otsu1;

                int[] koor = new int[4];

                while (true)
                {
                    if (!(ImageQueue_otsu.IsEmpty) )
                    {

               

                        lock (_LOCK)
                        {
                            ImageQueue_otsu.TryDequeue(out otsu);
                            otsu1 = (Bitmap)otsu.Clone();
                            Mask = new HandMask(otsu1);
                            Mask.HandSelect();



                            koor[0] = Mask.Koordinat[0];//xmax
                            koor[1] = Mask.Koordinat[1];//xmin
                            koor[2] = Mask.Koordinat[2];//ymax
                            koor[3] = Mask.Koordinat[3];//ymin
                          

                           ImageQueue_hand.Enqueue(koor);


                           
                          ImageQueue_otsu1.Enqueue(otsu1);

                            
                          

                        }
                     
                    }
                }
            });

            ////4. task fingertrip
            Task finger_tip = Task.Factory.StartNew(() =>
            {
                Bitmap otsu;
                Bitmap otsu1;

                int[] koor_ = new int[4];


                while (true)
                {
                    if (!(ImageQueue_hand.IsEmpty) && !(ImageQueue_otsu1.IsEmpty))
                    {

                    


                        lock (_LOCK)
                        {
                            ImageQueue_hand.TryDequeue(out koor_);

                          

                            ImageQueue_otsu1.TryDequeue(out otsu);
                            otsu1 = (Bitmap)otsu.Clone();
                            Finger.finger_tip(otsu1, koor_[0], koor_[1], koor_[2], koor_[3]);
                            ImageQueue_finger.Enqueue(otsu1);
                            //  GC.Collect();
                            ImageQueue_hand1.Enqueue(koor_);

                          

                        }




                    }
                }
            });


            Task fingertrip = Task.Factory.StartNew(() =>
            {

                Bitmap reel1;
                Bitmap reel2;
                //Bitmap reel3;
                Bitmap finger1;

                int[] koor_1 = new int[4];
               

                while (true)
                {
                    if (!(ImageQueue_hand1.IsEmpty) && !(ImageQueue_reel.IsEmpty) && !(ImageQueue_finger.IsEmpty))
                    {

                        lock (_LOCK)
                        {
                            ImageQueue_reel.TryDequeue(out reel1);
                            reel2 = (Bitmap)reel1.Clone();
                            ImageQueue_hand1.TryDequeue(out koor_1);


                            ImageQueue_finger.TryDequeue(out finger1);
                            finger1 = (Bitmap)finger1.Clone();

                            int[] parmak_xy = Finger.FingersFind(finger1, koor_1[0], koor_1[1], koor_1[2], koor_1[3]);

                            int[] parmak_x = new int[5];
                            int[] parmak_y = new int[5];
                            int[] parmaky1 = new int[5];
                            int[] parmakx1;
                            for (int k = 0; k < 5; k++)
                            {
                                parmak_y[k] = parmak_xy[k];
                            }
                            for (int k = 0; k < 5; k++)
                            {
                                parmak_x[k] = parmak_xy[k + 5];
                            }

                            parmakx1 = (int[])parmak_x.Clone();
                            int index;
                            Array.Sort(parmak_x);
                            for (int l = 0; l < 5; l++)
                            {

                                index = parmakx1.ToList().IndexOf(parmak_x[l]);
                                parmaky1[l] = parmak_y[index];
                            }


                            parmak_y = parmaky1;

                            int boy = koor_1[0] - koor_1[1];
                            int oran = boy / 5;
                         



                            parmak_bolge1 = Finger.finger_degree(reel2, parmak_x, parmak_y, koor_1[0], koor_1[1], koor_1[2], koor_1[3]);



                            Graphics g = Graphics.FromImage(reel2);
                            SolidBrush drawBrush = new SolidBrush(Color.Red);
                            SolidBrush drawBrush1 = new SolidBrush(Color.Blue);
                            SolidBrush drawBrush2 = new SolidBrush(Color.Yellow);
                            SolidBrush drawBrush3 = new SolidBrush(Color.Orange);
                            SolidBrush drawBrush4 = new SolidBrush(Color.Green);

                            Pen mypen = new Pen(Color.Red, 3);
                            Pen mypen1 = new Pen(Color.Blue, 3);
                            Pen mypen2= new Pen(Color.Yellow, 3);
                            Pen mypen3 = new Pen(Color.Orange, 3);
                            Pen mypen4 = new Pen(Color.Green, 3);
                            Font font = new Font("Arial", 24);
                            g.DrawRectangle(mypen, koor_1[1], koor_1[3], koor_1[0] - koor_1[1], koor_1[2] - koor_1[3]);
                          

                            g.DrawEllipse(mypen, parmak_x[0], parmak_y[0], 2, 2);
                            g.DrawEllipse(mypen1, parmak_x[1], parmak_y[1], 2, 2);
                            g.DrawEllipse(mypen2, parmak_x[2], parmak_y[2], 2, 2);
                            g.DrawEllipse(mypen3, parmak_x[3], parmak_y[3], 2, 2);
                            g.DrawEllipse(mypen4, parmak_x[4], parmak_y[4], 2, 2);


                            g.DrawString(parmak_bolge1[0].ToString(), font, drawBrush, 2 * 1 * 10, 2);
                            g.DrawString(parmak_bolge1[1].ToString(), font, drawBrush1, 2 * 2 * 10, 2);
                            g.DrawString(parmak_bolge1[2].ToString(), font, drawBrush2, 2 * 3 * 10, 2);
                            g.DrawString(parmak_bolge1[3].ToString(), font, drawBrush3, 2 * 4 * 10, 2);
                            g.DrawString(parmak_bolge1[4].ToString(), font, drawBrush4, 2 * 5 * 10, 2);


                            gri_pcx.Image = reel2;


                        }
                    }
                }

            });
           

        }

       

        private void otsu_pcbx_Click(object sender, EventArgs e)
        {

        }

        private void gri_pcx_Click(object sender, EventArgs e)
        {

        }

        private void video_box_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

            FinalVideo.Stop();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ///Porta yazma,
            ///
            try
            {
                if (port.IsOpen == false)
                    port.Open();

                port.Write("" + parmak_bolge1[0]);
                port.Write("" + parmak_bolge1[1]);
                port.Write("" + parmak_bolge1[2]);
                port.Write("" + parmak_bolge1[3]);
                port.Write("" + parmak_bolge1[4]);

                port.Close();


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);

            }
        
        }


    }
}