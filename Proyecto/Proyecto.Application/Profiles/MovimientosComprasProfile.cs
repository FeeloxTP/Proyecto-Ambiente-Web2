using AutoMapper;
using Proyecto.Application.DTOs;
using Proyecto.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Application.Profiles
{
    public class MovimientosComprasProfile : Profile
    {
        public MovimientosComprasProfile()
        {
            CreateMap<MovimientosComprasDTO, MovimientosCompras>().ReverseMap();

            CreateMap<MovimientosComprasDTO, MovimientosCompras>()
            .ForMember(dest => dest.Id, orig => orig.MapFrom(x => x.Id))
            .ForMember(dest => dest.Fecha, orig => orig.MapFrom(x => x.Fecha))
            .ForMember(dest => dest.IdNft, orig => orig.MapFrom(x => x.IdNft))
            .ForMember(dest => dest.ClienteId, orig => orig.MapFrom(x => x.ClienteId))
            .ForMember(dest => dest.Estado, orig => orig.MapFrom(x => x.Estado));
        }
    }
}