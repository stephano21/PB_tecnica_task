using Commons;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Diagnostics;
using WebTask.Models;
using WebTask.Web.Decorators;
using WebTask.Web.Servicios;

namespace WebTask.Controllers
{
    [TokenRequired]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RestClientHelper _restClientHelper;
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;

            _restClientHelper = new RestClientHelper(configuration);
        }

        public async Task<IActionResult> IndexAsync()
        {
            string token = Request.Cookies["token"];
            var model = new TaskDTO();
            var response = await _restClientHelper.GetRequestAsync<List<ItemTaskDTO>>("/Task", token);
            model.Items.AddRange(response);
            return View(model);
        }
    }
}
