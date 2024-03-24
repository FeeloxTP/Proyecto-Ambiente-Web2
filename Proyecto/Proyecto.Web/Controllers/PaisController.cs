using Microsoft.AspNetCore.Mvc;
using Proyecto.Application.DTOs;
using Proyecto.Application.Services.Interfaces;

namespace Proyecto.Web.Controllers;

public class PaisController : Controller
{
    private readonly IServicesPais _servicePais;

    public PaisController(IServicesPais servicePais)
    {
        _servicePais = servicePais;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var collection = await _servicePais.ListAsync();
        return View(collection);
    }

    // GET: BodegaController/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: BodegaController/Create
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

        await _servicePais.AddAsync(dto);


        return RedirectToAction("Index");

    }


    // GET: BodegaController/Details/5
    public async Task<IActionResult> Details(string id)
    {
        var @object = await _servicePais.FindByIdAsync(id);

        return PartialView(@object);
    }

    // GET: BodegaController/Edit/5
    public async Task<IActionResult> Edit(string id)
    {
        var @object = await _servicePais.FindByIdAsync(id);

        return View(@object);
    }

    // POST: BodegaController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, PaisDTO dto)
    {

        await _servicePais.UpdateAsync(id, dto);

        return RedirectToAction("Index");

    }

    // GET: BodegaController/Delete/5
    public async Task<IActionResult> Delete(string id)
    {
        var @object = await _servicePais.FindByIdAsync(id);

        return View(@object);

    }

    // POST: BodegaController/Delete/5
    [HttpPost]
    public async Task<IActionResult> DeleteConfirm(string id)
    {
        await _servicePais.DeleteAsync(id);

        return RedirectToAction("Index");
    }

}
