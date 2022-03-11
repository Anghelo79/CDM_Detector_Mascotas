using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using System.Drawing;
using System.Drawing.Imaging;
using AForge.Video.DirectShow;
using System.Threading;
using GlyphRecognition;
using AForge.Imaging.Filters;
using ProyectoCDM.Reconitions;
using System.Data.SqlClient;
using System.Collections;

namespace ProyectoCDM.MVM.View
{
    /// <summary>
    /// Lógica de interacción para ViewSecurityView.xaml
    /// </summary>
    public partial class ViewSecurityView : UserControl
    {
        public class FrameData : EventArgs
        {
            private List<ExtractedGlyphData> glyphs;
            private Bitmap image;

            public FrameData(List<ExtractedGlyphData> _glyphs, Bitmap _image)
            {
                glyphs = _glyphs;
                image = _image;
            }

            public List<ExtractedGlyphData> getGlyphs()
            {
                return glyphs;
            }

            public Bitmap getImage()
            {
                return image;
            }
        }
        public event EventHandler<FrameData> frameProcessed;
        private GlyphImageProcessor imageProcessor = new GlyphImageProcessor();
        public BitmapImage bi;
        private object sync = new object();

        public ViewSecurityView()
        {
            InitializeComponent();
            loadd();
        }
        private FilterInfoCollection Dispositivos;
        private VideoCaptureDevice FuenteVideo;
        public string mascotacodigo;
        private void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                using (var bitmap = (Bitmap)eventArgs.Frame.Clone())
                {

                    if (bitmap.PixelFormat == System.Drawing.Imaging.PixelFormat.Format8bppIndexed)
                    {
                        // convert image to RGB if it is grayscale
                        GrayscaleToRGB filter = new GrayscaleToRGB();

                        Bitmap temp = filter.Apply(bitmap);
                        bitmap.Dispose();
                        bi = temp.ToBitmapImage();
                    }
                    lock (sync)
                    {
                        Bitmap b2 = new Bitmap(bitmap);
                        List<ExtractedGlyphData> glyphs = imageProcessor.ProcessImage(b2, out mascotacodigo);


                        EventHandler<FrameData> temp = frameProcessed;
                        if (temp != null)
                        {

                            temp(this, new FrameData(glyphs, b2));
                            
                        }


                        bi = b2.ToBitmapImage();
                    }




                }
                bi.Freeze();

                Dispatcher.BeginInvoke(new ThreadStart(delegate {
                    img1.Source = bi;
                    puertas(mascotacodigo);

                }));

            }
            catch (Exception ex)
            {
            }

        }
        private void Button_Click(object sender, RoutedEventArgs e)//on
        {
            FuenteVideo = new VideoCaptureDevice(Dispositivos[cbxmascota.SelectedIndex].MonikerString);
            FuenteVideo.NewFrame += new NewFrameEventHandler(video_NewFrame);
            FuenteVideo.Start();


        }

        public bool compararhabitaciones(int idhabitacion ,int idmascot)
        {
            bool estado = false;
            int count = 0;
            using (SqlConnection conection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\opali\Downloads\ProyectoCDMOriginalV2\ProyectoCDM\ProyectoCDM\DataBase\Bdd.mdf;Integrated Security=True;Connect Timeout=30"))
            {
                conection.Open();

                SqlCommand cmdidMascota = new SqlCommand("select IdMascota from Lista_De_Accesos where IdHabitacion = " + idhabitacion, conection);
                SqlDataReader dr = cmdidMascota.ExecuteReader();

                string vista = string.Empty;
                List<int> lis = new List<int>();
                while (dr.Read())
                {
                    for (int i = 0; i < dr.FieldCount; i++)
                    {
                        lis.Add(dr.GetInt32(i));
                    }
                }

                foreach (var item in lis)
                {
                    txblog.AppendText(""+item);
                }
                conection.Close();
                foreach (var item in lis)
                {
                    if (item==idmascot)
                    {
                        estado = true;
                        txblog.Text=("La Puerta se ha habierto");
                        break;
                    }
                }
                
            }
            if (estado==false)
            {
                txblog.Text = ("La mascota no tiene acceso ");
            }
            return estado;

        }
        public int puertas(string nombremascota)
        {
            int idret=0;
            if (nombremascota != "")
            {               
                using (SqlConnection conection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\opali\Downloads\ProyectoCDMOriginalV2\ProyectoCDM\ProyectoCDM\DataBase\Bdd.mdf;Integrated Security=True;Connect Timeout=30"))
                {
                conection.Open();

                SqlCommand cmdidMascota = new SqlCommand("select IdMascota from Mascota where NombreMascota = '"+ nombremascota+"'", conection);
                SqlDataReader dr = cmdidMascota.ExecuteReader();
                dr.Read();
                idret = (Convert.ToInt32(dr["IdMascota"]));

                conection.Close();
                
                }
            
            }
            return idret;
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
            
            Dispositivos = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            
            foreach (FilterInfo x in Dispositivos)
            {
                cbxmascota.Items.Add(x.Name);
            }
            cbxmascota.SelectedIndex = 0;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            compararhabitaciones(1, puertas(mascotacodigo));
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            compararhabitaciones(2, puertas(mascotacodigo));
        }

        private void RadioButton_Checked_2(object sender, RoutedEventArgs e)
        {
            compararhabitaciones(3, puertas(mascotacodigo));
        }

        private void RadioButton_Checked_3(object sender, RoutedEventArgs e)
        {
            compararhabitaciones(4, puertas(mascotacodigo));
        }

        private void RadioButton_Checked_4(object sender, RoutedEventArgs e)
        {
            compararhabitaciones(5, puertas(mascotacodigo));
        }

        private void RadioButton_Checked_5(object sender, RoutedEventArgs e)
        {
            compararhabitaciones(6, puertas(mascotacodigo));
        }

        private void RadioButton_Checked_6(object sender, RoutedEventArgs e)
        {
            compararhabitaciones(7, puertas(mascotacodigo));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
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
