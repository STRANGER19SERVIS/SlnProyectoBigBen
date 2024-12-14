using Microsoft.AspNetCore.Mvc;
namespace ProyectoBigBen.Models;
using System.Data.SqlClient;


public class PedidoController : Controller
{
    // Reemplaza con tu cadena de conexión
    private readonly string connectionString = "ConnectionStrings:cn"; 
    

    // GET: api/Pedidos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> Pedidos()
    {
        var pedidos = new List<object>();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            await connection.OpenAsync();
            string query = "SELECT * FROM Pedidos";
            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataReader reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    pedidos.Add(new
                    {
                        id_pedido = reader["id_pedido"].ToString(),
                        fecha_hora = Convert.ToDateTime(reader["fecha_hora"]),
                        total = Convert.ToDecimal(reader["total"]),
                        estado = reader["estado"].ToString(),
                        id_cliente = reader["id_cliente"].ToString()
                    });
                }
            }
        }

        return Ok(pedidos);
    }

    // GET: api/Pedidos/{id}
    [HttpGet("ListadoPedido")]
    public async Task<ActionResult<object>> ListadoPedido(string id)
    {
        object pedido = null;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            await connection.OpenAsync();
            string query = "SELECT * FROM Pedidos WHERE id_pedido = @id_pedido";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id_pedido", id);
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        pedido = new
                        {
                            id_pedido = reader["id_pedido"].ToString(),
                            fecha_hora = Convert.ToDateTime(reader["fecha_hora"]),
                            total = Convert.ToDecimal(reader["total"]),
                            estado = reader["estado"].ToString(),
                            id_cliente = reader["id_cliente"].ToString()
                        };
                    }
                }
            }
        }

        if (pedido == null)
        {
            return NotFound();
        }

        return Ok(pedido);
    }

    // POST: api/Pedidos
    [HttpPost]
    public async Task<ActionResult> CrearPedido([FromBody] dynamic pedido)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            await connection.OpenAsync();
            string query = @"INSERT INTO Pedidos (id_pedido, fecha_hora, total, estado, id_cliente) 
                                 VALUES (@id_pedido, @fecha_hora, @total, @estado, @id_cliente)";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id_pedido", (string)pedido.id_pedido);
                command.Parameters.AddWithValue("@fecha_hora", (DateTime)pedido.fecha_hora);
                command.Parameters.AddWithValue("@total", (decimal)pedido.total);
                command.Parameters.AddWithValue("@estado", (string)pedido.estado);
                command.Parameters.AddWithValue("@id_cliente", (string)pedido.id_cliente);

                await command.ExecuteNonQueryAsync();
            }
        }

        return CreatedAtAction(nameof(Pedidos), new { id = (string)pedido.id_pedido }, pedido);
    }

    // PUT: api/Pedidos/{id}
    [HttpPut("DetallePedido")]
    public async Task<ActionResult> DetallePedido(string id, [FromBody] dynamic pedido)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            await connection.OpenAsync();
            string query = @"UPDATE Pedidos 
                                 SET fecha_hora = @fecha_hora, total = @total, estado = @estado, id_cliente = @id_cliente 
                                 WHERE id_pedido = @id_pedido";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id_pedido", id);
                command.Parameters.AddWithValue("@fecha_hora", (DateTime)pedido.fecha_hora);
                command.Parameters.AddWithValue("@total", (decimal)pedido.total);
                command.Parameters.AddWithValue("@estado", (string)pedido.estado);
                command.Parameters.AddWithValue("@id_cliente", (string)pedido.id_cliente);

                int rowsAffected = await command.ExecuteNonQueryAsync();

                if (rowsAffected == 0)
                {
                    return NotFound();
                }
            }
        }

        return NoContent();
    }

    // DELETE: api/Pedidos/{id}
    [HttpDelete("EliminarPedido")]
    public async Task<ActionResult> EliminarPedido(string id)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            await connection.OpenAsync();
            string query = "DELETE FROM Pedidos WHERE id_pedido = @id_pedido";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id_pedido", id);

                int rowsAffected = await command.ExecuteNonQueryAsync();

                if (rowsAffected == 0)
                {
                    return NotFound();
                }
            }
        }

        return NoContent();
    }
}
