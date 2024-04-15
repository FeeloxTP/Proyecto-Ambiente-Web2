using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Proyecto.Application.Services.Interfaces;
using Proyecto.Infraestructure.Repository.Interfaces;

namespace Proyecto.Web.Controllers
{
    public class GraficoController : Controller
    {
        private readonly IServiceNFT _serviceNFT;
        private readonly IServiceReportes _serviceReporte;
        private readonly IServiceCliente _serviceCliente;
        private readonly IServiceFactura _serviceFactura;
        private readonly IRepositoryFacturaDetalle _repositoryDetalle;

        public GraficoController(IServiceNFT serviceProducto,
                                 IServiceReportes serviceReporte,
                                 IServiceCliente serviceCliente,
                                 IServiceFactura serviceFactura,
                                 IRepositoryFacturaDetalle repositoryFacturaDetalle)
        {
            _serviceNFT = serviceProducto;
            _serviceReporte = serviceReporte;
            _serviceCliente = serviceCliente;
            _serviceFactura = serviceFactura;
            _repositoryDetalle = repositoryFacturaDetalle;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GraficoProductos()
        {
            return View();
        }

        public IActionResult GraficoVentas()
        {
            return View();
        }

        public async Task<IActionResult> VentasXCliente(DateTime fechaInicial, DateTime fechaFinal)
        {
            string etiquetas = "";
            string precios = "";
            decimal total = 0M;

            if (string.IsNullOrEmpty(fechaInicial.ToString()))
            {
                ViewBag.Message = $"Fecha Inial Requerida";
                return View("GraficoVentas");
            }

            if (string.IsNullOrEmpty(fechaFinal.ToString()))
            {
                ViewBag.Message = $"fecha Final Requerida";
                return View("GraficoVentas");
            }

            var collectionFacturaEncabezado = await _serviceFactura.FindByReporteXFechas(fechaInicial, fechaFinal);
            var collectionFacturaDetalle = await _repositoryDetalle.ListAsync();
            var collectionNFT = await _serviceNFT.ListAsync();
            var NFTDic = collectionNFT.ToDictionary(c => c.IdNft, c => c.Nombre);
            var collectionClientes = await _serviceCliente.ListAsync();
            var clienteDict = collectionClientes.ToDictionary(c => c.IdCliente, c => $"{c.Nombre} {c.Apellido1} {c.Apellido2}");


            //if (cliente == null)
            //{
            //    ViewBag.Message = $"Cliente Id = {idCliente} No existe";
            //    return View("GraficoVentas");
            //}

            //var lista = await _serviceReporte.BillsByClientIdAsync(idCliente);

            //if (lista.Count == 0)
            //{
            //    ViewBag.Message = $"No hay reportes de ventas para Id = {idCliente}";
            //    return View("GraficoVentas");
            //}
            var clientes = "";
            var cantidad = "";
            
            foreach (var item in collectionFacturaEncabezado)
            {
                // concatenade 
                foreach(var c in item.ListFacturaDetalle)
                {
                    etiquetas += "";
                    etiquetas +=  NFTDic.ContainsKey(c.IdNFT) ? @NFTDic[c.IdNFT] : "";
                    etiquetas += ",";
                    cantidad += c.Cantidad.ToString("##") + ",";
                }
                etiquetas += "" + item.ListFacturaDetalle.Any()  + ",";
                // Has List Any data?
                var sigma = item.ListFacturaDetalle.Any() ? item.ListFacturaDetalle.Sum(p => (p.Cantidad * p.Precio)) : 0;
                total += item.Total;
                precios += sigma.ToString("##") + ",";
                clientes += clienteDict.ContainsKey(item.IdCliente) ? @clienteDict[item.IdCliente] : "";
                clientes += ",";
            }
            etiquetas = etiquetas.Substring(0, etiquetas.Length - 1); // ultima coma
            precios = precios.Substring(0, precios.Length - 1);
            cantidad = precios.Substring(0, cantidad.Length - 1);

            ViewBag.GraphTitle = $"Cliente  {clientes} -  Total de Ventas {total.ToString("###,###.00")} ";
            ViewBag.Etiquetas = etiquetas;
            ViewBag.Valores = precios;
            ViewBag.clientes = clientes;
            ViewBag.cantidadNFT = cantidad;
            return View("GraficoVentas");
        }


        public async Task<IActionResult> InventarioProducto()
        {

            var lista = await _serviceNFT.ListAsync();
            string etiquetas = "";
            string precios = "";
            foreach (var item in lista)
            {
                // Hay que concatenarle comillas 'arduino','xxx', ...
                etiquetas += "" + item.Nombre + ",";
                precios += item.Inventario.ToString() + ",";
            }
            etiquetas = etiquetas.Substring(0, etiquetas.Length - 1); // ultima coma
            precios = precios.Substring(0, precios.Length - 1);


            // toma la lista y le agrega separa por comas (,)

            ViewBag.Etiquetas = etiquetas;
            ViewBag.Valores = precios;
            return View("GraficoProductos");
        }
    }
}
