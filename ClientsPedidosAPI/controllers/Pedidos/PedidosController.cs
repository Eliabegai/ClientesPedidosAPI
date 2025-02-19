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
        var pedido = await _context.Pedidos
            .Include(p => p.Itens)
            .Include(p => p.Cliente)
            .FirstOrDefaultAsync(p => p.Id == id);
        
        if (pedido == null) return NotFound();
        return Ok(pedido);
    }

    [HttpGet]
    public async Task<IActionResult> ListarPedidos([FromQuery] string? clienteNome, [FromQuery] DateTime? inicio, [FromQuery] DateTime? fim)
    {
        var query = _context.Pedidos.Include(p => p.Cliente).Include(p => p.Itens).AsQueryable();
        
        if (!string.IsNullOrEmpty(clienteNome))
            query = query.Where(p => p.Cliente != null && p.Cliente.Nome.Contains(clienteNome));
        
        if (inicio.HasValue && fim.HasValue)
        {
            var inicioUtc = DateTime.SpecifyKind(inicio.Value, DateTimeKind.Utc);
            var fimUtc = DateTime.SpecifyKind(fim.Value, DateTimeKind.Utc);
            query = query.Where(p => p.DataPedido >= inicioUtc && p.DataPedido <= fimUtc);
        }
        else if (inicio.HasValue)
        {
            var inicioUtc = DateTime.SpecifyKind(inicio.Value, DateTimeKind.Utc);
            query = query.Where(p => p.DataPedido >= inicioUtc);
        }
        else if (fim.HasValue)
        {
            var fimUtc = DateTime.SpecifyKind(fim.Value, DateTimeKind.Utc);
            query = query.Where(p => p.DataPedido <= fimUtc);
        }
        
        return Ok(await query.ToListAsync());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarPedido(int id, [FromBody] Pedido pedidoAtualizado)
    {
        var pedidoExistente = await _context.Pedidos
            .Include(p => p.Itens)
            .FirstOrDefaultAsync(p => p.Id == id);
        
        if (pedidoExistente == null) return NotFound();

        // Verifica se a data de criação do pedido passou de 24h
        if((DateTime.UtcNow - pedidoExistente.DataPedido).TotalHours > 24)
            return BadRequest("Não é possível alterar pedidos com mais de 24h de criação!");

        // Atualiza os campos do pedido existente com os novos dados

        pedidoExistente.ClienteId = pedidoAtualizado.ClienteId;
        pedidoExistente.DataPedido = DateTime.SpecifyKind(pedidoAtualizado.DataPedido, DateTimeKind.Utc);
        pedidoExistente.ValorTotal = pedidoAtualizado.Itens.Sum(i => i.Subtotal);


        // Atualiza os itens do pedido
        pedidoExistente.Itens.Clear();
        foreach (var item in pedidoAtualizado.Itens)
        {
            pedidoExistente.Itens.Add(new ItemPedido
            {
                Produto = item.Produto,
                Quantidade = item.Quantidade,
                PrecoUnitario = item.PrecoUnitario
            });
        }

        pedidoExistente.ValorTotal = pedidoExistente.Itens.Sum(i => i.Quantidade * i.PrecoUnitario);

        _context.Pedidos.Update(pedidoExistente);
        await _context.SaveChangesAsync();

        return Ok(pedidoExistente);

    } 
}