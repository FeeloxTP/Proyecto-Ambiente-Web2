using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto.Application.DTOs;
using Proyecto.Application.Services.Implementations;
using Proyecto.Application.Services.Interfaces;

namespace Proyecto.Web.Controllers;

[Authorize(Roles = "Admin")]
public class UsuarioController : Controller
{
    private readonly IServiceUsuario _serviceUsuario;
    private readonly IServicePerfil _servicePeril;

    public UsuarioController(IServiceUsuario serviceUsuario, IServicePerfil servicePeril)
    {
        _serviceUsuario = serviceUsuario;
        _servicePeril = servicePeril;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var collection = await _serviceUsuario.ListAsync();
        return View(collection);
    }

    // GET: UsuarioController/Create
    public async Task<IActionResult> Create()
    {
        ViewBag.ListaPerfiles = await _servicePeril.ListAsync();
        return View();
    }

    // GET:  
    public async Task<IActionResult> Login(string id, string password)
    {
        var @object = await _serviceUsuario.LoginAsync(id, password);
        if (@object == null)
        {
            ViewBag.Message = "Error en Login o Password";
            return View("Login");
        }
        else
        {
            return RedirectToAction("Index", "Home");
        }
    }


    // POST: UsuarioController/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UsuarioDTO dto)
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

        await _serviceUsuario.AddAsync(dto);
        return RedirectToAction("Index");
    }

    // GET: UsuarioController/Details/5
    public async Task<IActionResult> Details(string id)
    {
        var @object = await _serviceUsuario.FindByIdAsync(id);
        return PartialView(@object);
    }

    // GET: UsuarioController/Edit/5
    public async Task<IActionResult> Edit(string id)
    {
        var @object = await _serviceUsuario.FindByIdAsync(id);
        ViewBag.ListaPerfiles = await _servicePeril.ListAsync();
        return View(@object);
    }

    // POST: UsuarioController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, UsuarioDTO dto)
    {
        await _serviceUsuario.UpdateAsync(id, dto);
        return RedirectToAction("Index");
    }

    // GET: UsuarioController/Delete/5
    public async Task<IActionResult> Delete(string id)
    {
        var @object = await _serviceUsuario.FindByIdAsync(id);
        return View(@object);
    }

    // POST: UsuarioController/Delete/5
    [HttpPost]
    public async Task<IActionResult> DeleteConfirm(string id)
    {
        await _serviceUsuario.DeleteAsync(id);
        return RedirectToAction("Index");
    }
}
