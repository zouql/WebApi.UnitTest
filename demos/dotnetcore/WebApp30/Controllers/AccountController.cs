namespace WebApp30.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using WebApp30.Accounts;

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

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> CreateOrUpdateAsync(string userName, string passWord)
        {
            var result = await service.CreateOrUpdateAsync(userName, passWord, new string[0]);

            return result.Succeeded ? (IActionResult)Ok() : BadRequest();
        }
    }
}