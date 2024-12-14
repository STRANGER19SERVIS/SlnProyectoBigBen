namespace ProyectoBigBen.Models
{
    public class Pedidos
    {
        public string id_pedido { get; set; }
        public DateTime fecha_hora { get; set; }
        public double total { get; set; }
        public string estado { get; set; }
    }
}
