using Proyecto.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto.Infraestructure.Repository.Interfaces;

public interface IRepositoryFacturaDetalle
{
    Task<ICollection<FacturaDetalle>> ListAsync();

}
