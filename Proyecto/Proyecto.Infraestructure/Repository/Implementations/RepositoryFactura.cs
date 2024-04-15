using Proyecto.Infraestructure.Data;
using Proyecto.Infraestructure.Models;
using Proyecto.Infraestructure.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Proyecto.Infraestructure.Repository.Implementations;

public class RepositoryFactura : IRepositoryFactura
{
    private readonly ProyectoContext _context;

    public RepositoryFactura(ProyectoContext context)
    {
        _context = context;

    }
    public async Task<int> AddAsync(FacturaEncabezado entity)
    {

        try
        {
            // Get No Receipt
            entity.IdFactura = GetNoReceipt();
            // Reenumerate
            entity.FacturaDetalle.ToList().ForEach(p => p.IdFactura = entity.IdFactura);
            // Begin Transaction
            await _context.Database.BeginTransactionAsync();
            await _context.Set<FacturaEncabezado>().AddAsync(entity);
            //se agregar la fecha del dia
            entity.FechaFacturacion = DateTime.Now;
            //entity.EstadoFactura = 1;

            entity.Total = entity.FacturaDetalle.Sum(p => p.Cantidad * p.Precio);

            // Withdraw from inventory
            foreach (var item in entity.FacturaDetalle)
            {
                // find the product
                var product = await _context.Set<ActivoNft>().FindAsync(item.IdNft);
                // update stock
                product!.Inventario = product.Inventario - item.Cantidad;
                // update entity product
                _context.Set<ActivoNft>().Update(product);
            }

            await _context.SaveChangesAsync();
            // Commit
            await _context.Database.CommitTransactionAsync();

            return entity.IdFactura;
        }
        catch (Exception ex)
        {
            Exception exception = ex;
            // Rollback 
            await _context.Database.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<ICollection<FacturaEncabezado>> ListAsync()
    {
        var collection = await _context.Set<FacturaEncabezado>().AsNoTracking().ToListAsync();
        return collection;
    }

    /// <summary>
    /// Get current NoReceipt without increment
    /// </summary>
    /// <returns></returns>
    public async Task<int> GetNextReceiptNumber()
    {

        int current = 0;

        string sql = string.Format("SELECT current_value FROM sys.sequences WHERE name = 'ReceiptNumber'");

        System.Data.DataTable dataTable = new System.Data.DataTable();

        System.Data.Common.DbConnection connection = _context.Database.GetDbConnection();
        System.Data.Common.DbProviderFactory dbFactory = System.Data.Common.DbProviderFactories.GetFactory(connection!)!;
        using (var cmd = dbFactory!.CreateCommand())
        {
            cmd!.Connection = connection;
            cmd.CommandText = sql;
            using (System.Data.Common.DbDataAdapter adapter = dbFactory.CreateDataAdapter()!)
            {
                adapter.SelectCommand = cmd;
                adapter.Fill(dataTable);
            }
        }


        current = Convert.ToInt32(dataTable.Rows[0][0].ToString());
        return await Task.FromResult(current);

    }




    /// <summary>
    /// Get sequence in order to assign Receipt number.   
    /// Automaticaly INCREMENT ++
    /// http://technet.microsoft.com/es-es/library/ff878091.aspx
    /// CREATE SEQUENCE  ReceiptNumber  START WITH 1 INCREMENT BY 1 ;
    /// </summary>
    /// <returns>Num. de factura</returns>
    private int GetNoReceipt()
    {
        int siguiente = 0;

        string sql = string.Format("SELECT NEXT VALUE FOR ReceiptNumber");

        System.Data.DataTable dataTable = new System.Data.DataTable();

        System.Data.Common.DbConnection connection = _context.Database.GetDbConnection();
        System.Data.Common.DbProviderFactory dbFactory = System.Data.Common.DbProviderFactories.GetFactory(connection!)!;
        using (var cmd = dbFactory!.CreateCommand())
        {
            cmd!.Connection = connection;
            cmd.CommandText = sql;
            using (System.Data.Common.DbDataAdapter adapter = dbFactory.CreateDataAdapter()!)
            {
                adapter.SelectCommand = cmd;
                adapter.Fill(dataTable);
            }
        }


        siguiente = Convert.ToInt32(dataTable.Rows[0][0].ToString());
        return siguiente;

    }

    public async Task<ICollection<FacturaEncabezado>> BillsByClientIdAsync(Guid id)
    {
        var response = await _context.Set<FacturaEncabezado>()
                       .Include(p => p.FacturaDetalle)
                       .Where(p => p.IdCliente == id).ToListAsync();

        return response;

    }

    public async Task<FacturaEncabezado> FindByIdAsync(int id)
    {

        var response = await _context.Set<FacturaEncabezado>()
                    .Include(detalle => detalle.FacturaDetalle)
                    .ThenInclude(detalle => detalle.IdNftNavigation)
                    .Include(cliente => cliente.IdClienteNavigation)
                    .Where(p => p.IdFactura == id).FirstOrDefaultAsync();

        return response!;
    }
    public async Task<ICollection<FacturaEncabezado>> FindByReporteXFechas(DateTime fechaInicial, DateTime fechaFinal)
    {

        var response = await _context.Set<FacturaEncabezado>()
                    .Include(detalle => detalle.FacturaDetalle)
                    .Where(p => p.FechaFacturacion >= fechaInicial && p.FechaFacturacion <= fechaFinal).ToListAsync();

        return response!;
    }

    public async Task UpdateAsync(int id, FacturaEncabezado entity)
    {
        var @object = await FindByIdAsync(id);
        @object.EstadoFactura = entity.EstadoFactura;

        await _context.SaveChangesAsync();
    }

    public async Task<ICollection<FacturaEncabezado>> FindByClientNameAsync(string name)
    {
        name = name.Replace(' ', '%');
        name = "%" + name + "%";
        FormattableString sql = $@"SELECT fe.IdFactura, fe.IdTarjeta, fe.IdCliente, fe.FechaFacturacion, fe.EstadoFactura, fe.TajetaNumero
                                   FROM FacturaEncabezado fe 
                                   INNER JOIN Cliente c ON fe.IdCliente = c.IdCliente
                                   WHERE c.Nombre + c.Apellido1 + c.Apellido2 LIKE {name};";

        var collection = await _context.FacturaEncabezado.FromSql(sql).AsNoTracking().ToListAsync();
        return collection;
    }

    public async Task<ICollection<FacturaEncabezado>> FindByVentasByFechasAsync(DateTime fechaInicial, DateTime fechaFinal)
    {
        //name = name.Replace(' ', '%');
        //name = "%" + name + "%";
        FormattableString sql = $@"SELECT Fe.IdFactura, Fe.IdTarjeta,Fe.IdCliente,
                                   Fe.FechaFacturacion, Fe.EstadoFactura,Fe.TajetaNumero, Fe.Total
                                   FROM FacturaEncabezado Fe
                                   INNER JOIN Cliente c ON Fe.IdCliente = c.IdCliente
                                   INNER JOIN FacturaDetalle fd ON Fe.IdFactura = fd.IdFactura
                                   WHERE Fe.FechaFacturacion BETWEEN {fechaInicial} AND {fechaFinal}
                                   GROUP BY Fe.IdCliente, Fe.IdFactura, Fe.FechaFacturacion, Fe.IdTarjeta, Fe.EstadoFactura,Fe.TajetaNumero,Fe.Total;";

        var collection = await _context.FacturaEncabezado.FromSql(sql).AsNoTracking().ToListAsync();
        return collection;
    }
}
