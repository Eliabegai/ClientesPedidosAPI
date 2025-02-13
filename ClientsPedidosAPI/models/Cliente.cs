using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Cliente
{
  public int Id { get; set; }

  [Required]
  public string Nome { get; set; }

  [Required, EmailAddress]
  public string Email { get; set; }

  [Required]
  public DateTime DataNascimento  { get; set; }

  [Required, StringLength(11)]
  public string CPF { get; set; }

  public string Endereco { get; set; }

  public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}