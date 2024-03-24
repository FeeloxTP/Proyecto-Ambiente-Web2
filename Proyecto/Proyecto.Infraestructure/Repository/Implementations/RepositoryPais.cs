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

public class RepositoryPais : IRepositoryPais
{
    private readonly ProyectoContext _context;

    public RepositoryPais(ProyectoContext context)
    {
        _context = context;
    }

    public async Task<string> AddAsync(Pais entity)
    {
        await _context.Set<Pais>().AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity.IdPais;
    }

    public async Task DeleteAsync(string id)
    {
        // Raw Query
        //https://www.learnentityframeworkcore.com/raw-sql/execute-sql
        int rowAffected = _context.Database.ExecuteSql($"Delete Pais Where IdPais = {id}");
        await Task.FromResult(1);

    }

    public async Task<ICollection<Pais>> FindByDescriptionAsync(string description)
    {
        var collection = await _context
                                     .Set<Pais>()
                                     .Where(p => p.Descripcion.Contains(description))
                                     .ToListAsync();
        return collection;
    }

    public async Task<Pais> FindByIdAsync(string id)
    {
        var @object = await _context.Set<Pais>().FindAsync(id);
        return @object!;
    }

    public async Task<ICollection<Pais>> ListAsync()
    {
        var collection = await _context.Set<Pais>().AsNoTracking().ToListAsync();
        return collection;
    }

    public async Task UpdateAsync(string id, Pais entity)
    {
        var @object = await FindByIdAsync(id);
        @object.Descripcion = entity.Descripcion;
        await _context.SaveChangesAsync();
    }
}
