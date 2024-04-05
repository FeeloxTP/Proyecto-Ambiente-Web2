using Microsoft.AspNetCore.Mvc;
using Proyecto.Application.Services.Interfaces;

namespace Proyecto.Web.Controllers
{
    public class FacturaController : Controller
    {
        private readonly IServiceNFT _serviceNFT;
        private readonly IServicesTarjeta _serviceTarjeta;
        private readonly IServiceFactura _serviceFactura;
        //private readonly IServiceImpuesto _serviceImpuesto;
        private readonly IServiceCliente _serviceCliente;

        public IActionResult Index()
        {
            return View();
        }
    }
}
