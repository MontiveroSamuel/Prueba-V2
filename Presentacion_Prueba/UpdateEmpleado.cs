using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentacion_Prueba
{
    public partial class UpdateEmpleado : Form
    {
        public UpdateEmpleado()
        {
            InitializeComponent();
        }



        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //Al generar cambios en texto mostrar el cuadro de busqueda y lo lleva al medio
            //dgvBuscaEmpleado.Show();

            //Crear aux
            String sqlQuery, connectionString;
            SqlConnection connection;

            //Conexion a db
            connectionString = @"Server=.;Data Source=MAXI\MAX;Initial Catalog=Autos para 5;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            connection = new SqlConnection(connectionString);

            //Busqueda de coincidencia

            sqlQuery = "SELECT [Id_Empleado], [Nombre], [Direccion], [Barrio].[Descripcion] AS Barrio, [Ciudad].[Descripcion] AS Ciudad, [Provincia].[Descripcion] AS Provincia, Telefonos_Empleados.Telefono AS Telefono, [DNI], [Empleado].[Fecha_de_Inicio], [Empleado].[Observaciones]" +
                    "FROM[Autos para 5].[dbo].[Empleado] JOIN [Autos para 5].[dbo].[Ubicacion_Empleados] ON [Empleado].[Id_UbicacionEmpleados] = [Ubicacion_Empleados].[Id_UbicacionEmpleados] " +
                    "JOIN[Autos para 5].[dbo].[Barrio] ON [Ubicacion_Empleados].[Id_Barrio] = [Barrio].[Id_Barrio] JOIN [Autos para 5].[dbo].[Ciudad] ON [Barrio].[Id_Ciudad] = [Ciudad].[Id_Ciudad] " +
                    "JOIN[Autos para 5].[dbo].[Provincia] ON [Ciudad].[Id_Ciudad] = [Provincia].[Id_Provincia] JOIN [Autos para 5].[dbo].[Telefonos_Empleados] ON [Empleado].[Id_TelefEmpleados] = [Telefonos_Empleados].[Id_TelefEmpleados] WHERE [Nombre] LIKE '%" + txtBuscador.Text + "%' AND [Empleado].[Id_Estado] = 1";


            //Abrir conexion
            connection.Open();
            //Adaptar datos y crear dataTable que va a llenar la DataViewGrid
            SqlDataAdapter adaptador = new SqlDataAdapter(sqlQuery, connection);
            DataTable dt = new DataTable();
            adaptador.Fill(dt);

            //Mandar datos a DVG
            dgvBuscaEmpleado.DataSource = dt;

            //Ocultar tabla si buscador queda en blanco
            //if (txtBuscador.Text == "")
            //{
            //    dgvBuscaEmpleado.Hide();
            //}

            //Cerrar conexion

            connection.Close();
        }

        private void CrearEmpleado_Load(object sender, EventArgs e)
        {
            lblInforme.Text = "";

        }

        private void dgvBuscaEmpleado_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            txtDireccion.Text = dgvBuscaEmpleado.SelectedCells[2].Value.ToString();
            txtBarrio.Text = dgvBuscaEmpleado.SelectedCells[3].Value.ToString();
            txtCiudad.Text = dgvBuscaEmpleado.SelectedCells[4].Value.ToString();
            txtProvincia.Text = dgvBuscaEmpleado.SelectedCells[5].Value.ToString();
            txtTelefono.Text = dgvBuscaEmpleado.SelectedCells[6].Value.ToString();
            txtDNI.Text = dgvBuscaEmpleado.SelectedCells[7].Value.ToString();
            txtFechaInicio.Text = dgvBuscaEmpleado.SelectedCells[8].Value.ToString();
            txtObservaciones.Text = dgvBuscaEmpleado.SelectedCells[9].Value.ToString();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            SqlCommand command;
            String sqlQueryDireccion, sqlQueryBarrio, sqlQueryCiudad, sqlQueryProvincia, sqlQueryTelefono, sqlQueryDirecta, connectionString;
            SqlConnection connection;
            SqlDataAdapter adaptador = new SqlDataAdapter();


            connectionString = @"Server=.;Data Source=MAXI\MAX;Initial Catalog=Autos para 5;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            connection = new SqlConnection(connectionString);

            //Validacion de datos- Telefono-DNI
            if (string.IsNullOrEmpty(txtDNI.Text))
            {
                lblInforme.Text = "No se pude dejar el campo DNI en blanco";
                return;
            }

            int datoInt;
            if (int.TryParse(txtDNI.Text, out datoInt))
            {
                if (datoInt < 0)
                { datoInt = Math.Abs(datoInt); }
                else if (datoInt >= 0)
                { txtDNI.Text = datoInt.ToString(); }

                lblInforme.BackColor = Color.Green;
                lblInforme.Text = "Se han guardados los datos!";

            }
            else
            {
                lblInforme.Text = "El DNI no puede contener letras";
                return;

            }


            if (int.TryParse(txtTelefono.Text, out datoInt))
            {
                if (datoInt < 0)
                { datoInt = Math.Abs(datoInt); }
                else if (datoInt >= 0)
                { txtTelefono.Text = datoInt.ToString(); }

                lblInforme.BackColor = Color.Green;   
                lblInforme.Text = "Se han guardados los datos!";

            }
            else
            {
                lblInforme.Text = "El teléfono no puede contener letras";
                return;

            }


            if (string.IsNullOrEmpty(txtTelefono.Text))
            {
                lblInforme.Text = "No se pude dejar el campo teléfono en blanco";
            }



            connection.Open();


            //Actualizacion de direccion en db


            sqlQueryDireccion = "UPDATE [Ubicacion_Empleados] SET [Direccion] = '" + txtDireccion.Text + "' FROM [Ubicacion_Empleados] JOIN [Empleado] " +
                            "ON [Empleado].[Id_UbicacionEmpleados] = [Ubicacion_Empleados].[Id_UbicacionEmpleados] WHERE [Id_Empleado] = '" + dgvBuscaEmpleado.SelectedCells[0].Value.ToString() + "' ";
            command = new SqlCommand(sqlQueryDireccion, connection);

            adaptador.UpdateCommand = new SqlCommand(sqlQueryDireccion, connection);
            adaptador.UpdateCommand.ExecuteNonQuery();

            //Actualizacion de Barrio en db
            sqlQueryBarrio = "UPDATE [Barrio] SET [Descripcion] = '" + txtBarrio.Text + "' FROM [Barrio] JOIN [Ubicacion_Empleados] ON [Ubicacion_Empleados].[Id_Barrio] = [Barrio].[Id_Barrio] " +
                " WHERE [Barrio].[Descripcion] = '" + dgvBuscaEmpleado.SelectedCells[3].Value.ToString() + "' ";
            command = new SqlCommand(sqlQueryBarrio, connection);

            adaptador.UpdateCommand = new SqlCommand(sqlQueryBarrio, connection);
            adaptador.UpdateCommand.ExecuteNonQuery();




            //Actualizacion Ciudad en db
            sqlQueryCiudad = "UPDATE [Ciudad] SET [Descripcion] = '" + txtCiudad.Text + "' FROM [Ciudad] JOIN [Barrio] ON [Ciudad].[Id_Ciudad] = [Barrio].[Id_Ciudad] " +
                " WHERE [Ciudad].[Descripcion] = '" + dgvBuscaEmpleado.SelectedCells[4].Value.ToString() + "' ";
            command = new SqlCommand(sqlQueryCiudad, connection);

            adaptador.UpdateCommand = new SqlCommand(sqlQueryCiudad, connection);
            adaptador.UpdateCommand.ExecuteNonQuery();


            //Actualizacion Provincia en db

            sqlQueryProvincia = "UPDATE [Provincia] SET [Descripcion] = '" + txtProvincia.Text + "' FROM [Provincia] JOIN [Ciudad] ON [Ciudad].[Id_Provincia] = [Provincia].[Id_Provincia] " +
                " WHERE [Provincia].[Descripcion] = '" + dgvBuscaEmpleado.SelectedCells[5].Value.ToString() + "' ";
            command = new SqlCommand(sqlQueryProvincia, connection);

            adaptador.UpdateCommand = new SqlCommand(sqlQueryProvincia, connection);
            adaptador.UpdateCommand.ExecuteNonQuery();


            //Actualizacion telefono

            sqlQueryTelefono = "UPDATE [Telefonos_Empleados] SET [Telefono] = '" + txtTelefono.Text + "' FROM[Telefonos_Empleados] JOIN [Empleado] ON [Telefonos_Empleados].[Id_TelefEmpleados] = [Empleado].[Id_TelefEmpleados] WHERE [Id_Empleado] = '" + dgvBuscaEmpleado.SelectedCells[0].Value.ToString() + "' ";
            command = new SqlCommand(sqlQueryTelefono, connection);

            adaptador.UpdateCommand = new SqlCommand(sqlQueryTelefono, connection);
            adaptador.UpdateCommand.ExecuteNonQuery();

            //Actualizacion DNI en db

            sqlQueryDirecta = "UPDATE [Empleado] SET [DNI] = '" + txtDNI.Text + "', [Observaciones] = '" + txtObservaciones.Text + "' FROM [Empleado] " +
                " WHERE [Id_Empleado] = '" + dgvBuscaEmpleado.SelectedCells[0].Value.ToString() + "' ";
            command = new SqlCommand(sqlQueryDirecta, connection);

            adaptador.UpdateCommand = new SqlCommand(sqlQueryDirecta, connection);
            adaptador.UpdateCommand.ExecuteNonQuery();



            command.Dispose();
            connection.Close();



        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            SqlCommand command;
            String sqlQuery, connectionString;
            SqlConnection connection;
            SqlDataAdapter adaptador = new SqlDataAdapter();

            lblInforme.BackColor = Color.Green;
            lblInforme.Text = "Se han eliminado los campos con éxito";

            connectionString = @"Server=.;Data Source=MAXI\MAX;Initial Catalog=Autos para 5;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            connection = new SqlConnection(connectionString);
            connection.Open();

            sqlQuery = "UPDATE [Empleado] SET [Id_Estado] = 2 WHERE [Nombre] = '" + dgvBuscaEmpleado.SelectedCells[1].Value.ToString() + "' ";

            command = new SqlCommand(sqlQuery, connection);

            adaptador.UpdateCommand = new SqlCommand(sqlQuery, connection);
            adaptador.UpdateCommand.ExecuteNonQuery();

            command.Dispose();
            connection.Close();


        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            //Limpiar txt
            lblInforme.Text = "";
            txtBarrio.Clear();
            txtDireccion.Clear();
            txtCiudad.Clear();
            txtDNI.Clear();
            txtFechaInicio.Clear();
            txtObservaciones.Clear();
            txtProvincia.Clear();
            txtTelefono.Clear();
            txtBuscador.Clear();
        }

    }
}

