using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto.Application.DTOs;
using Proyecto.Application.Services.Implementations;
using Proyecto.Application.Services.Interfaces;
using System.Text.Json;
using X.PagedList;

namespace Proyecto.Web.Controllers;

[Authorize(Roles = "Admin,Procesos")]
public class FacturaController : Controller
{
    private readonly IServiceNFT _serviceNFT;
    private readonly IServicesTarjeta _serviceTarjeta;
    private readonly IServiceFactura _serviceFactura;
    private readonly IServiceCliente _serviceCliente;

    public FacturaController(IServiceNFT serviceNFT,
                         IServicesTarjeta servicoTarjeta,
                         IServiceFactura serviceFactura,
                         IServiceCliente serviceCliente)
    {
        _serviceNFT = serviceNFT;
        _serviceTarjeta = servicoTarjeta;
        _serviceFactura = serviceFactura;
        _serviceCliente = serviceCliente;
    }

    public async Task<IActionResult> Index()
    {
        var nextReceiptNumber = await _serviceFactura.GetNextReceiptNumber();
        ViewBag.CurrentReceipt = nextReceiptNumber;
        var collection = await _serviceTarjeta.ListAsync();
        ViewBag.ListTarjeta = collection;

        // Clear CarShopping
        TempData["CartShopping"] = null;
        TempData.Keep();

        return View();
    }

    public async Task<IActionResult> AnularFacturaIndex(int? page)
    {
        ViewBag.clientes = await _serviceCliente.ListAsync();
        var collection = await _serviceFactura.ListAsync();
        return View(collection.ToPagedList(page ?? 1, 5));
    }

    public async Task<IActionResult> List(string nombre)
    {
        if (string.IsNullOrEmpty(nombre))
            return RedirectToAction("AnularFacturaIndex");

        ViewBag.clientes = await _serviceCliente.ListAsync();
        var collection = await _serviceFactura.FindByClientNameAsync(nombre);
        return View("AnularFacturaIndex", collection.ToPagedList(1, 10));
    }


    public async Task<IActionResult> CambiarEstado(int id)
    {
        var @object = await _serviceFactura.FindByIdAsync(id);
        return View(@object);

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CambiarEstado(int id, FacturaDTO dto)
    {
        await _serviceFactura.UpdateAsync(id, dto);
        return RedirectToAction("AnularFacturaIndex");

    }

    public async Task<IActionResult> AddProduct(Guid id, int cantidad)
    {
        FacturaDetalleDTO facturaDetalleDTO = new FacturaDetalleDTO();
        List<FacturaDetalleDTO> lista = new List<FacturaDetalleDTO>();
        string json = "";

        var producto = await _serviceNFT.FindByIdAsync(id);

        // Stock ??
        if (cantidad > producto.Inventario)
        {
            return BadRequest("No hay inventario suficiente!");
        }


        facturaDetalleDTO.DescripcionProducto = producto.Nombre;
        facturaDetalleDTO.Cantidad = cantidad;
        facturaDetalleDTO.Precio = producto.Precio;
        facturaDetalleDTO.IdNFT = id;
        facturaDetalleDTO.TotalLinea = (facturaDetalleDTO.Precio * facturaDetalleDTO.Cantidad);
        if (TempData["CartShopping"] == null)
        {
            lista.Add(facturaDetalleDTO);
            // Reenumerate 
            int idx = 1;
            lista.ForEach(p => p.Secuencia = idx++);
            json = JsonSerializer.Serialize(lista);
            TempData["CartShopping"] = json;
        }
        else
        {
            json = (string)TempData["CartShopping"]!;
            lista = JsonSerializer.Deserialize<List<FacturaDetalleDTO>>(json!)!;
            lista.Add(facturaDetalleDTO);
            // Reenumerate 
            int idx = 1;
            lista.ForEach(p => p.Secuencia = idx++);
            json = JsonSerializer.Serialize(lista);
            TempData["CartShopping"] = json;
        }

        TempData.Keep();
        return PartialView("_DetailFactura", lista);
    }

    public IActionResult GetDetailFactura()
    {
        List<FacturaDetalleDTO> lista = new List<FacturaDetalleDTO>();
        string json = "";
        json = (string)TempData["CartShopping"]!;
        lista = JsonSerializer.Deserialize<List<FacturaDetalleDTO>>(json!)!;
        // Reenumerate 
        int idx = 1;
        lista.ForEach(p => p.Secuencia = idx++);
        json = JsonSerializer.Serialize(lista);
        TempData["CartShopping"] = json;
        TempData.Keep();

        return PartialView("_DetailFactura", lista);
    }

    public IActionResult DeleteProduct(int id)
    {
        FacturaDetalleDTO facturaDetalleDTO = new FacturaDetalleDTO();
        List<FacturaDetalleDTO> lista = new List<FacturaDetalleDTO>();
        string json = "";

        if (TempData["CartShopping"] != null)
        {
            json = (string)TempData["CartShopping"]!;
            lista = JsonSerializer.Deserialize<List<FacturaDetalleDTO>>(json!)!;
            // Remove from list by Index
            int idx = lista.FindIndex(p => p.Secuencia == id);
            lista.RemoveAt(idx);
            json = JsonSerializer.Serialize(lista);
            TempData["CartShopping"] = json;
        }

        TempData.Keep();

        // return Content("Ok");
        return PartialView("_DetailFactura", lista);

    }


    public async Task<IActionResult> Create(FacturaDTO facturaEncabezadoDTO)
    {
        string json;
        try
        {

            // IdClient exist?
            var cliente = await _serviceCliente.FindByIdAsync(facturaEncabezadoDTO.IdCliente);
            if (cliente == null)
            {
                // Keep Cache data
                TempData.Keep();
                return BadRequest("Cliente No Existe!");
            }


            // TODO: Validate! 
            if (!ModelState.IsValid)
            {

            }

            json = (string)TempData["CartShopping"]!;

            if (string.IsNullOrEmpty(json))
            {
                return BadRequest("No hay datos por facturar");
            }

            var lista = JsonSerializer.Deserialize<List<FacturaDetalleDTO>>(json!)!;

            if (lista.Count == 0 || lista == null)
            {
                return BadRequest("No hay datos por facturar");
            }

            //Mismo numero de factura para el detalle FK
            lista.ForEach(p => p.IdFactura = facturaEncabezadoDTO.IdFactura);
            facturaEncabezadoDTO.ListFacturaDetalle = lista;

            await _serviceFactura.AddAsync(facturaEncabezadoDTO);


            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            // Keep Cache data
            TempData.Keep();
            return BadRequest(ex.Message);
        }
    }


}
