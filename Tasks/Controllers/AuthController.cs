using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tasks.Manager;
using Commons;

namespace Tasks.Controllers
{
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly CustomUserIdentityManager _userManager;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AuthController(CustomUserIdentityManager ApplicationUserManager, 
            IConfiguration IConfiguration,
            IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            _userManager = ApplicationUserManager;
        }
        [HttpPost("login")]
        public async Task<ActionResult> LoginAsync([FromBody]LoginDTO data) => Ok(await _userManager.LoginUserAsync(data));
        [HttpPost("register")]
        public async Task<ActionResult> RegisterAsync([FromBody] RegisterDTO UserData) => Ok(await _userManager.RegisterUserAsync(UserData));
       
        [HttpGet("Profile")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Profile() => Ok(await _userManager.GetProfile(httpContextAccessor.HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == "Username").Value));
        [HttpGet("/")]
        [AllowAnonymous]
        public ActionResult RedirectAPI()
        {
            //return  RedirectToAction("Get", "Root");
            return Redirect("swagger/");
        }
    }
}
