using Microsoft.AspNetCore.Mvc;
using Proyecto.Application.Services.Interfaces;

namespace Proyecto.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NftController : Controller
{
    private readonly IServiceNFT _serviceNFT;

    public NftController(IServiceNFT serviceNFT)
    {
        _serviceNFT = serviceNFT;
    }

    [HttpGet("nft")]
    public async Task<IActionResult> GetAllNft()
    {
        var collection = await _serviceNFT.ListAsync();
        return Ok(collection);
    }

    [HttpGet("nft/Nombre/{nombre}")]
    public async Task<IActionResult> GetProductoByDescription(string nombre)
    {
        var collection = await _serviceNFT.FindByNameAsync(nombre);

        if (collection != null)
            return Ok(collection);
        else
            return NotFound($"No existe {nombre}");
    }
}
