using Microsoft.AspNetCore.Mvc;
using Optimal.Framework.Controller;
using Optimal.Framework.Data;
using Optimal.Framework.Infrastructure;

namespace TestPackage.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TestController:BaseController
    {

        private readonly IRepository<UserAccount> _userAccountRepository; //= EngineContext.Current.Resolve<IRepository<UserAccount>>();
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
