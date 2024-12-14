using System.ComponentModel.DataAnnotations;

namespace ProyectoBigBen.Models
{
    public class Clientes
    {
        public string id_cliente { get; set; }

        public string nombre { get; set; }

        public string apellido {  get; set; }

        public string telefono {  get; set; }

        public string direccion {  get; set; }          
    }
}
