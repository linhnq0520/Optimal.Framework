using Microsoft.AspNetCore.Mvc;
using Optimal.Framework.Controller;
using Optimal.Framework.Data;

namespace TestPackage.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TestController:BaseController
    {
        private readonly IRepository<UserAccount> _userAccountRepository;
        public TestController(IRepository<UserAccount> userAccountRepository)
        {
            _userAccountRepository = userAccountRepository;
        }
        [HttpGet]
        public async Task<IActionResult> TestGetAll()
        {
            var result = await _userAccountRepository.GetAllAsync();
            return Ok(result);
        }
    }
}
