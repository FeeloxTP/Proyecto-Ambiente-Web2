using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Proyecto.Application.DTOs;
using Proyecto.Application.Services.Implementations;
using Proyecto.Application.Services.Interfaces;
using Proyecto.Infraestructure.Models;
using Proyecto.Infraestructure.Repository.Implementations;
using Proyecto.Infraestructure.Repository.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;


namespace Proyecto.Web.Controllers;

[Authorize(Roles = "Admin,Reportes")]
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

    public IActionResult ReporteClientes()
    {
        return View();
    }

    public IActionResult ReporteNFTs()
    {
        return View();
    }

    public IActionResult ClienteReportByNFT()
    {
        return View();
    }

    public IActionResult ReporteVentasByFecha()
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
            //validamos si tiene movimientos
            if (collectionMovimientos.Any())
            {
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
                return BadRequest("No hay registros!");
            }
           
        }
        else
        {
            // Maneja el caso en que la colección esté vacía...
            return BadRequest("No existe ese NFT!");
        }

        return PartialView("details", cliente);
    }

    [HttpPost]
    [RequireAntiforgeryToken]
    public async Task<FileResult> ClientesReportPDF()
    {
        byte[] bytes = await _servicioReporte.ClientesReportPDF();
        return File(bytes, "text/plain", "clientes.pdf");
    }

    [HttpPost]
    [RequireAntiforgeryToken]
    public async Task<FileResult> NFTsReportPDF()
    {
        byte[] bytes = await _servicioReporte.NFTsReportPDF();
        return File(bytes, "text/plain", "NFTs.pdf");
    }

    [HttpPost]
    [RequireAntiforgeryToken]
    public async Task<IActionResult> VentasReportByFechaPDF(DateTime fechaInicial, DateTime fechaFinal)
    {
        //fecha inicial
        if (string.IsNullOrEmpty(fechaInicial.ToString()))
        {
            ViewBag.Message = "La fecha inicial es requerida";
            return View("ReporteVentasByFecha");
        }

        if (string.IsNullOrEmpty(fechaFinal.ToString()))
        {
            ViewBag.Message = "La fecha final es requerida";
            return View("ReporteVentasByFecha");
        }

        byte[] bytes = await _servicioReporte.VentaReporteByFechas(fechaInicial, fechaFinal);
        return File(bytes, "text/plain", "ventas.pdf");
    } 
}
