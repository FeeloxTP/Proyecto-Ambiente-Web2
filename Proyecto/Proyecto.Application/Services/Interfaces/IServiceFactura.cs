using Proyecto.Application.DTOs;
using Proyecto.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Application.Services.Interfaces;

public interface IServiceFactura
{
    Task<int> AddAsync(FacturaDTO dto);
    Task<int> GetNextReceiptNumber();
    Task<ICollection<FacturaDTO>> ListAsync();
    Task UpdateAsync(int id, FacturaDTO dto);
    Task<FacturaDTO> FindByIdAsync(int id);
    Task<ICollection<FacturaDTO>> FindByClientNameAsync(string name);
    Task<ICollection<FacturaDTO>> FindByVentasByFechasAsync(DateTime fechaInicial, DateTime fechaFinal);
    Task<ICollection<FacturaDTO>> FindByReporteXFechas(DateTime fechaInicial, DateTime fechaFinal);


}
