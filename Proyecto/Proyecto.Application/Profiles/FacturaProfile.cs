using AutoMapper;
using Proyecto.Application.DTOs;
using Proyecto.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Application.Profiles;

public class FacturaProfile : Profile
{
    public FacturaProfile()
    {
        CreateMap<FacturaDTO, FacturaEncabezado>().ReverseMap();
        CreateMap<FacturaDetalleDTO, FacturaDetalle>().ReverseMap();

        CreateMap<FacturaDTO, FacturaEncabezado>()
             .ForMember(dest => dest.IdFactura, orig => orig.MapFrom(x => x.IdFactura))
             .ForMember(dest => dest.IdTarjeta, orig => orig.MapFrom(x => x.IdTarjeta))
             .ForMember(dest => dest.IdCliente, orig => orig.MapFrom(x => x.IdCliente))
             .ForMember(dest => dest.FacturaDetalle, orig => orig.MapFrom(x => x.ListFacturaDetalle)).ReverseMap();
    }
}
