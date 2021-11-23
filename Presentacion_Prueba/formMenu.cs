using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentacion_Prueba
{
    public partial class formMenu : Form
    {
        public formMenu()
        {
            InitializeComponent();
        }

        private void consultarModificarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form formularioModificacion = new UpdateEmpleado();
            formularioModificacion.Show();
        }

        private void formMenu_Load(object sender, EventArgs e)
        {
            
        }

        private void archivoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void nuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form formularioCrear = new CrearEmpleado();
            formularioCrear.Show();
        }
    }
}
