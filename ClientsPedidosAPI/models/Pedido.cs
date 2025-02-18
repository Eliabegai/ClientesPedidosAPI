public class Pedido
{
  public int Id { get; set; }
  public int ClienteId { get; set; }
  public DateTime DataPedido { get; set; }
  public decimal ValorTotal { get; set; }
  public Cliente? Cliente { get; set; }
  public ICollection<ItemPedido> Itens { get; set; } = new List<ItemPedido>();
}