using Pedidos.model;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Windows;

namespace Pedidos.connection
{
    public class Connection
    {
        // Creamos una variable de referencia a la cadena de conexión almacenada en la configuración del proyecto.

        //private string cadenaConexion = Server=tcp:dam2023-di.database.windows.net,1433;Initial Catalog=prueba1;Persist Security Info=False;User ID=usuario;Password={your_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30";

        private string connectionStr = "Server=tcp:bdd1-di.database.windows.net,1433;Initial Catalog=servidor1di;Persist Security Info=False;User ID=izzywav;Password=AzureBDD112@;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30";
        // Variables para recuperar información de la Base de datos
        //private OleDbConnection CN;
        //private OleDbCommand CMD;
        //private OleDbDataReader RDR;

        private SqlConnection CN;
        private SqlCommand CMD;
        private SqlDataReader RDR;



        public ObservableCollection<Pedido> GetPedidos()
        {
            // Instanciamos la variable CN pasandole a su constructor la variable "cadenaConexion".
            CN = new SqlConnection(connectionStr);
            // Instanciamos la variable CMD pasandole a su constructor la instrucción OleDb que debe ejecutar
            // así como  la variable CN que le indica en que base de datos debe ejecutar dicha instrucción.
            CMD = new SqlCommand("SELECT * FROM TPedidos", CN);
            // Tipo de comando.
            CMD.CommandType = CommandType.Text;

            // Creamos una colección de tipo Coche que "envuelve"
            // a los registros de la tabla que se van a recuperar.
            ObservableCollection<Pedido> PedidoList =
                new ObservableCollection<Pedido>();

            try // Intentamos...
            {
                CN.Open(); // Abrir la conexión.
                RDR = CMD.ExecuteReader(); // Ejecutar la instrucción OleDb SELECT.

                while (RDR.Read()) // recorrer todos los registros.
                {

                    // Crear un objecto que "envuelve" el registro actual.
                    Pedido currentPedido = new Pedido();
                    currentPedido.NPedido = (string)RDR["NPedido"];
                    currentPedido.Cliente = (string)RDR["Cliente"];
                    currentPedido.DNI = (string)RDR["DNI"];
                    Debug.WriteLine(RDR["Cantidad"]);
                    currentPedido.Cantidad = (int)RDR["Cantidad"];
                    currentPedido.Fecha = (string)RDR["Fecha"];

                    // Agregar el objeto a la coleccion.
                    PedidoList.Add(currentPedido);
                }
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.ToString()); // Lanzamos excepción.
            }
            finally
            {
                CN.Close(); // Cerramos la conexión.
            }

            return PedidoList; // Regresamos la lista.
        }

        // Método que inserta un nuevo libro en la tabla.
        public void SaveNewPedido(Pedido newPedido)
        {
            CN = new SqlConnection(connectionStr);
            CMD = new SqlCommand();
            CMD.Connection = CN;
            CMD.CommandType = CommandType.Text;

            Debug.WriteLine(newPedido.NPedido);
            CMD.CommandText = "INSERT INTO TPedidos VALUES (@p1, @p2, @p3, @p4, @p5)";


            //// establecemos los valores que tomarán los parámetros de la instrucción OleDb.
            CMD.Parameters.AddWithValue("@p1", newPedido.NPedido);
            CMD.Parameters.AddWithValue("@p2", newPedido.Cliente);
            CMD.Parameters.AddWithValue("@p3", newPedido.DNI);
            CMD.Parameters.AddWithValue("@p4", newPedido.Cantidad);
            CMD.Parameters.AddWithValue("@p5", newPedido.Fecha);

            // Insertamos registro sin utilizar parámetros
            //CMD.CommandText = "INSERT INTO Libros VALUES ('" + nuevoLibro.Titulo + "', '" + nuevoLibro.Isbn + "', '" + nuevoLibro.Autor + "', " +
            //        "'" + nuevoLibro.Editorial + "')";


            CN.Open();
            CMD.ExecuteNonQuery();

            CN.Close();
        }

        public int DeletePedido(string NPedido)
        {
            CN = new SqlConnection(connectionStr);
            CMD = new SqlCommand("DELETE FROM TPedidos WHERE NPedido = @p0", CN);
            CMD.CommandType = CommandType.Text;
            CMD.Parameters.AddWithValue("@p0", NPedido);

            // Eliminamos registro sin parámetros
            //CMD = new OleDbCommand("DELETE FROM Libros WHERE ISBN = '" + isbn + "'", CN);


            try
            {
                CN.Open();
                return CMD.ExecuteNonQuery();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CN.Close();
            }
        }

        public int UpdateExistingPedido(string NPedido, string Cliente, string DNI, int Cantidad, string Fecha)
        {
            CN = new SqlConnection(connectionStr);
            CMD = new SqlCommand("UPDATE TPedidos" +
                               " SET Cliente = @p1, DNI = @p2, Cantidad = @p3, Fecha = @p4" +
                                              " WHERE NPedido = @p0", CN);

            // set values for every parameter
            CMD.Parameters.AddWithValue("@p0", NPedido);
            CMD.Parameters.AddWithValue("@p1", Cliente);
            CMD.Parameters.AddWithValue("@p2", DNI);
            CMD.Parameters.AddWithValue("@p3", Cantidad);
            CMD.Parameters.AddWithValue("@p4", Fecha);

            CMD.CommandType = CommandType.Text;
            try
            {
                CN.Open();
                return CMD.ExecuteNonQuery();// Devuelve el número de filas afectadas en este caso debe ser 1.
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                CN.Close();
            }
        }
    }
}
