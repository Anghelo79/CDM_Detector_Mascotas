using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Vision.Motion;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using AForge.Imaging.Filters;
using AForge.Imaging;
using System.Timers;

namespace ProyectoCDM.MVM.View
{
    /// <summary>
    /// Lógica de interacción para MoveDetectionView.xaml
    /// </summary>
    public partial class MoveDetectionView : UserControl
    {

        public MoveDetectionView()
        {
            InitializeComponent();
            loadd();
            
        }
        private FilterInfoCollection Dispositivos;
        private VideoCaptureDevice FuenteVideo;
        MotionDetector Detector;
        float NivelDeDeteccion;





        public void timerrr()
        {
           
        }
        private Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {

            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }
        public static BitmapImage ToBitmapImage(System.Drawing.Image bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;



                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();


                return bitmapImage;
            }
        }
        public void loadd()
        {
            Detector = new MotionDetector(new TwoFramesDifferenceDetector(),new MotionBorderHighlighting());
            Dispositivos = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            NivelDeDeteccion = 0;
            foreach (FilterInfo x in Dispositivos)
            {
                cb1.Items.Add(x.Name);
            }
            cb1.SelectedIndex = 0;
        }
        private void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                
               
                System.Drawing.Image img = (Bitmap)eventArgs.Frame.Clone();

                MemoryStream memorystream = new MemoryStream();
                img.Save(memorystream, ImageFormat.Bmp);
                memorystream.Seek(0, SeekOrigin.Begin);

                BitmapImage BitMapAux = new BitmapImage();
                
                BitMapAux.BeginInit();
                BitMapAux.StreamSource = memorystream;
                BitMapAux.EndInit();

                BitMapAux.Freeze();
                Dispatcher.BeginInvoke(new ThreadStart(delegate
                {
                    NivelDeDeteccion = Detector.ProcessFrame(BitmapImage2Bitmap(BitMapAux));
                    lab.Content =String.Format("{0:00.000} ",NivelDeDeteccion);

                    if (NivelDeDeteccion <= 0.1)
                    {
                        txb_log.Text = "Luz Apagada";
                    }
                    if (NivelDeDeteccion > 0.1)
                    {
                        txb_log.Text = "Luz Encendida";
                       
                    }
                if (NivelDeDeteccion == 0)
                    {
                       txb_time.Text  = "La Luz Se A pagara En 5 Segundos"; 
                        
                    }
                    
                    img1.Source = (BitMapAux);


                }));
            }
            catch (Exception ex)
            {
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)//on
        {
            
                FuenteVideo = new VideoCaptureDevice(Dispositivos[cb1.SelectedIndex].MonikerString);
                FuenteVideo.NewFrame += new NewFrameEventHandler(video_NewFrame);
                FuenteVideo.Start();
            
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)//off
        {
            if (!(FuenteVideo == null))
            {
                if (FuenteVideo.IsRunning)
                {
                    FuenteVideo.SignalToStop();
                    FuenteVideo = null;
                }
            }
            
        }
    }
}
