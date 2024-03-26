﻿using Microsoft.AspNetCore.Mvc;
using Proyecto.Application.DTOs;
using Proyecto.Application.Services.Implementations;
using Proyecto.Application.Services.Interfaces;
using Proyecto.Infraestructure.Models;

namespace Proyecto.Web.Controllers
{
    public class ClienteController : Controller
    {
        private readonly IServiceCliente _serviceCliente;
        private readonly IServicesPais _servicePais;

        public ClienteController(IServiceCliente serviceCliente, IServicesPais servicePais)
        {
            _serviceCliente = serviceCliente;
            _servicePais = servicePais;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var collection = await _serviceCliente.ListAsync();
            return View(collection);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.ListPais = await _servicePais.ListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClienteDTO dto)
        {
            dto.IdCliente = Guid.NewGuid();

            if (!ModelState.IsValid)
            {
                // Lee del ModelState todos los errores que
                // vienen para el lado del server
                string errors = string.Join("; ", ModelState.Values
                                   .SelectMany(x => x.Errors)
                                   .Select(x => x.ErrorMessage));
                return BadRequest(errors);
            }

            await _serviceCliente.AddAsync(dto);
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Details(Guid id)
        {
            var @object = await _serviceCliente.FindByIdAsync(id);
            return PartialView(@object);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var @object = await _serviceCliente.FindByIdAsync(id);

            return View(@object);

        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirm(Guid id)
        {
            await _serviceCliente.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var @object = await _serviceCliente.FindByIdAsync(id);
            ViewBag.ListPais = await _servicePais.ListAsync();
            return View(@object);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ClienteDTO dto)
        {
            await _serviceCliente.UpdateAsync(id, dto);
            return RedirectToAction("Index");

        }
    }
}
