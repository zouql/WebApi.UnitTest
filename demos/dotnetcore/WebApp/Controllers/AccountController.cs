namespace WebApp.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using WebApp.Accounts;

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountService service;

        public AccountController(AccountService service)
        {
            this.service = service;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> SignInForIdentityAsync(string userName, string passWord)
        {
            var result = await service.SignInForIdentityAsync(userName, passWord) ? (IActionResult)Ok() : BadRequest("用户名或密码错误！");

            return result;
        }
    }
}