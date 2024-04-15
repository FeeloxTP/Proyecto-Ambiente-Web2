using Microsoft.EntityFrameworkCore;
using Proyecto.Infraestructure.Data;
using Proyecto.Infraestructure.Models;
using Proyecto.Infraestructure.Repository.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Infraestructure.Repository.Implementations;

public class RepositoryFacturaDetalle : IRepositoryFacturaDetalle
{

    private readonly ProyectoContext _context;

    public RepositoryFacturaDetalle(ProyectoContext context)
    {
        _context = context;

    }
    public async Task<ICollection<FacturaDetalle>> ListAsync()
    {
        var collection = await _context.Set<FacturaDetalle>().AsNoTracking().ToListAsync();
        return collection;
    }
}
