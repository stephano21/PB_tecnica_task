using Commons;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tasks.Manager;

namespace Tasks.Controllers
{

    [ApiController]
    [Route("api")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TaskController : ControllerBase
    {
        private readonly TaskManager _manager;
        public TaskController(ApplicationDbContext Context, CustomUserIdentityManager userManager, IHttpContextAccessor httpContextAccessor)
        {
            string ip = httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();
            string userName = Task.Run(async () => await userManager
            .FindByNameAsync(httpContextAccessor.HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == "Username").Value)).Result.UserName;
            string id = Task.Run(async () => await userManager
            .FindByNameAsync(httpContextAccessor.HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == "Username").Value)).Result.Id;
            _manager = new TaskManager(Context, userName, ip, id);
        }
        [HttpGet("Task")]
        public async Task<ActionResult> IndexAsync() => Ok(await _manager.Get());
        [HttpGet("Task/{id:long}")]
        public async Task<ActionResult> GetItem(long id) => Ok(await _manager.GetById(id));
        [HttpPost("Task")]
        public async Task<ActionResult> SaveAsync([FromBody] CreateTask data) => Ok(await _manager.Save(data));
        [HttpPut("Task")]
        public async Task<ActionResult> EditAsync(UpdateTask data) => Ok(await _manager.Edit(data));
        [HttpDelete("Task/{Id:long}")]
        public async Task<ActionResult> DeleteAsync(long Id) => Ok(await _manager.Delete(Id));
        [HttpPost("Task/start/{Id:long}")]
        public async Task<ActionResult> StartTask(long Id) => Ok(await _manager.HandleStatus(Id));
        [HttpPost("Task/end/{Id:long}")]
        public async Task<ActionResult> EndTask(long Id) => Ok(await _manager.HandleStatus(Id,false));

    }

}
