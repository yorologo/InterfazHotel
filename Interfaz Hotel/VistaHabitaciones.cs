using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MySql.Data;
using MySql.Data.MySqlClient;

namespace Interfaz_Hotel
{
    public partial class VistaHabitaciones : Form
    {
        List<Button> botones = new List<Button>();

        public VistaHabitaciones()
        {
            InitializeComponent();

            int numHabitaciones = conteo();

            int filas = 1, columnas = 1;

            while (numHabitaciones > Math.Pow(columnas, 2)) columnas++;
            while (numHabitaciones > filas * columnas) filas++;

            int X = 600 / (columnas + 1);
            int Y = 600 / (filas + 1);
            int size = ((X < Y) ? X : Y) * 3 / 4;

            Console.WriteLine(numHabitaciones);
            Console.WriteLine(columnas + " " + filas);
            Console.WriteLine(X + " " + Y);
            Console.WriteLine(size);

            string connStr = "server=localhost;user=root;database=dbHotel;port=3306;password=";
            MySqlConnection conn = new MySqlConnection(connStr);

            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();

                string sql = "SELECT * FROM habitacion;";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                for (int i = 0, y = 0; i < numHabitaciones; y++)
                {
                    for (int x = 0; x < columnas && rdr.Read() && i < numHabitaciones; x++, i++)
                    {
                        botones.Add(new Button());
                        botones[i].Location = new Point(X * x + X * 5 / 8, Y * y + Y / 2);
                        botones[i].Height = size;
                        botones[i].Width = size;
                        botones[i].Text = rdr.GetString(rdr.GetOrdinal("id"));
                        if (rdr.GetBoolean(rdr.GetOrdinal("estado")))
                        {
                            botones[i].BackColor = System.Drawing.Color.Lime;
                            botones[i].Click += new EventHandler(reservacionInicio);
                        }
                        else
                        {
                            botones[i].BackColor = System.Drawing.Color.OrangeRed;
                            botones[i].Click += new EventHandler(reservacionTermina);
                        }

                        this.toolTip1.SetToolTip(botones[i], rdr.GetString(rdr.GetOrdinal("nombre")) + "\n" + rdr.GetString(rdr.GetOrdinal("descripcion")));
                        Controls.Add(botones[i]);
                    }
                }
                rdr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            conn.Close();
            Console.WriteLine("Done.");
        }


        private void reservacionInicio(object sender, EventArgs e)
        {
            string i = (sender as Button).Text;
            if (query("UPDATE habitacion SET estado = " + 0 + ", descripcion = 'Ocupado' WHERE id LIKE " + i + ";"))
            {
                if (query("INSERT INTO reservacion (fkIdHabitacion) VALUES (" + i + ");"))
                {
                    VistaHabitaciones form = new VistaHabitaciones();
                    form.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Error 2 al crear reservacion");
                }
            }
            else
            {
                MessageBox.Show("Error 1 al crear reservacion");
            }
        }

        private void reservacionTermina(object sender, EventArgs e)
        {
            string i = (sender as Button).Text;
            if (query("UPDATE habitacion SET estado = " + 1 + ", descripcion = 'Ocupado' WHERE id LIKE " + i + ";"))
            {
                var src = DateTime.Now;
                int dias = 0;
                double costo = 0;

                string connStr = "server=localhost;user=root;database=dbHotel;port=3306;password=";
                MySqlConnection conn = new MySqlConnection(connStr);
                try
                {
                    Console.WriteLine("Connecting to MySQL...");
                    conn.Open();

                    string sql = "select datediff((SELECT fechaEntrada FROM reservacion WHERE fkIdHabitacion LIKE " + i + " AND fechaSalida IS NULL), NOW()) as resultado1, (SELECT costo FROM habitacion WHERE Id LIKE " + i + ")as resultado2;";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    MySqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        dias = rdr.GetInt32(rdr.GetOrdinal("resultado1"));
                        costo = rdr.GetDouble(rdr.GetOrdinal("resultado2"));
                    }

                    if (src.Hour >= 12 && src.Hour <= 24 || dias == 0) dias++;

                    rdr.Close();
                    conn.Close();
                    Console.WriteLine("Done.");

                    if (query("UPDATE reservacion SET fechaSalida = NOW(), importe = " + (costo * dias) + " WHERE fkIdHabitacion LIKE " + i + " AND fechaSalida IS NULL;"))
                    {
                        VistaHabitaciones form = new VistaHabitaciones();
                        form.Show();
                        this.Hide();
                        MessageBox.Show("Sevicio a la habitacion " + this.Text + " terminado, importe: " + (costo * dias).ToString("G", CultureInfo.CreateSpecificCulture("eu-ES")));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    conn.Close();
                    MessageBox.Show("Error 2 al terminar reservacion");
                }
            } else
            {
                MessageBox.Show("Error 1 al terminar reservacion");
            }
        }

        private bool query(string sql)
        {
            string connStr = "server=localhost;user=root;database=dbHotel;port=3306;password=";
            MySqlConnection conn = new MySqlConnection(connStr);
            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                Console.WriteLine(sql);
                Console.WriteLine(cmd.ExecuteNonQuery());

                conn.Close();
                Console.WriteLine("Done.");

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                conn.Close();

                return false;
            }
        }

        private int conteo()
        {
            string connStr = "server=localhost;user=root;database=dbHotel;port=3306;password=";
            MySqlConnection conn = new MySqlConnection(connStr);

            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();

                int num = 0;

                string sql = "SELECT COUNT(*) AS count FROM habitacion;";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    num = rdr.GetInt32(rdr.GetOrdinal("count"));
                    if (num > 169)
                    {
                        Limite limite = new Limite();
                        limite.Show();
                    }
                }

                rdr.Close();
                conn.Close();

                return num;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                conn.Close();
                return 0;
            }
        }

        private void VistaHabitaciones_FormClosing(object sender, FormClosingEventArgs e)
        {
            AdministracionHotel form = new AdministracionHotel();
            form.Show();
            this.Hide();
        }

    }
}
