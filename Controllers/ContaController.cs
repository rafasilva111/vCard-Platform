using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using vCardPlatform.Models;

namespace vCardPlatform.Controllers
{
    public class ContaController : ApiController
    {
        //string connectionString = "Data Source=(LocalDB)\\MyInstance;AttachDbFilename=C:\\Users\\rafae\\source\\repos\\vCardPlatform\\App_Data\\Database.mdf;Integrated Security=True";
        string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\lucas\\Documents\\GitHub\\js\\vCard-Platform\\App_Data\\Database.mdf;Integrated Security = True";

        // GET: Conta/GetContaById/1
        public IHttpActionResult GetContaById(int id)
        {
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                string cmdSQL = "SELECT * FROM Contas WHERE Id=@idPedidosTable";
                SqlCommand command = new SqlCommand(cmdSQL, connection);
                command.Parameters.AddWithValue("@idPedidosTable", id);
                SqlDataReader reader = command.ExecuteReader();

                Conta pedidoAInserir = null;

                while (reader.Read())
                {
                    pedidoAInserir = new Conta();
                    pedidoAInserir.Id = (int)reader["Id"];
                    pedidoAInserir.Balance = (float)reader["Balance"];
                    pedidoAInserir.AccountOwner = (string)reader["AccountOwner"];
                    string value = (string)reader["CreatedAt"];
                    if (value == null)
                    {
                        pedidoAInserir.CreatedAt = DateTime.Now.Ticks+"";
                    }
                    pedidoAInserir.CreatedAt = value;
                }

                reader.Close();
                connection.Close();
                if (pedidoAInserir != null)
                {
                    return Ok(pedidoAInserir);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }

                return Ok(e.Message+e.StackTrace);
            }
        }

        
        public IHttpActionResult GetAllContas()
        {
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                string cmdSQL = "SELECT * FROM Contas";
                SqlCommand command = new SqlCommand(cmdSQL, connection);
                
                SqlDataReader reader = command.ExecuteReader();
                LinkedList<Conta> contas = new LinkedList<Conta>(); 

                Conta pedidoAInserir = null;

                while (reader.Read())
                {
                    pedidoAInserir = new Conta();
                    pedidoAInserir.Id = (int)reader["Id"];
                    pedidoAInserir.Balance = (float)reader["Balance"];
                    //pedidoAInserir.AccountOwner = (string)reader["Name"];
                    pedidoAInserir.CreatedAt = (string)reader["CreatedAt"];
                    //pedidoAInserir.Email = (string)reader["Email"];
                    pedidoAInserir.ConfirmationCode = (int)reader["ConfirmationCode"];
                    

                    contas.AddLast(pedidoAInserir);
                }

                reader.Close();
                connection.Close();
                if (pedidoAInserir != null)
                {
                    return Ok(contas);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }

                return Ok(e.Message + "__________________" +e.StackTrace);
            }
        }

        // POST: api/Products
        public IHttpActionResult PostConta([FromBody] Conta value)
        {
            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                string cmdSQL = "INSERT INTO pedidosTable VALUES (@AccountOwner,@Balance,@CreatedAt,@PhoneNumber,@Email,@ConfirmationCode)";
                SqlCommand command = new SqlCommand(cmdSQL, connection);
                command.Parameters.AddWithValue("@AccountOwner", value.AccountOwner);
                command.Parameters.AddWithValue("@Balance", value.Balance);
                command.Parameters.AddWithValue("@CreatedAt", value.CreatedAt);
                command.Parameters.AddWithValue("@PhoneNumber", value.PhoneNumber);
                command.Parameters.AddWithValue("@Email", value.Email);
                command.Parameters.AddWithValue("@ConfirmationCode", value.ConfirmationCode);

                int numRows = command.ExecuteNonQuery();

                connection.Close();

                if (numRows > 0)
                {
                    return Ok();
                }

                return NotFound();

            }
            catch (Exception)
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }

                return NotFound();
            }

        }

    }
}
