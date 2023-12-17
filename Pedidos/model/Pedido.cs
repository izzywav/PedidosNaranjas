namespace Pedidos.model
{
    public class Pedido
    {
        public string NPedido { get; set; }
        public string Cliente { get; set; }
        public string DNI { get; set; }
        public int Cantidad { get; set; }
        public string Fecha { get; set; }

        public Pedido()
        {
            NPedido = "";
            Cliente = "";
            DNI = "";
            Cantidad = 0;
            Fecha = "";
        }
    }
}
