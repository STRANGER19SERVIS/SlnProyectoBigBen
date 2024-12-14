using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ProyectoBigBen.Models
{
    public class Empleados
    {
        [DisplayName("ID")]
        [Required(ErrorMessage = "El id del empleado es obligatorio")]
        public string id_empleado { get; set; }
        [DisplayName("Nombre")]
        [Required(ErrorMessage = "El nombre del empleado es obligatorio")]
        public string nombre_empleado { get; set; }
        [DisplayName("Apellidos")]
        [Required(ErrorMessage = "El apellido del empleado es obligatorio")]
        public string apellido_empleado { get; set; }
        [DisplayName("Rol")]
        [Required(ErrorMessage = "El rol es obligatorio")]
        public string rol { get; set; }
        [RegularExpression(@"^9\d{8}$", ErrorMessage = "El teléfono debe tener 9 dígitos numéricos y debe comenzar con 9.")]
        [DisplayName("Teléfono")]
        [Required(ErrorMessage = "El telefono es obligatorio")]
        public string telefono { get; set; }
    }
}
