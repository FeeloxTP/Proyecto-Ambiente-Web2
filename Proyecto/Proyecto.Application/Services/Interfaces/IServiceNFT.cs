﻿using Proyecto.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Application.Services.Interfaces;

public interface IServiceNFT
{
    Task<ICollection<NFTDTO>> FindByDescriptionAsync(string description);
    Task<ICollection<NFTDTO>> ListAsync();
    Task<NFTDTO> FindByIdAsync(Guid id);
    Task<Guid> AddAsync(NFTDTO dto);
    Task DeleteAsync(Guid id);
    Task UpdateAsync(Guid id, NFTDTO dto);
}
