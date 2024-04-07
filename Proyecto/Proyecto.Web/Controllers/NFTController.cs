﻿using Microsoft.AspNetCore.Mvc;
using Proyecto.Application.DTOs;
using Proyecto.Application.Services.Interfaces;

namespace Proyecto.Web.Controllers;

public class NFTController : Controller
{
    private readonly IServiceNFT _serviceNFT;

    public NFTController(IServiceNFT serviceNFT)
    {
        _serviceNFT = serviceNFT;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var collection = await _serviceNFT.ListAsync();
        return View(collection);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(NFTDTO dto, IFormFile imageFile)
    {
        dto.IdNft = Guid.NewGuid();

        MemoryStream target = new MemoryStream();

        // Cuando es Insert Image viene en null porque se pasa diferente
        if (dto.Imagen == null)
        {
            if (imageFile != null)
            {
                imageFile.OpenReadStream().CopyTo(target);

                dto.Imagen = target.ToArray();
                ModelState.Remove("Imagen");
            }
        }

        if (!ModelState.IsValid)
        {
            // Lee del ModelState todos los errores que
            // vienen para el lado del server
            string errors = string.Join("; ", ModelState.Values
                               .SelectMany(x => x.Errors)
                               .Select(x => x.ErrorMessage));
            return BadRequest(errors);
        }

        await _serviceNFT.AddAsync(dto);
        return RedirectToAction("Index");

    }

    public async Task<IActionResult> Details(Guid id)
    {
        var @object = await _serviceNFT.FindByIdAsync(id);
        return PartialView(@object);
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var @object = await _serviceNFT.FindByIdAsync(id);

        return View(@object);

    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirm(Guid id)
    {
        await _serviceNFT.DeleteAsync(id);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var @object = await _serviceNFT.FindByIdAsync(id);
        return View(@object);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Guid id, NFTDTO dto, IFormFile imageFile)
    {
        // Verificar si se ha seleccionado un nuevo archivo de imagen
        if (imageFile != null && imageFile.Length > 0)
        {
            // Si se ha seleccionado un archivo de imagen, actualizar la imagen en el DTO
            using (var memoryStream = new MemoryStream())
            {
                await imageFile.CopyToAsync(memoryStream);
                dto.Imagen = memoryStream.ToArray();
            }
        }
        else
        {
            NFTDTO nft = await _serviceNFT.FindByIdAsync(id);
            dto.Imagen = nft.Imagen;
        }

        // Realizar la actualización del NFT con los datos proporcionados
        await _serviceNFT.UpdateAsync(id, dto);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> GetProductoByName(string filtro)
    {

        var collection = await _serviceNFT.FindByDescriptionAsync(filtro);
        return Json(collection);

    }
}
