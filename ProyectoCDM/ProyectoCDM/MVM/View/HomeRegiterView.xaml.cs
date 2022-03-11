using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Configuration;

namespace ProyectoCDM.MVM.View
{
    /// <summary>
    /// Lógica de interacción para HomeRegiterView.xaml
    /// </summary>
    public partial class HomeRegiterView : UserControl
    {
        
        public HomeRegiterView()
        {
            InitializeComponent();
            cargaMascotas();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int conth = 0;
            int idmascota = cbxMascotas.SelectedIndex + 1;
            

            if (cb1.IsChecked == true)
            {
                insertarbdd(idmascota, 1);
                conth++;
            }
            if (cb2.IsChecked == true)
            {
                insertarbdd(idmascota, 2);
                conth++;
            }
            if (cb3.IsChecked == true)
            {
                insertarbdd(idmascota, 3);
                conth++;
            }
            if (cb4.IsChecked == true)
            {
                insertarbdd(idmascota, 4);
                conth++;
            }
            if (cb5.IsChecked == true)
            {
                insertarbdd(idmascota, 5);
                conth++;
            }
            if (cb6.IsChecked == true)
            {
                insertarbdd(idmascota, 6);
                conth++;
            }
            if (cb7.IsChecked == true)
            {
                insertarbdd(idmascota, 7);
                conth++;
            }

            MessageBox.Show("Mascota: " + cbxMascotas.SelectedItem + "  A sido asigando a "+conth +" Habitaciones");
        }
        public void insertarbdd(int idm, int idh) 
        {
            using (SqlConnection conection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\opali\Downloads\ProyectoCDMOriginalV2\ProyectoCDM\ProyectoCDM\DataBase\Bdd.mdf;Integrated Security=True;Connect Timeout=30"))
            {
                conection.Open();

                SqlCommand cmd = new SqlCommand("insert into Lista_De_Accesos(IdMascota,IdHabitacion) values(" + idm + "," + idh + ")", conection);
                cmd.ExecuteNonQuery();

                conection.Close();
            }
        }

        public void cargaMascotas()
        {
            using (SqlConnection conection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\opali\Downloads\ProyectoCDMOriginalV2\ProyectoCDM\ProyectoCDM\DataBase\Bdd.mdf;Integrated Security=True;Connect Timeout=30"))
            {
                conection.Open();

                SqlCommand cmd = new SqlCommand("SELECT * From Mascota", conection);
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    cbxMascotas.Items.Add(Convert.ToString(dr["NombreMascota"]));
                }
                conection.Close();
            }
        }





        //    using (SqlConnection conection = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\opali\Downloads\ProyectoCDM\ProyectoCDM\ProyectoCDM\DataBase\Bdd.mdf; Integrated Security = True"))
        //        {
        //            conection.Open();

        //            SqlCommand cmd = new SqlCommand("SELECT * From Habitacion", conection);
        //SqlDataReader dr = cmd.ExecuteReader();

        //            while (dr.Read())
        //            {
        //                Console.WriteLine(Convert.ToString(dr["NombreHabitacion"]));
        //            }
        //        }
    }
}
