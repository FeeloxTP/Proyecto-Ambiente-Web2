using AutoMapper;
using Proyecto.Application.DTOs;
using Proyecto.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Application.Profiles;

public class NFTProfile : Profile
{
    public NFTProfile()
    {
        // Means: Source   , Destination and Reverse :)  
        CreateMap<NFTDTO, ActivoNft>().ReverseMap();
    }
}
