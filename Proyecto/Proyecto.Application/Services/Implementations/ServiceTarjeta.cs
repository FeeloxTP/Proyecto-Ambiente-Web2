using AutoMapper;
using Proyecto.Application.DTOs;
using Proyecto.Application.Services.Interfaces;
using Proyecto.Infraestructure.Models;
using Proyecto.Infraestructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Application.Services.Implementations;

public class ServiceTarjeta : IServicesTarjeta
{
    private readonly IRepositoryTarjeta _repository;
    private readonly IMapper _mapper;

    public ServiceTarjeta(IRepositoryTarjeta repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<int> AddAsync(TarjetaDTO dto)
    {
        // Map BodegaDTO to Bodega
        var objectMapped = _mapper.Map<Tarjeta>(dto);

        // Return
        return await _repository.AddAsync(objectMapped);
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<ICollection<TarjetaDTO>> FindByDescriptionAsync(string DescripcionTarjeta)
    {
        var list = await _repository.FindByDescriptionAsync(DescripcionTarjeta);
        var collection = _mapper.Map<ICollection<TarjetaDTO>>(list);
        return collection;
    }

    public async Task<TarjetaDTO> FindByIdAsync(int id)
    {
        var @object = await _repository.FindByIdAsync(id);
        var objectMapped = _mapper.Map<TarjetaDTO>(@object);
        return objectMapped;
    }

    public async Task<ICollection<TarjetaDTO>> ListAsync()
    {
        // Get data from Repository
        var list = await _repository.ListAsync();
        // Map List<Bodega> to ICollection<BodegaDTO>
        var collection = _mapper.Map<ICollection<TarjetaDTO>>(list);
        // Return Data
        return collection;
    }

    public async Task UpdateAsync(int id, TarjetaDTO dto)
    {
        var objectMapped = _mapper.Map<Tarjeta>(dto);
        await _repository.UpdateAsync(id, objectMapped);
    }
}
