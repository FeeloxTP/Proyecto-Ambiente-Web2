using Microsoft.AspNetCore.Mvc;
using Proyecto.Application.DTOs;
using Proyecto.Application.Services.Interfaces;
using Proyecto.Infraestructure.Models;
using Proyecto.Infraestructure.Repository.Implementations;
using Proyecto.Infraestructure.Repository.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Proyecto.Web.Controllers;

public class ReportesController : Controller
{
    private readonly IServiceReportes _servicioReporte;
    private readonly IServiceNFT _serviceNFT;
    private readonly IServiceFactura _serviceFactura;
    private readonly IServiceCliente _serviceCliente;
    private readonly IServiceMovimientosCompras _serviceMovimientosCompras;
    public ReportesController(IServiceReportes servicioReporte,
                                               IServiceNFT serviceNFT,
                                               IServiceFactura serviceFactura, 
                                               IServiceCliente serviceCliente,
                                               IServiceMovimientosCompras serviceMovimientosCompras)
    {
        _servicioReporte = servicioReporte;
        _serviceNFT = serviceNFT;
        _serviceFactura = serviceFactura;
        _serviceCliente = serviceCliente;
        _serviceMovimientosCompras = serviceMovimientosCompras;

    }
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult ClienteReportByNFT()
    {
        return View();
    }


    public async Task<IActionResult> ClienteReportByNFTResult(string nombreNFT)
    {
        var collection = await _serviceNFT.FindByNameXReporteAsync(nombreNFT);
        ClienteDTO cliente = new ClienteDTO();
        if (collection.Any())
        {
            var nftId = collection.First().IdNft;
            var collectionMovimientos = await _serviceMovimientosCompras.FindByIdNFT(nftId);
            // preguntamos cual es el dueño para despues cargar los datos de ese cliente
            var clienteMovimientos = ((IEnumerable<MovimientosComprasDTO>)collectionMovimientos).ToList();
            var clienteDict = clienteMovimientos.ToDictionary(c => c.Estado, c => c.ClienteId);

            if (clienteDict.ContainsKey(true))
            {
                Guid id = clienteDict[true];
                //cliente
                cliente = await _serviceCliente.FindByIdAsync(id);
            }
            //pasamos la info del nft a la vista para usarla con el cliente
            ViewBag.nft = collection;
        }
        else
        {
            // Maneja el caso en que la colección esté vacía...
            throw new Exception("La colección NFT está vacía.");
        }

        return PartialView("details", cliente);
    }

}
