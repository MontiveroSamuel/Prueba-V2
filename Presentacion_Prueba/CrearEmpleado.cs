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
    public partial class CrearEmpleado : Form
    {
        public CrearEmpleado()
        {
            InitializeComponent();
        }

        private void txtBuscador_TextChanged(object sender, EventArgs e)
        {
            //Crear aux
            String sqlQuery, connectionString;
            SqlConnection connection;

            //Conexion a db
            connectionString = @"Server=PC-SAMUEL\SMONTIVERO;Database=Autos para 5;Trusted_Connection=True;";
            connection = new SqlConnection(connectionString); 
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

            
            //Cerrar conexion

            connection.Close();
        }

        private void CrearEmpleado_Load(object sender, EventArgs e)
        {
            lblInforme.Text = "";
        }

        private void btnCrear_Click(object sender, EventArgs e)
        {

            SqlCommand command;
            String sqlQueryUbicacion, sqlQueryBarrio, sqlQueryCiudad, sqlQueryProvincia, sqlQueryTelefono, sqlQueryDirecta, connectionString;
            SqlConnection connection;
            SqlDataAdapter adaptador = new SqlDataAdapter();


            connectionString = @"Server=PC-SAMUEL\SMONTIVERO;Database=Autos para 5;Trusted_Connection=True;";
            connection = new SqlConnection(connectionString);

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

            sqlQueryProvincia = "INSERT INTO [Provincia] ([Descripcion]) VALUES ('" + txtProvincia.Text + "')";
            command = new SqlCommand(sqlQueryProvincia, connection);

            adaptador.UpdateCommand = new SqlCommand(sqlQueryProvincia, connection);
            adaptador.UpdateCommand.ExecuteNonQuery();


            sqlQueryCiudad = "INSERT INTO [Ciudad] ([Descripcion], [Id_Provincia]) values ('" + txtCiudad.Text + "', 3);";
            command = new SqlCommand(sqlQueryCiudad, connection);

            adaptador.UpdateCommand = new SqlCommand(sqlQueryCiudad, connection);
            adaptador.UpdateCommand.ExecuteNonQuery();

            sqlQueryBarrio = "INSERT INTO [Barrio] ([Descripcion],[Id_Ciudad]) values  ('" + txtBarrio.Text + "', 3)";
            command = new SqlCommand(sqlQueryBarrio, connection);

            adaptador.UpdateCommand = new SqlCommand(sqlQueryBarrio, connection);
            adaptador.UpdateCommand.ExecuteNonQuery();

            sqlQueryUbicacion = "INSERT INTO [Ubicacion_Empleados] ([Id_Barrio], [Id_Estado], [Direccion], [Fecha_De_Inicio]) values (3, 1, '"+ txtDireccion.Text +"', '2021-11-20')";
            command = new SqlCommand(sqlQueryUbicacion, connection);

            adaptador.UpdateCommand = new SqlCommand(sqlQueryUbicacion, connection);
            adaptador.UpdateCommand.ExecuteNonQuery();

            sqlQueryTelefono = "INSERT INTO [Telefonos_Empleados] ([Id_Estado], [Telefono]) Values (1, '"+ txtTelefono.Text +"')";
            command = new SqlCommand(sqlQueryTelefono, connection);

            adaptador.UpdateCommand = new SqlCommand(sqlQueryTelefono, connection);
            adaptador.UpdateCommand.ExecuteNonQuery();

            sqlQueryDirecta = "INSERT INTO [Empleado] ([Id_UbicacionEmpleados], [Id_TelefEmpleados], [Id_Estado], [Nombre], [DNI], [Fecha_De_Inicio], [Observaciones]) values (8, 4, 1, '" + txtNombre.Text + "', " +
              ""+ txtDNI.Text + ",  '2021-11-20', '" + txtObservaciones.Text + "')";
            command = new SqlCommand(sqlQueryDirecta, connection);

            adaptador.UpdateCommand = new SqlCommand(sqlQueryDirecta, connection);
            adaptador.UpdateCommand.ExecuteNonQuery();





            command.Dispose();
            connection.Close();


        }
    }
}
