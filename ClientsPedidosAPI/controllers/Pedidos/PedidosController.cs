using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/pedidos")]
[ApiController]
public class PedidosController : ControllerBase
{
    private readonly AppDbContext _context;

    public PedidosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CriarPedido([FromBody] Pedido pedido)
    {
        Console.WriteLine("Criar Pedido");
        var cliente = await _context.Clientes.FindAsync(pedido.ClienteId);
        if (cliente == null) return BadRequest("Cliente não encontrado!");

        if (!pedido.Itens.Any())
            return BadRequest("O Pedido deve conter pelo menos um item!");
        
        var newPedido = new Pedido
        {
            ClienteId = pedido.ClienteId,
            DataPedido = DateTime.SpecifyKind(pedido.DataPedido, DateTimeKind.Utc), // Convertendo para UTC
            ValorTotal = pedido.Itens.Sum(i => i.Subtotal),
            Itens = pedido.Itens.Select(i => new ItemPedido
            {
                Produto = i.Produto,
                Quantidade = i.Quantidade,
                PrecoUnitario = i.PrecoUnitario
            }).ToList()
        };

        newPedido.ValorTotal = newPedido.Itens.Sum(i => i.Quantidade * i.PrecoUnitario);

        _context.Pedidos.Add(newPedido);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(ObterPedido), new { id = newPedido.Id }, newPedido);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> ObterPedido(int id)
    {
        var pedido = await _context.Pedidos.Include(p => p.Itens)
            .FirstOrDefaultAsync(p => p.Id == id);
        
        if (pedido == null) return NotFound();
        return Ok(pedido);
    }

    [HttpGet]
    public async Task<IActionResult> ListarPedidos([FromQuery] string? clienteNome, [FromQuery] DateTime? inicio, [FromQuery] DateTime? fim)
    {
        var query = _context.Pedidos.Include(p => p.Cliente).Include(p => p.Itens).AsQueryable();

        if (!string.IsNullOrEmpty(clienteNome))
            query = query.Where(p => p.Cliente.Nome.Contains(clienteNome));
        
        if (inicio.HasValue && fim.HasValue)
            query = query.Where(p => p.DataPedido >= inicio && p.DataPedido <= fim);
        
        return Ok(await query.ToListAsync());
    }
}