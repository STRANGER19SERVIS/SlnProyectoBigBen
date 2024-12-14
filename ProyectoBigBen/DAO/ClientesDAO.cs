using ProyectoBigBen.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ProyectoBigBen.DAO
{
    public class ClienteDAO
    {
        private readonly string cad_cn;

        public ClienteDAO(IConfiguration cfg)
        {
            cad_cn = cfg.GetConnectionString("cn1");
        }

        public List<Clientes> GetClientes()
        {
            var lista = new List<Clientes>();
            using (var cn = new SqlConnection(cad_cn))
            {
                using (var cmd = new SqlCommand("sp_ObtenerTodosLosClientes", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cn.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Clientes()
                            {
                                id_cliente = dr.GetString(dr.GetOrdinal("id_cliente")),
                                nombre = dr.GetString(dr.GetOrdinal("nombre")),
                                apellido = dr.GetString(dr.GetOrdinal("apellido")),
                                telefono = dr.GetString(dr.GetOrdinal("telefono")),
                                direccion = dr.GetString(dr.GetOrdinal("direccion"))
                            });
                        }
                    }
                }
            }
            return lista;
        }

        public Clientes BuscarCliente(string id_cliente)
        {
            Clientes cliente = null;
            using (var cn = new SqlConnection(cad_cn))
            {
                using (var cmd = new SqlCommand("sp_ObtenerClientePorID", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_cliente", id_cliente);

                    cn.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            cliente = new Clientes()
                            {
                                id_cliente = dr.GetString(dr.GetOrdinal("id_cliente")),
                                nombre = dr.GetString(dr.GetOrdinal("nombre")),
                                apellido = dr.GetString(dr.GetOrdinal("apellido")),
                                telefono = dr.GetString(dr.GetOrdinal("telefono")),
                                direccion = dr.GetString(dr.GetOrdinal("direccion"))
                            };
                        }
                    }
                }
            }
            return cliente;
        }

        public void InsertarCliente(Clientes cliente)
        {
            using (var cn = new SqlConnection(cad_cn))
            {
                using (var cmd = new SqlCommand("sp_InsertarCliente", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_cliente", cliente.id_cliente);
                    cmd.Parameters.AddWithValue("@nombre", cliente.nombre);
                    cmd.Parameters.AddWithValue("@apellido", cliente.apellido);
                    cmd.Parameters.AddWithValue("@telefono", cliente.telefono);
                    cmd.Parameters.AddWithValue("@direccion", cliente.direccion);

                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void ActualizarCliente(Clientes cliente)
        {
            using (var cn = new SqlConnection(cad_cn))
            {
                using (var cmd = new SqlCommand("sp_ActualizarCliente", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_cliente", cliente.id_cliente);
                    cmd.Parameters.AddWithValue("@nombre", cliente.nombre);
                    cmd.Parameters.AddWithValue("@apellido", cliente.apellido);
                    cmd.Parameters.AddWithValue("@telefono", cliente.telefono);
                    cmd.Parameters.AddWithValue("@direccion", cliente.direccion);

                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void EliminarCliente(string id_cliente)
        {
            using (var cn = new SqlConnection(cad_cn))
            {
                using (var cmd = new SqlCommand("sp_EliminarCliente", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_cliente", id_cliente);

                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}

