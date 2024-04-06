using AutoMapper;
using Proyecto.Application.DTOs;
using Proyecto.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Application.Profiles;

public class UsuarioProfile : Profile
{
    public UsuarioProfile()
    {
        // Means: Source   , Destination and Reverse :)  
        CreateMap<UsuarioDTO, Usuario>();

        CreateMap<Usuario, UsuarioDTO>()
            .ForMember(dest => dest.Login, orig => orig.MapFrom(x => x.Login))
            .ForMember(dest => dest.Nombre, orig => orig.MapFrom(x => x.Nombre))
            .ForMember(dest => dest.Apellidos, orig => orig.MapFrom(x => x.Apellidos))
            .ForMember(dest => dest.Estado, orig => orig.MapFrom(x => x.Estado))
            .ForMember(dest => dest.IdPerfil, orig => orig.MapFrom(x => x.IdPerfil))
            .ForMember(dest => dest.DescripcionPerfil, orig => orig.MapFrom(x => x.IdPerfilNavigation.DescripcionPerfil));
    }
}
