using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Configuration;
using System.IO;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;
using ProyectoCDM.Reconitions;

namespace ProyectoCDM.MVM.View
{
    /// <summary>
    /// Lógica de interacción para PetRegisterView.xaml
    /// </summary>
    public partial class PetRegisterView : UserControl
    {


        static string folderPath = System.IO.Path.Combine(Environment.GetFolderPath
            (Environment.SpecialFolder.ApplicationData), "Codigo_CDM_Mascotas");
        static string folderPathPDF = System.IO.Path.Combine(Environment.GetFolderPath
            (Environment.SpecialFolder.MyDocuments), "CDM");
        private GlyphDatabases glyphDatabases = new GlyphDatabases();
        public string nombremascota="";
        public byte[,] matris;
        public BitmapImage im;
        public PetRegisterView()
        {
            InitializeComponent();
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);

            }
            if (!Directory.Exists(folderPathPDF))
            {
                Directory.CreateDirectory(folderPathPDF);

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int id=-1;
                nombremascota = txbnombremascota.Text;
                using (SqlConnection conection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\opali\Downloads\ProyectoCDMOriginalV2\ProyectoCDM\ProyectoCDM\DataBase\Bdd.mdf;Integrated Security=True;Connect Timeout=30"))
                {
                    conection.Open();
                    // prmero iserto el codigo
                    SqlCommand cmd = new SqlCommand("insert into Codigo(Codigo) values('" + nombremascota + "')", conection);
                    cmd.ExecuteNonQuery();
                    //
                    /// despue ago un selec para que extraiga el id del codigo de la mascota en 
                    /// 
                    SqlCommand cmdidMascota = new SqlCommand("SELECT IdCodigo From Codigo WHERE Codigo ='"+nombremascota+"' ", conection);
                    SqlDataReader dr = cmdidMascota.ExecuteReader();
                    dr.Read();
                    id = (Convert.ToInt32(dr["IdCodigo"]));
                    Console.WriteLine(id);
                    conection.Close();
                    //
                    // inserto a mascota nombre ma s id
                    if (id != -1)
                    {
                        conection.Open();
                        SqlCommand cmd2 = new SqlCommand("insert into Mascota(NombreMascota,Idcodigo) values('" + nombremascota + "'," + id + ")", conection);
                        cmd2.ExecuteNonQuery();
                        conection.Close();

                    }
                    else
                    {
                        MessageBox.Show("no se pudo insertar a mascota");
                       
                    }

                   
                }
                // para guardar el 
                var UtimoNameImg = Directory.GetFiles(folderPath).Select(fn => new FileInfo(fn)).Where(ft => ft.Extension == ".png").OrderBy(f => f.Name).LastOrDefault();
                int numero;
                if (UtimoNameImg == null)
                {
                    numero = 1;
                }
                else
                {
                    numero = int.Parse(UtimoNameImg.Name.Split('.')[0]) + 1;
                }

                Bitmap img = BitmapImage2Bitmap(im);
                string destino = System.IO.Path.Combine(folderPath, numero.ToString() + ".png");
                img.Save(destino, ImageFormat.Png);
                string destinopdf = System.IO.Path.Combine(folderPathPDF, nombremascota + "_Codigo.png");
                img.Save(destinopdf, ImageFormat.Png);
                if (nombremascota != "" && matris != null)
                {
                    glyphDatabases.AddGlyph(nombremascota, matris);
                    System.Windows.MessageBox.Show("se guardo el codigo");
                    MessageBox.Show("Mascota: " + nombremascota + " Fue añadido correctamente, Vea la pestaña de View Security designar el acceso a las habitaciones Permitidas");

                }
                else
                {
                    System.Windows.MessageBox.Show("no se lleno un campo");
                }


               
            }
            catch (Exception)
            {

                MessageBox.Show("no se pudo registrar ");
            }
         }

