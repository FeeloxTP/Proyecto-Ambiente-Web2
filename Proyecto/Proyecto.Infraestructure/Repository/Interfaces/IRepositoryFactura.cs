using Proyecto.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Infraestructure.Repository.Interfaces;

public interface IRepositoryFactura
{
    Task<int> AddAsync(FacturaEncabezado entity);

    Task<int> GetNextReceiptNumber();
    Task<FacturaEncabezado> FindByIdAsync(int id);

    Task<ICollection<FacturaEncabezado>> BillsByClientIdAsync(Guid id);

    Task<ICollection<FacturaEncabezado>> ListAsync();
    Task UpdateAsync(int id, FacturaEncabezado entity);
    Task<ICollection<FacturaEncabezado>> FindByClientNameAsync(string name);
}
