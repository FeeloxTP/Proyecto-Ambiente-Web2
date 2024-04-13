using Microsoft.AspNetCore.Mvc;
using Proyecto.Application.Services.Interfaces;

namespace Proyecto.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NftController : Controller
{
    private readonly IServiceNFT _serviceNFT;
    private readonly IServiceCliente _serviceCliente;
 

    public NftController(IServiceNFT serviceNFT)
    {
        _serviceNFT = serviceNFT;
    }

    [HttpGet("nft")]
    public async Task<IActionResult> GetAllNft()
    {
        var collection = await _serviceNFT.ListAsync();
        var responseData = collection.Select(n => new { n.IdNft, n.Nombre, n.Imagen }).ToList();
        return Ok(responseData);
    }

    //NO COPIAR NO ESTA TERMINADO!!!!

    [HttpGet("nft/Nombre/{nombre}")]
    public async Task<IActionResult> GetInfoNftByName(string nombre)
    {
        var nftCollection = await _serviceNFT.FindByNameAsync(nombre);
        var ownerCollection = await _serviceNFT.FindByNameAsync(nombre);
        var MovementsPurchaseCollection = await _serviceNFT.FindByNameAsync(nombre);

        if (nftCollection != null)
        {
            var responseData = nftCollection.Select(n => new { n.IdNft, n.Nombre, n.Imagen }).ToList();
            return Ok(responseData);
        }
        else
            return NotFound($"No existe {nombre}");
    }
}
