using Commons;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
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

        public async Task<IActionResult> Index()
        {
            var model = new TaskDTO();
            try
            {
                string token = Request.Cookies["token"];
                var response = await _restClientHelper.GetRequestAsync<List<ItemTaskDTO>>("/Task", token);
                model.Items.AddRange(response);
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["MensajeError"] = ex.Message;
                return View(model);

            }
        }
        [HttpPost]
        public async Task<JsonResult> Create(CreateTask data)
        {
            string token = Request.Cookies["token"];
            var response = await _restClientHelper.PostRequestAsync<bool>("/Task", token, data);

            return Json(new { succes = response });
        }
        [HttpPut]
        public async Task<JsonResult> Update(ItemTaskDTO data)
        {
            string token = Request.Cookies["token"];
            var response = await _restClientHelper.PutRequestAsync<bool>("/Task", token, data);
            return Json(new { succes = response });
        }
        public async Task<JsonResult> GetById(long id)
        {
            string token = Request.Cookies["token"];
            var response = await _restClientHelper.DeleteRequestAsync($"/Task/{id}", token);
            return Json(new { succes = response });
        }
        [HttpDelete]
        public async Task<JsonResult> Delete(long id)
        {
            string token = Request.Cookies["token"];
            var response = await _restClientHelper.GetRequestAsync<ItemTaskDTO>("/Task", token);
            return Json(response);
        }
    }
}
