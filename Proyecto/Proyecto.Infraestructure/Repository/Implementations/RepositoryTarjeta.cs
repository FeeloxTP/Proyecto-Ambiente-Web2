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

public class RepositoryTarjeta : IRepositoryTarjeta
{
    private readonly ProyectoContext _context;

    public RepositoryTarjeta(ProyectoContext context)
    {
        _context = context;
    }

    public async Task<int> AddAsync(Tarjeta entity)
    {
        await _context.Set<Tarjeta>().AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity.IdTarjeta;
    }

    public async Task DeleteAsync(int id)
    {
        // Raw Query
        //https://www.learnentityframeworkcore.com/raw-sql/execute-sql
        int rowAffected = _context.Database.ExecuteSql($"Delete Tarjeta Where IdTarjeta = {id}");
        await Task.FromResult(1);
    }

    public async Task<ICollection<Tarjeta>> FindByDescriptionAsync(string description)
    {
        var collection = await _context
                                     .Set<Tarjeta>()
                                     .Where(p => p.DescripcionTarjeta.Contains(description))
                                     .ToListAsync();
        return collection;
    }

    public async Task<Tarjeta> FindByIdAsync(int id)
    {
        var @object = await _context.Set<Tarjeta>().FindAsync(id);
        return @object!;
    }

    public async Task<ICollection<Tarjeta>> ListAsync()
    {
        var collection = await _context.Set<Tarjeta>().AsNoTracking().ToListAsync();
        return collection;
    }

    public async Task UpdateAsync(int id, Tarjeta entity)
    {
        var @object = await FindByIdAsync(id);
        @object.DescripcionTarjeta = entity.DescripcionTarjeta;
        @object.Estado = entity.Estado;
        await _context.SaveChangesAsync();
    }
}
