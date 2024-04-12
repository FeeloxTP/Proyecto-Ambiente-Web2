using Microsoft.AspNetCore.Mvc;

namespace Proyecto.Web.Controllers;

public class ErrorController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [Route("error/{code:int}")]
    public IActionResult Error(int code)
    {
        // handle different codes or just return the default error view
        ViewBag.Code = code;

        switch (code)
        {
            case 400:
                ViewBag.Message = "Servidor no pudo interpretar la solicitud dada una sintaxis inválida";
                break;
            case 401:
                ViewBag.Message = "Es necesario autenticar para obtener la respuesta solicitada. Esta es similar a 403, pero en este caso, la autenticación es posible";
                break;
            case 402:
                ViewBag.Message = "Este código de respuesta está reservado para futuros usos";
                break;
            case 403:
                ViewBag.Message = "El cliente no posee los permisos necesarios para cierto contenido, por lo que el servidor está rechazando otorgar una respuesta apropiada.";
                break;
            case 404:
                ViewBag.Message = "El servidor no pudo encontrar el contenido solicitado";
                break;
            case 405:
                ViewBag.Message = "El método solicitado es conocido por el servidor pero ha sido deshabilitado y no puede ser utilizado.";
                break;
            case 408:
                ViewBag.Message = "Respuesta es enviada en una conexión inactiva en algunos servidores, incluso sin alguna petición previa por el cliente. Significa que el servidor quiere desconectar esta conexión sin usar. ";
                break;

            case 500:
                ViewBag.Message = "El servidor ha encontrado una situación que no sabe cómo manejarla. ";
                break;

            case 501:
                ViewBag.Message = "El método solicitado no está soportado por el servidor y no puede ser manejado. ";
                break;

            case 502:
                ViewBag.Message = "Esta respuesta de error significa que el servidor, mientras trabaja como una puerta de enlace para obtener una respuesta necesaria para manejar la petición, obtuvo una respuesta inválida ";
                break;

            case 504:
                ViewBag.Message = "El servidor no está listo para manejar la petición. Causas comunes puede ser que el servidor está caído por mantenimiento o está sobrecargado";
                break;
        }

        return View("ErrorDetail");
    }
}