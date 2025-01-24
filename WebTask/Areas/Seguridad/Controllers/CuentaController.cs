using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Threading.Tasks;
using ConautoDT.Web.Servicios;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Commons;

namespace WebTask.Web.Areas.Seguridad.Controllers
{
    [Area("Seguridad")]
    public class CuentaController : Controller
    {
        private readonly RestClientHelper _restClientHelper;
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _memoryCache;

        public CuentaController(IConfiguration configuration, IMemoryCache memoryCache, RestClientHelper restClientHelper)
        {
            _configuration = configuration;
            _restClientHelper = restClientHelper;
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public async Task<ActionResult> IniciarSesion()
        {
            var authenticated = false; //TODO: Implementar logica de validar token
            if (authenticated)
            {
                return Redirect("../../../");
            }

            return View();
        }

        [HttpPost]
        public async Task<ActionResult> IniciarSesion(LoginDTO model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await _restClientHelper.PostRequestAsync<LoggedUser>("/auth/login", null, model);

                    if (response != null)
                    {
                        // Crear una cookie para almacenar el token
                        Response.Cookies.Append("token", response.Auth.Access_Token);
                        Response.Cookies.Append("username", model.Username);
                        Response.Cookies.Append("rol", response.Role);
                        Response.Cookies.Append("env", response.Env);
                        Response.Cookies.Append("expiracion", response.Expiracion.ToString());

                        // Recuperar la URL almacenada
                        string returnUrl = HttpContext.Request.Cookies["returnUrl"];
                        // Limpiar la cookie
                        HttpContext.Response.Cookies.Delete("returnUrl");

                        // Redirecciona al home
                        return Redirect(string.IsNullOrEmpty(returnUrl) ? "../../../" : returnUrl);
                    }
                    else
                    {
                        TempData["MensajeError"] = "Error al iniciar sesión. Por favor, intente nuevamente.";
                    }
                }
                else
                {
                    TempData["MensajeError"] = "Rellene todos los campos correctamente.";
                }
            }
            catch (HttpRequestException e)
            {
                TempData["MensajeError"] = $"Error de conexión con el API: {e.Message}";
            }
            catch (JsonException e)
            {
                TempData["MensajeError"] = $"Error al procesar la respuesta del API: {e.Message}";
            }
            catch (Exception e)
            {
                TempData["MensajeError"] = $"Error inesperado: {e.Message}";
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult CerrarSession()
        {
            Response.Cookies.Delete("token");
            Response.Cookies.Delete("expiracion");
            Response.Cookies.Delete("rol");
            Response.Cookies.Delete("username");
            return RedirectToAction(nameof(IniciarSesion));
        }
    }
}