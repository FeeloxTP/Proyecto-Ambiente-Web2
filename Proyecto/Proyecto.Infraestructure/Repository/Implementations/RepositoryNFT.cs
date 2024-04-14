using Microsoft.EntityFrameworkCore;
using Proyecto.Infraestructure.Data;
using Proyecto.Infraestructure.Models;
using Proyecto.Infraestructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Infraestructure.Repository.Implementations;

public class RepositoryNFT : IRepositoryNFT
{
    private readonly ProyectoContext _context;

    public RepositoryNFT(ProyectoContext context)
    {
        _context = context;
    }
    public async Task<ICollection<ActivoNft>> FindByNameAsync(string name)
    {
        var collection = await _context
                                    .Set<ActivoNft>()
                                    .Where(p => p.Nombre.Contains(name))
                                    .ToListAsync();
        return collection;
    }

    public async Task<ICollection<ActivoNft>> FindByNameXReporteAsync(string name)
    {
        var collection = await _context
                                    .Set<ActivoNft>()
                                    .Where(p => p.Nombre.Contains(name))
                                    .ToListAsync();
        return collection;
    }

    public async Task<Guid> AddAsync(ActivoNft entity)
    {
        await _context.Set<ActivoNft>().AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity.IdNft;
    }

    public async Task DeleteAsync(Guid id)
    {
        var @object = await FindByIdAsync(id);
        _context.Remove(@object);
        _context.SaveChanges();
    }

    public async Task<ActivoNft> FindByIdAsync(Guid id)
    {
        var @object = await _context.Set<ActivoNft>().FindAsync(id);
        return @object!;
    }

    public async Task<ICollection<ActivoNft>> FindByDescriptionAsync(string description)
    {
        var collection = await _context
                                    .Set<ActivoNft>()
                                    .Where(p => p.Nombre.Contains(description))
                                    .ToListAsync();
        return collection;
    }

    public async Task<ICollection<ActivoNft>> ListAsync()
    {
        var collection = await _context.Set<ActivoNft>().AsNoTracking().ToListAsync();
        return collection;
    }

    public async Task UpdateAsync(Guid id, ActivoNft entity)
    {
        var @object = await FindByIdAsync(id);
        @object.Nombre = entity.Nombre;
        @object.Autor = entity.Autor;
        @object.Precio = entity.Precio;
        @object.Inventario = entity.Inventario;
        @object.Imagen = entity.Imagen;
        await _context.SaveChangesAsync();
    }
}

