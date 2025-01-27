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
        List<ItemTaskDTO> GetDataFAke()
        {
            return new List<ItemTaskDTO> { new ItemTaskDTO
            {
                Id=1,
                Description="Prueba",
                status = Status.EnProceso,
                Title ="Test",
                StartDate = DateTime.Now.AddDays(-1).AddHours(-2)

            } };
        }
        public async Task<IActionResult> Index()
        {
            var model = new TaskDTO();
            try
            {
                string token = Request.Cookies["token"];
                var response = await _restClientHelper.GetRequestAsync<List<ItemTaskDTO>>("/Task", token);
                model.Items.AddRange(response);
                if (!model.Items.Any())
                {
                    
                }
                return View(model);
            }
            catch (Exception ex)
            {
                var dataFake = GetDataFAke();
                model.Items.AddRange(dataFake);
                TempData["MensajeError"] = ex.Message;
                return View(model);

            }
        }
        [HttpPost("task")]
        public async Task<JsonResult> Create(CreateTask data)
        {
            string token = Request.Cookies["token"];
            var response = await _restClientHelper.PostRequestAsync<bool>("/Task", token, data);

            return Json(new { succes = response });
        }
        [HttpPut("task")]
        public async Task<JsonResult> Update(UpdateTask data)
        {
            string token = Request.Cookies["token"];
            var response = await _restClientHelper.PutRequestAsync<bool>("/Task", token, data);
            return Json(new { succes = response });
        }
        [HttpGet("GetById/{id:long}")]
        public async Task<JsonResult> GetById(long id)
        {
            string token = Request.Cookies["token"];
            var response = await _restClientHelper.GetRequestAsync<ItemTaskDTO>($"/Task/{id}", token);
            return Json( response);
        }
        [HttpDelete]
        public async Task<JsonResult> Delete(long id)
        {
            string token = Request.Cookies["token"];
            var response = await _restClientHelper.DeleteRequestAsync($"/Task/{id}", token);
            return Json(response);
        }
    }
}
