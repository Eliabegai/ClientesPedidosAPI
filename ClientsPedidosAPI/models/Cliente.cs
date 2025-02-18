using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Cliente
{
  private DateTime _dataNascimento;
  public int Id { get; set; }

  [Required]
  public string Nome { get; set; }

  [Required, EmailAddress]
  public string Email { get; set; }

  [Required]
  public DateTime DataNascimento  { 
    get=> _dataNascimento; 
    set => _dataNascimento = DateTime.SpecifyKind(value, DateTimeKind.Utc); 
  }

  [Required, StringLength(11)]
  public string CPF { get; set; }

  public string Endereco { get; set; }

  public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}