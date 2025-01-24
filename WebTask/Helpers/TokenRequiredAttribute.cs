using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using RestSharp.Serializers.Utf8Json;
using Microsoft.Extensions.Configuration;
using System;

namespace WebTask.Web.Decorators
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class TokenRequiredAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Verifica si la solicitud tiene un token
            if (!context.HttpContext.Request.Cookies.ContainsKey("token"))
            {
                // El token no está presente, almacena la URL actual y redirige a la página de inicio de sesión
                string returnUrl = context.HttpContext.Request.Path + context.HttpContext.Request.QueryString;
                context.HttpContext.Response.Cookies.Append("returnUrl", returnUrl);
                context.Result = new RedirectToActionResult("IniciarSesion", "Cuenta", new { area = "Seguridad" });
                return;
            }

            if (context.HttpContext.Request.Cookies.ContainsKey("token"))
            {
                var token = context.HttpContext.Request.Cookies["token"];
                var apiEndpoint = context.HttpContext.RequestServices.GetService<IConfiguration>()?["APIClient"];

                // Configuración del cliente RestSharp
                var options = new RestClientOptions(apiEndpoint)
                {
                    ThrowOnAnyError = true, // Esto ya no existe en RestSharp v107+
                    MaxTimeout = 120000 // Timeout en milisegundos
                };

                var apiClient = new RestClient(options);

                var request = new RestRequest("/cuentas/ValidateToken", Method.Get);
                request.AddParameter("token", token);

                try
                {
                    // Ejecuta la solicitud de manera asíncrona
                    var response = apiClient.ExecuteAsync(request).Result;

                    if (true)//TODO:No he creado validacion de token
                    {
                        // Token válido
                        return;
                    }

                  

                    // Almacena la URL actual y redirige a la página de inicio de sesión
                    string returnUrl = context.HttpContext.Request.Path + context.HttpContext.Request.QueryString;
                    context.HttpContext.Response.Cookies.Append("returnUrl", returnUrl);
                    context.Result = new RedirectToActionResult("IniciarSesion", "Cuenta", new { area = "Seguridad" });
                    return;
                }
                catch (Exception ex)
                {
                    return;
                    // Manejar el error...
                    //throw new Exception("Error al validar el token", ex);
                }
            }
        }
    }
}