        private void BtnGenerar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                im = CDM();
                imgGenrador.Source = im;
                btnAgregar.IsEnabled = true;
            }
            catch (Exception)
            {

                MessageBox.Show("no se puede generrar el codigo");
            }
        }

        public BitmapImage CDM()
        {
            Random rt = new Random();

            matris = new byte[5, 5];


            for (int x = 0; x < matris.GetLength(0); x++)
            {

                for (int y = 0; y < matris.GetLength(1); y++)
                {

                    if (x >= 1 && x <= 3 && y >= 1 && y <= 3)
                    {
                        matris[x, y] = (byte)rt.Next(0, 2);

                    }

                    else
                    {

                        matris[x, y] = 0;
                    }

                }

            }
            //fila
            if (matris[1, 1] == 0 && matris[1, 2] == 0 && matris[1, 3] == 0)
            {
                matris[1, 1] = 1;

            }
            if (matris[2, 1] == 0 && matris[2, 2] == 0 && matris[2, 3] == 0)
            {
                matris[2, 2] = 1;


            }
            if (matris[3, 1] == 0 && matris[3, 2] == 0 && matris[3, 3] == 0)
            {
                matris[3, 3] = 1;

            }
            ////
            ///
            //columna
            if (matris[1, 1] == 0 && matris[2, 1] == 0 && matris[3, 1] == 0)
            {
                matris[1, 1] = 1;

            }
            if (matris[1, 2] == 0 && matris[2, 2] == 0 && matris[3, 2] == 0)
            {
                matris[2, 2] = 1;


            }
            if (matris[1, 3] == 0 && matris[2, 3] == 0 && matris[3, 3] == 0)
            {
                matris[3, 3] = 1;

            }

    
            Bitmap bitmap = new Bitmap(200, 200);


            for (int Xcount = 0; Xcount < bitmap.Width; Xcount++)
            {
                for (int Ycount = 0; Ycount < bitmap.Height; Ycount++)
                {


                    if (Xcount >= 40 && Xcount < 80 && Ycount >= 40 && Ycount < 80 && matris[1, 1] == 0)
                    {
                        bitmap.SetPixel(Xcount, Ycount, System.Drawing.Color.Black);
                    }
                    else if (Xcount >= 40 && Xcount < 80 && Ycount >= 40 && Ycount < 80 && matris[1, 1] == 1)
                    {
                        bitmap.SetPixel(Xcount, Ycount, System.Drawing.Color.White);
                    }
                    else if (Xcount >= 40 && Xcount < 80 && Ycount >= 80 && Ycount < 120 && matris[2, 1] == 0)
                    {
                        bitmap.SetPixel(Xcount, Ycount, System.Drawing.Color.Black);
                    }
                    else if (Xcount >= 40 && Xcount < 80 && Ycount >= 80 && Ycount < 120 && matris[2, 1] == 1)
                    {
                        bitmap.SetPixel(Xcount, Ycount, System.Drawing.Color.White);
                    }
                    else if (Xcount >= 40 && Xcount < 80 && Ycount >= 120 && Ycount < 160 && matris[3, 1] == 0)
                    {
                        bitmap.SetPixel(Xcount, Ycount, System.Drawing.Color.Black);
                    }
                    else if (Xcount >= 40 && Xcount < 80 && Ycount >= 120 && Ycount < 160 && matris[3, 1] == 1)
                    {
                        bitmap.SetPixel(Xcount, Ycount, System.Drawing.Color.White);
                    }

                    //-----------------------------------------------------------------------------------------------

                    else if (Xcount >= 80 && Xcount < 120 && Ycount >= 40 && Ycount < 80 && matris[1, 2] == 0)
                    {
                        bitmap.SetPixel(Xcount, Ycount, System.Drawing.Color.Black);
                    }
                    else if (Xcount >= 80 && Xcount < 120 && Ycount >= 40 && Ycount < 80 && matris[1, 2] == 1)
                    {
                        bitmap.SetPixel(Xcount, Ycount, System.Drawing.Color.White);
                    }
                    else if (Xcount >= 80 && Xcount < 120 && Ycount >= 80 && Ycount < 120 && matris[2, 2] == 0)
                    {
                        bitmap.SetPixel(Xcount, Ycount, System.Drawing.Color.Black);
                    }
                    else if (Xcount >= 80 && Xcount < 120 && Ycount >= 80 && Ycount < 120 && matris[2, 2] == 1)
                    {
                        bitmap.SetPixel(Xcount, Ycount, System.Drawing.Color.White);
                    }
                    else if (Xcount >= 80 && Xcount < 120 && Ycount >= 120 && Ycount < 160 && matris[3, 2] == 0)
                    {
                        bitmap.SetPixel(Xcount, Ycount, System.Drawing.Color.Black);
                    }
                    else if (Xcount >= 80 && Xcount < 120 && Ycount >= 120 && Ycount < 160 && matris[3, 2] == 1)
                    {
                        bitmap.SetPixel(Xcount, Ycount, System.Drawing.Color.White);
                    }
                    //----------------------------------------------------------------------

                    else if (Xcount >= 120 && Xcount < 160 && Ycount >= 40 && Ycount < 80 && matris[1, 3] == 0)
                    {
                        bitmap.SetPixel(Xcount, Ycount, System.Drawing.Color.Black);
                    }
                    else if (Xcount >= 120 && Xcount < 160 && Ycount >= 40 && Ycount < 80 && matris[1, 3] == 1)
                    {
                        bitmap.SetPixel(Xcount, Ycount, System.Drawing.Color.White);
                    }
                    else if (Xcount >= 120 && Xcount < 160 && Ycount >= 80 && Ycount < 120 && matris[2, 3] == 0)
                    {
                        bitmap.SetPixel(Xcount, Ycount, System.Drawing.Color.Black);
                    }
                    else if (Xcount >= 120 && Xcount < 160 && Ycount >= 80 && Ycount < 120 && matris[2, 3] == 1)
                    {
                        bitmap.SetPixel(Xcount, Ycount, System.Drawing.Color.White);
                    }
                    else if (Xcount >= 120 && Xcount < 160 && Ycount >= 120 && Ycount < 160 && matris[3, 3] == 0)
                    {
                        bitmap.SetPixel(Xcount, Ycount, System.Drawing.Color.Black);
                    }
                    else if (Xcount >= 120 && Xcount < 160 && Ycount >= 120 && Ycount < 160 && matris[3, 3] == 1)
                    {
                        bitmap.SetPixel(Xcount, Ycount, System.Drawing.Color.White);
                    }

                    else
                    {
                        bitmap.SetPixel(Xcount, Ycount, System.Drawing.Color.Black);
                    }

                }


            }


            return ToBitmapImage(bitmap);
        }

        #region
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
        private Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {

            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);
                return new Bitmap(bitmap);
            }
        }
        #endregion

    }
}
