using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto.Application.DTOs;
using Proyecto.Application.Services.Implementations;
using Proyecto.Application.Services.Interfaces;
using X.PagedList;

namespace Proyecto.Web.Controllers;

[Authorize(Roles = "Admin,Procesos")]
public class PaisController : Controller
{
    private readonly IServicesPais _servicePais;

    public PaisController(IServicesPais servicePais)
    {
        _servicePais = servicePais;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int? page)
    {
        var collection = await _servicePais.ListAsync();
        return View(collection.ToPagedList(page ?? 1, 5));
    }

    public async Task<IActionResult> List(string descripcion)
    {
        if (string.IsNullOrEmpty(descripcion))
            return RedirectToAction("Index");

        var collection = await _servicePais.FindByDescriptionAsync(descripcion);
        return View("index", collection.ToPagedList(1, 10));
    }

    // GET: PaisController/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: PaisController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PaisDTO dto)
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
        var paises = await _servicePais.FindByIdAsync(dto.IdPais);

        if (paises != null)
        {
            ViewData["Mensaje"] = "Ya se encuentra registrado este Pais!";
            return View("Create");
        }
        //no estra al if se agrega normal 
        await _servicePais.AddAsync(dto);


        return RedirectToAction("Index");

    }

    public async Task<IActionResult> Details(string id)
    {
        var @object = await _servicePais.FindByIdAsync(id);

        return PartialView(@object);
    }

    // GET: PaisController/Edit/5
    public async Task<IActionResult> Edit(string id)
    {
        var @object = await _servicePais.FindByIdAsync(id);

        return View(@object);
    }

    // POST: PaisController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, PaisDTO dto)
    {
        var paises = await _servicePais.FindByDescriptionAsync(dto.Descripcion);

        if (paises.Count() > 0)
        {
            ViewData["Mensaje"] = "Ya se encuentra registrado este Pais!";
            return View("Edit");
        }

        await _servicePais.UpdateAsync(id, dto);

        return RedirectToAction("Index");

    }

    // GET: PaisController/Delete/5
    public async Task<IActionResult> Delete(string id)
    {
        var @object = await _servicePais.FindByIdAsync(id);

        return View(@object);

    }

    // POST: PaisController/Delete/5
    [HttpPost]
    public async Task<IActionResult> DeleteConfirm(string id)
    {
        await _servicePais.DeleteAsync(id);

        return RedirectToAction("Index");
    }

}
