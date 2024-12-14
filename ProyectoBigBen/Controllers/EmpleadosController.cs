using Microsoft.AspNetCore.Mvc;
using ProyectoBigBen.Models;
using System.Data.SqlClient;

namespace ProyectoBigBen.Controllers
{
    public class EmpleadosController : Controller
    {
        public readonly IConfiguration _config;

        public EmpleadosController(IConfiguration config)
        {
            _config = config;
        }

        public IActionResult Index()
        {
            return View();
        }

        //LISTADO DE EMPLEADOS
        IEnumerable<Empleados> empleados()
        {
            List<Empleados> empleados = new List<Empleados>();
            using(SqlConnection cn = new SqlConnection(_config["ConnectionStrings:cn"]))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("sp_listar_empleados", cn);
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read()) {

                    empleados.Add(new Empleados()
                    {
                        id_empleado = dr.GetString(0),
                        nombre_empleado = dr.GetString(1),
                        apellido_empleado = dr.GetString(2),
                        rol = dr.GetString(3),
                        telefono = dr.GetString(4)
                    });
                }
            }
            return empleados;
        }

        //PAGINACION EN EL LISTADO
        public async Task<IActionResult> listadoEmpleados(int p = 0)
        {
            int nr = 10;
            var empleadosList = empleados().ToList();
            int tr = empleadosList.Count();
            int paginas = (tr+nr-1)/nr;
            ViewBag.paginas = paginas;
            return View(await Task.Run(() => empleadosList.Skip(p*nr).Take(nr)));
        }

        //REGISTRAR EMPLEADO
        [HttpGet]
        public IActionResult RegistrarEmpleado()
        {
            string nuevoCodigo = GenerarNuevoCodigo();

            Empleados nuevoEmpleado = new Empleados
            {
                id_empleado = nuevoCodigo
            };

            return View(nuevoEmpleado);
        }

        private string GenerarNuevoCodigo()
        {
            string nuevoCodigo = "E0001";

            using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:cn"]))
            {
                SqlCommand cmd = new SqlCommand("SELECT TOP 1 id_empleado FROM Empleado ORDER BY id_empleado DESC", cn);
                cn.Open();

                var resultado = cmd.ExecuteScalar();

                if (resultado != DBNull.Value)
                {
                    string ultimoCodigo = (string)resultado;
                    int numero = int.Parse(ultimoCodigo.Substring(1)); 
                    numero++; 
                    nuevoCodigo = "E" + numero.ToString("D4");
                }
            }

            return nuevoCodigo;
        }

        [HttpPost]
        public IActionResult RegistrarEmpleado(Empleados objE)
        {
            string mensaje = "";

            using(SqlConnection cn = new SqlConnection(_config["ConnectionStrings:cn"]))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_InsertarEmpleado",cn);
                    cn.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_empleado", objE.id_empleado);
                    cmd.Parameters.AddWithValue("@nombre", objE.nombre_empleado);
                    cmd.Parameters.AddWithValue("@apellido", objE.apellido_empleado);
                    cmd.Parameters.AddWithValue("@rol", objE.rol);
                    cmd.Parameters.AddWithValue("@telefono", objE.telefono);

                    int e = cmd.ExecuteNonQuery();

                    mensaje = $"registro insertado{e} correctamente";
                }
                catch(Exception ex) 
                {
                    mensaje = ex.Message;
                }
            }
            ViewBag.mensaje = mensaje;

            return RedirectToAction("listadoEmpleados","Empleados");
        }

        //EDITAR EMPLEADOS
        [HttpGet]
        public IActionResult EditarEmpleado(string id)
        {
            Empleados emp = new Empleados();
            using(SqlConnection cn = new SqlConnection(_config["ConnectionStrings:cn"]))
            {
                SqlCommand cmd = new SqlCommand("sp_ObtenerEmpleadoPorID @id_empleado", cn);
                cmd.Parameters.AddWithValue("@id_empleado", id);
                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        emp.id_empleado = (string)dr["id_empleado"];
                        emp.nombre_empleado = (string)dr["nombre"];
                        emp.apellido_empleado = (string)dr["apellido"];
                        emp.rol = (string)dr["rol"];
                        emp.telefono = (string)dr["telefono"];
                    }
                }
            }
            return View(emp);
        }

        [HttpPost, ActionName("EditarEmpleado")]
        public IActionResult Editar_Post(Empleados objE)
        {
            string mensaje = "";

            using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:cn"]))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_ActualizarEmpleado", cn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cn.Open();
                    cmd.Parameters.AddWithValue("@id_empleado", objE.id_empleado);
                    cmd.Parameters.AddWithValue("@nombre", objE.nombre_empleado);
                    cmd.Parameters.AddWithValue("@apellido", objE.apellido_empleado);
                    cmd.Parameters.AddWithValue("@rol", objE.rol);
                    cmd.Parameters.AddWithValue("@telefono", objE.telefono);

                    int e = cmd.ExecuteNonQuery();

                    mensaje = $"registro actualizado{e} correctamente";
                }
                catch (Exception ex)
                {
                    mensaje = ex.Message;
                }
            }
            ViewBag.mensaje = mensaje;

            return RedirectToAction("listadoEmpleados", "Empleados");
        }

        //ELIMINAR
        [HttpGet]
        public IActionResult EliminarEmpleado(String id)
        {
            Empleados emp = new Empleados();
            using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:cn"]))
            {
                SqlCommand cmd = new SqlCommand("sp_ObtenerEmpleadoPorID @id_empleado", cn);
                cmd.Parameters.AddWithValue("@id_empleado", id);
                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        emp.id_empleado = (string)dr["id_empleado"];
                        emp.nombre_empleado = (string)dr["nombre"];
                        emp.apellido_empleado = (string)dr["apellido"];
                        emp.rol = (string)dr["rol"];
                        emp.telefono = (string)dr["telefono"];
                    }
                }
            }
            return View(emp);
        }

        [HttpPost, ActionName("EliminarEmpleado")]
        public IActionResult EliminarEmp(string id)
        {
            string mensaje = "";

            using (SqlConnection cn = new SqlConnection(_config["ConnectionStrings:cn"]))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_EliminarEmpleado", cn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cn.Open();
                    cmd.Parameters.AddWithValue("@id_empleado", id);
                    int c = cmd.ExecuteNonQuery();
                    mensaje = $"registro eliminado {c}";
                }
                catch (Exception ex)
                {
                    mensaje = ex.Message;
                }
            }
            ViewBag.mensaje = mensaje;
            return RedirectToAction("listadoEmpleados");
        }
    }
}
