using Proyecto.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Application.Services.Interfaces;

public interface IServiceReportes
{
    Task<byte[]> ClientesReportPDF();
    Task<byte[]> NFTsReportPDF();
    Task<byte[]> VentaReporteByFechas(DateTime fechaInicial, DateTime fechaFinal);
}
