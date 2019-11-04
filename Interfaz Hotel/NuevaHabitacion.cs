using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MySql.Data;
using MySql.Data.MySqlClient;

namespace Interfaz_Hotel
{
    public partial class NuevaHabitacion : Form
    {
        public NuevaHabitacion()
        {
            InitializeComponent();
        }

        private void NuevaHabitacion_FormClosing(object sender, FormClosingEventArgs e)
        {
            AdministracionHotel form = new AdministracionHotel();
            form.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] bytes;
            bytes = Encoding.UTF8.GetBytes(textBox1.Text);
            textBox1.Text = Encoding.Default.GetString(bytes);

            if (textBox1.Text != "")
            {
                if (comprobar(textBox1.Text))
                {
                    string connStr = "server=localhost;user=root;database=dbHotel;port=3306;password=";
                    MySqlConnection conn = new MySqlConnection(connStr);
                    try
                    {
                        Console.WriteLine("Connecting to MySQL...");
                        conn.Open();

                        string sql = "INSERT INTO habitacion (nombre, costo) VALUES (\'" + textBox1.Text + "\', " + numericUpDown1.Value + ");";
                        MySqlCommand cmd = new MySqlCommand(sql, conn);
                        Console.WriteLine(sql);
                        Console.WriteLine(cmd.ExecuteNonQuery());

                        conn.Close();
                        Console.WriteLine("Done.");

                        MessageBox.Show("Habitacion agregada correctamente.");

                        AdministracionHotel form = new AdministracionHotel();
                        form.Show();
                        this.Hide();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        conn.Close();
                        MessageBox.Show("Error al agregar la habitacion.");
                    }
                }
                else
                {
                    MessageBox.Show("Nombre duplicado, porfavor elija otro.");
                }
            }
            else
            {
                MessageBox.Show("No puede haber campos vacios.");
            }
        }

        private bool comprobar (string nombre)
        {
            int num = 0;

            string connStr = "server=localhost;user=root;database=dbHotel;port=3306;password=";
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();

                string sql = "SELECT COUNT(*) as count FROM habitacion WHERE nombre LIKE \'" + nombre + "\';";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    num = rdr.GetInt32(rdr.GetOrdinal("count"));
                }
                rdr.Close();

                conn.Close();
                Console.WriteLine("Done.");

                return (num > 0) ? false : true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                conn.Close();

                return false;
            }
        }
    }
}
