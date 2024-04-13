using Proyecto.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Application.Services.Interfaces;

public interface IServiceReportes
{
    Task<byte[]> ProductReport();

    Task<byte[]> ClienteReportByNFT(string description);

    Task<ICollection<FacturaDTO>> BillsByClientIdAsync(Guid id);
}
