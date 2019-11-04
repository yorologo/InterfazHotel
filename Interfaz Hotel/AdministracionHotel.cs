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
    public partial class AdministracionHotel : Form
    {
        public AdministracionHotel()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            VistaHabitaciones form = new VistaHabitaciones();
            form.Show();
            this.Hide();
        }

        private void AdministracionHotel_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            NuevaHabitacion habitacion = new NuevaHabitacion();
            habitacion.Show();
            this.Hide();
        }

    }
}
