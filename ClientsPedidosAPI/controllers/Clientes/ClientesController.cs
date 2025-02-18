using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

[Route("api/clientes")]
[ApiController]

public class ClientesController : ControllerBase
{
    private readonly AppDbContext _context;

    public ClientesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CriarCliente([FromBody] Cliente cliente)
    {

        if (!CpfValidator.isValid(cliente.CPF))
            return BadRequest("CPF Inválido!");
        
        if (await _context.Clientes.AnyAsync(c => c.CPF == cliente.CPF))
        return BadRequest("CPF já Cadastrado!");
        
        if ((DateTime.Now.Year - cliente.DataNascimento.Year) < 18)
        return BadRequest("Cliente deve ter 18 anos ou mais!");

        var emailAttribute = new EmailAddressAttribute();
        if (!emailAttribute.IsValid(cliente.Email))
            return BadRequest("Email Inválido!");

        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(ObterCliente), new { id = cliente.Id}, cliente);
    }

    [HttpGet("{id}")]
     public async Task<IActionResult> ObterCliente(int id)
    {
        var cliente = await _context.Clientes
            .Include(c => c.Pedidos)
            .ThenInclude(p => p.Itens)
            .FirstOrDefaultAsync(c => c.Id == id);
        
        if (cliente == null) return NotFound();
        return Ok(cliente);
    }
}