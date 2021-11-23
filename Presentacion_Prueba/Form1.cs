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
    public partial class PruebaConexion : Form
    {
        public PruebaConexion()
        {
            InitializeComponent();
        }

        private void btnConectar_Click(object sender, EventArgs e)
        {
            dGridViewClientes.Show();

            //Creacion de String y SqlConnection necesarios para realizar conexion
            string connetionString;
            SqlConnection cnn;

            //Dar el punto de entrada al servidor/db al String e instanciar SqlConnection con ese punto de entrada
            connetionString = @"Server=PC-SAMUEL\SMONTIVERO;Database=Autos para 5;Trusted_Connection=True;";
            cnn = new SqlConnection(connetionString);

            //Abrir conexion
            cnn.Open();


            //SqlCommand recibe un string con la consulta y una conexion donde ir a buscar los datos
            //Aca creamos el command y el dataReader que va a buscar la infom Output  va a ser un String que
            //acumule y muestre los datos
            SqlCommand command;
            SqlDataReader dataReader;
            String sql, Output = "";



            //sql es el string que contiene la consulta
            sql = "SELECT * FROM[Autos para 5].[dbo].[Clientes]";

            SqlDataAdapter adaptador = new SqlDataAdapter(sql, cnn);
            DataTable dataTable = new DataTable();
            adaptador.Fill(dataTable);

            dGridViewClientes.DataSource = dataTable;

            //Creamos la instancia nueva del SqlCommand para conectarse a la base de datos y mandar la consulta
            command = new SqlCommand(sql, cnn);

            //El dataReader recibe la info, al ejecutar .ExecuteReader devuelve todas las filas de la tabla
            dataReader = command.ExecuteReader();

            //Este while sirve para acumular linea por linea los datos de la columna definida en .GetValue(#) en un String
            while (dataReader.Read())
            {
               // int i = dGridViewClientes.Rows.Add();
                //Output = Output + dataReader.GetValue(0) + " - " + dataReader.GetValue(1) + " - " + dataReader.GetValue(2) + "\n";
               // dGridViewClientes.Rows(i).Cells(0).Value = dataReader.GetValue(0);
            }


            //Crea una ventana con la info almacenada en Output
            //MessageBox.Show(Output);

            //Proocolo de cierre de las acciones -> dataReader, command y cnn
            dataReader.Close();
            command.Dispose();
            cnn.Close();
        }

        private void PruebaConexion_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'diariosDataSet.Clientes' Puede moverla o quitarla según sea necesario.
            //this.clientesTableAdapter.Fill(this.diariosDataSet.Clientes);

        }

        private void txtBuscador_TextChanged(object sender, EventArgs e)
        {
            dGridViewClientes.Show();
            string connetionString, sql;
            SqlConnection cnn;

            connetionString = @"Server=PC-SAMUEL\SMONTIVERO;Database=DiariosPrueba;Trusted_Connection=True;";
            cnn = new SqlConnection(connetionString);
                        
           // SqlCommand command;

            sql = "SELECT [NroCliente], [ApellidoNombre],[Credito] FROM[DiariosPrueba].[dbo].[Clientes] WHERE [ApellidoNombre] like '%" +txtBuscador.Text +"%'";
            
            cnn.Open();

           // command = new SqlCommand(sql, cnn);
            SqlDataAdapter adaptador = new SqlDataAdapter(sql, cnn);
            DataTable dataTable = new DataTable();
            adaptador.Fill(dataTable);

            dGridViewClientes.DataSource = dataTable;

            if (txtBuscador.Text == "")
            {
                dGridViewClientes.Hide();
            }

           // command.Dispose();
            cnn.Close();

        }
    }
}
