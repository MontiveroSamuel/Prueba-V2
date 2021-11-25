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
            //String de maxi
            //connectionString = @"Server=.;Data Source=MAXI\MAX;Initial Catalog=Autos para 5;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            //String Sam
            connectionString = @"Server = PC-SAMUEL\SMONTIVERO; Database = Autos para 5; Trusted_Connection = True; ";
            connection = new SqlConnection(connectionString);

            //Busqueda de coincidencia

            sqlQuery = "SELECT [Id_Empleado], [Nombre], [Direccion], [Barrio].[Descripcion] AS Barrio, [Ciudad].[Descripcion] AS Ciudad, [Provincia].[Descripcion] AS Provincia, Telefonos_Empleados.Telefono AS Telefono, [DNI], [Empleado].[Fecha_de_Inicio], [Empleado].[Observaciones]" +
                    "FROM[Autos para 5].[dbo].[Empleado] JOIN [Autos para 5].[dbo].[Ubicacion_Empleados] ON [Empleado].[Id_UbicacionEmpleados] = [Ubicacion_Empleados].[Id_UbicacionEmpleados] " +
                    "JOIN[Autos para 5].[dbo].[Barrio] ON [Ubicacion_Empleados].[Id_Barrio] = [Barrio].[Id_Barrio] JOIN [Autos para 5].[dbo].[Ciudad] ON [Barrio].[Id_Ciudad] = [Ciudad].[Id_Ciudad] " +
                    "JOIN[Autos para 5].[dbo].[Provincia] ON [Ciudad].[Id_Ciudad] = [Provincia].[Id_Provincia] JOIN [Autos para 5].[dbo].[Telefonos_Empleados] ON [Empleado].[Id_TelefEmpleados] = [Telefonos_Empleados].[Id_TelefEmpleados] WHERE [Nombre] LIKE '%" + txtBuscador.Text + "%' AND [Empleado].[Id_Estado] = 1"; ;

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

            
            //Agregar datos al combobox 'provincias'
            String connectionString; // se crea un string de conexion a la db
            SqlConnection connection;//var de conexion sql

            //se escribe el stringconnection al directorio de la db
            //String de maxi
            //connectionString = @"Server=.;Data Source=MAXI\MAX;Initial Catalog=Autos para 5;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            //String Sam
            connectionString = @"Server = PC-SAMUEL\SMONTIVERO; Database = Autos para 5; Trusted_Connection = True; ";
            connection = new SqlConnection(connectionString);

            //creamos el comando para traer las provincias con su descripcion
            SqlCommand comando = new SqlCommand("SELECT Descripcion FROM Provincia", connection) ;
            
            //abrimos conexion con la base de datos
            connection.Open();

            //creamos un registro que lea el comando sql creado arriba

            SqlDataReader registro = comando.ExecuteReader();

            //'mientras haya datos en el registro', los combierte a string y los agrega al Combobox
            while (registro.Read())
            {
                cbProvincias.Items.Add(registro["Descripcion"].ToString());
            }
            connection.Close();
        }

        private void btnCrear_Click(object sender, EventArgs e)
        {
            DateTime fecha_de_hoy = DateTime.Now;
            SqlCommand command;
            String sqlQueryUbicacion, sqlQueryBarrio, sqlQueryCiudad, sqlQueryProvincia, sqlQueryTelefono, sqlQueryDirecta, connectionString;
            SqlConnection connection;
            SqlDataAdapter adaptador = new SqlDataAdapter();

            //String maxi
            //connectionString = @"Server=.;Data Source=MAXI\MAX;Initial Catalog=Autos para 5;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            //String Sam
            connectionString = @"Server = PC-SAMUEL\SMONTIVERO; Database = Autos para 5; Trusted_Connection = True; MultipleActiveResultSets=True";

            connection = new SqlConnection(connectionString);

            if (string.IsNullOrEmpty(txtDNI.Text))
            {
                lblInforme.BackColor = Color.Red;
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
                lblInforme.BackColor = Color.Red;
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
                lblInforme.BackColor = Color.Red;
                lblInforme.Text = "El teléfono no puede contener letras";
                return;

            }


            if (string.IsNullOrEmpty(txtTelefono.Text))
            {
                lblInforme.BackColor = Color.Red;
                lblInforme.Text = "No se pude dejar el campo teléfono en blanco";
                return;
            }



            connection.Open();


            //sqlQueryProvincia = "INSERT INTO [Provincia] ([Descripcion]) VALUES ('" + cbProvincias.SelectedItem.ToString() + "')";
            //command = new SqlCommand(sqlQueryProvincia, connection);

            //adaptador.UpdateCommand = new SqlCommand(sqlQueryProvincia, connection);
            //adaptador.UpdateCommand.ExecuteNonQuery();



            object value = cbProvincias.SelectedItem;
            //MessageBox.Show(value.ToString());

            //Traer id_Provincia seleccionado para ponerlo en query de creacion de barrio
            int idProvinciaSelec = 0;
            SqlCommand comandoProvincia = new SqlCommand("SELECT Id_Provincia FROM Provincia WHERE [Descripcion] = '" + cbProvincias.SelectedItem + "'", connection);
            SqlDataReader registroProvincia = comandoProvincia.ExecuteReader();

            //'mientras haya datos en el registro', los combierte a string y los agrega al Combobox
            while (registroProvincia.Read())
            {
                idProvinciaSelec = Convert.ToInt32(registroProvincia.GetValue(0));
            }



            sqlQueryCiudad = "INSERT INTO [Ciudad] ([Descripcion], [Id_Provincia]) values ('" + txtCiudad.Text + "', (SELECT Id_Provincia from Provincia WHERE Id_Provincia ='" + idProvinciaSelec + "') )";
            command = new SqlCommand(sqlQueryCiudad, connection);

            adaptador.UpdateCommand = new SqlCommand(sqlQueryCiudad, connection);
            adaptador.UpdateCommand.ExecuteNonQuery();


            //Traer id_ciudad seleccionado para ponerlo en query de creacion de barrio
            int idCiudadSelec = 0;
            SqlCommand comandoCiudad = new SqlCommand("SELECT Id_Ciudad FROM Ciudad WHERE [Descripcion] = '" + txtCiudad.Text + "'", connection);
            SqlDataReader registroCiudad = comandoCiudad.ExecuteReader();

            //'mientras haya datos en el registro', los combierte a string y los agrega al Combobox
            while (registroCiudad.Read())
            {
                idCiudadSelec = Convert.ToInt32(registroCiudad.GetValue(0));
            }

            sqlQueryBarrio = "INSERT INTO [Barrio] ([Descripcion],[Id_Ciudad]) values  ('" + txtBarrio.Text + "', (SELECT Id_Ciudad from Ciudad WHERE Id_Ciudad =" + idCiudadSelec + "))";
            command = new SqlCommand(sqlQueryBarrio, connection);

            adaptador.UpdateCommand = new SqlCommand(sqlQueryBarrio, connection);
            adaptador.UpdateCommand.ExecuteNonQuery();

            //Traer id_barrio seleccionado para ponerlo en query de creacion de barrio
            int idBarrioSelec = 0;
            SqlCommand comandoBarrio = new SqlCommand("SELECT Id_Barrio FROM Barrio WHERE [Descripcion] = '" + txtBarrio.Text + "'", connection);
            SqlDataReader registroBarrio = comandoBarrio.ExecuteReader();

            //'mientras haya datos en el registro', los combierte a string y los agrega al Combobox
            while (registroBarrio.Read())
            {
                idBarrioSelec = Convert.ToInt32(registroBarrio.GetValue(0));
            }

            sqlQueryUbicacion = "INSERT INTO [Ubicacion_Empleados] ([Id_Barrio], [Id_Estado], [Direccion], [Fecha_De_Inicio]) values ((SELECT Id_Barrio from Barrio WHERE Id_Barrio ='" + idBarrioSelec + "'), 1, '" + txtDireccion.Text + "', '"+ fecha_de_hoy +"')";
            command = new SqlCommand(sqlQueryUbicacion, connection);

            adaptador.UpdateCommand = new SqlCommand(sqlQueryUbicacion, connection);
            adaptador.UpdateCommand.ExecuteNonQuery();

            sqlQueryTelefono = "INSERT INTO [Telefonos_Empleados] ([Id_Estado], [Telefono]) Values (1, '"+ txtTelefono.Text +"')";
            command = new SqlCommand(sqlQueryTelefono, connection);

            adaptador.UpdateCommand = new SqlCommand(sqlQueryTelefono, connection);
            adaptador.UpdateCommand.ExecuteNonQuery();

            //Busqueda de ultimos id_UbicacionEmpleados para la creacion
            int idUbicEmpleadoNuevo = 0;
            SqlCommand comandoEmpleado = new SqlCommand("SELECT Id_UbicacionEmpleados FROM Ubicacion_Empleados WHERE [Direccion] = '" + txtDireccion.Text + "'", connection);
            SqlDataReader registroEmpleadoUb = comandoEmpleado.ExecuteReader();

            while (registroEmpleadoUb.Read())
            {
                idUbicEmpleadoNuevo = Convert.ToInt32(registroEmpleadoUb.GetValue(0));
            }
            //Busqueda de ultimos id_TelefEmpleados para la creacion
            int idTelefEmpleadoNuevo = 0;
            SqlCommand comandoTelef = new SqlCommand("SELECT Id_TelefEmpleados FROM Telefonos_Empleados WHERE [Telefono] = '" + txtTelefono.Text + "'", connection);
            SqlDataReader registroTelef = comandoTelef.ExecuteReader();

            while (registroTelef.Read())
            {
                idTelefEmpleadoNuevo = Convert.ToInt32(registroTelef.GetValue(0));
            }


            sqlQueryDirecta = "INSERT INTO [Empleado] ([Id_UbicacionEmpleados], [Id_TelefEmpleados], [Id_Estado], [Nombre], [DNI], [Fecha_De_Inicio], [Observaciones]) values ("+ idUbicEmpleadoNuevo + ", "+ idTelefEmpleadoNuevo + ", 1, '" + txtNombre.Text + "', " +
              ""+ txtDNI.Text + ",  '" + fecha_de_hoy + "', '" + txtObservaciones.Text + "')";
            command = new SqlCommand(sqlQueryDirecta, connection);

            adaptador.UpdateCommand = new SqlCommand(sqlQueryDirecta, connection);
            adaptador.UpdateCommand.ExecuteNonQuery();

           



            command.Dispose();
            connection.Close();


        }

        private void cbProvincias_SelectedIndexChanged(object sender, EventArgs e)
        {
            


        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            lblInforme.Text = "";
            txtBarrio.Clear();
            txtDireccion.Clear();
            txtCiudad.Clear();
            txtDNI.Clear();
            txtObservaciones.Clear();
            txtTelefono.Clear();
            txtBuscador.Clear();
        }
    }
}
