using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto.Application.DTOs;
using Proyecto.Application.Services.Implementations;
using Proyecto.Application.Services.Interfaces;
using X.PagedList;

namespace Proyecto.Web.Controllers;

[Authorize(Roles = "Admin,Procesos")]
public class TarjetaController : Controller
{
    private readonly IServicesTarjeta _serviceTarjeta;

    public TarjetaController(IServicesTarjeta serviceTarjeta)
    {
        _serviceTarjeta = serviceTarjeta;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int? page)
    {
        var collection = await _serviceTarjeta.ListAsync();
        return View(collection.ToPagedList(page ?? 1, 5));
    }

    public async Task<IActionResult> List(string descripcion)
    {
        if (string.IsNullOrEmpty(descripcion))
            return RedirectToAction("Index");

        var collection = await _serviceTarjeta.FindByDescriptionAsync(descripcion);
        return View("index", collection.ToPagedList(1, 10));
    }

    // GET: BodegaController/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: BodegaController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TarjetaDTO dto)
    {

        if (!ModelState.IsValid)
        {
            // Lee del ModelState todos los errores que
            // vienen para el lado del server
            string errors = string.Join("; ", ModelState.Values
                               .SelectMany(x => x.Errors)
                               .Select(x => x.ErrorMessage));
            return BadRequest(errors);
        }

        var Tarejetas = await _serviceTarjeta.FindByIdAsync(dto.IdTarjeta);

        if (Tarejetas != null)
        {
            ViewData["Mensaje"] = "Ya se encuentra registrado esta Tarjeta!";
            return View("Create");
        }

        await _serviceTarjeta.AddAsync(dto);


        return RedirectToAction("Index");

    }


    // GET: BodegaController/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var @object = await _serviceTarjeta.FindByIdAsync(id);

        return PartialView(@object);
    }

    // GET: BodegaController/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var @object = await _serviceTarjeta.FindByIdAsync(id);

        return View(@object);
    }

    // POST: BodegaController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TarjetaDTO dto)
    {
        var tarjetas = await _serviceTarjeta.FindByDescriptionAsync(dto.DescripcionTarjeta);

        if (tarjetas.Count() > 0)
        {
            ViewData["Mensaje"] = "Ya se encuentra registrado esta Tarjeta!";
            return View("Edit");
        }

        await _serviceTarjeta.UpdateAsync(id, dto);

        return RedirectToAction("Index");

    }

    // GET: BodegaController/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var @object = await _serviceTarjeta.FindByIdAsync(id);

        return View(@object);

    }

    // POST: BodegaController/Delete/5
    [HttpPost]
    public async Task<IActionResult> DeleteConfirm(int id)
    {
        await _serviceTarjeta.DeleteAsync(id);

        return RedirectToAction("Index");
    }
}
