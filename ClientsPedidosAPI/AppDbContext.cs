using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
  public DbSet<Cliente> Clientes { get; set; }
  public DbSet<Pedido> Pedidos {get; set;}
  public DbSet<ItemPedido> ItensPedido {get; set;}

  public AppDbContext(DbContextOptions<AppDbContext> options): base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>()
          .HasIndex(c => c.CPF)
          .IsUnique();
        
        modelBuilder.Entity<Pedido>()
          .HasOne(p => p.Cliente)
          .WithMany(c => c.Pedidos)
          .HasForeignKey(p => p.ClienteId);
    }
}