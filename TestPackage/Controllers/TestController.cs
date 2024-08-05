using Microsoft.AspNetCore.Mvc;
using Optimal.Framework.Client;
using Optimal.Framework.Controller;
using Optimal.Framework.Data;
using Optimal.Framework.Infrastructure;

namespace TestPackage.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TestController : BaseController
    {
        private readonly QueueClient _queueClient;
        public TestController()
        {
            var serviceInfo = new ServiceInfo
            {
                broker_hostname = "localhost",
                broker_port = 5672,
                broker_virtual_host = "/",
                broker_user_name = "guest",
                broker_user_password = "guest",
                broker_queue_name = "TEST1",
                broker_response_queue_name = "TES2",
                ssl_active = false
            };

            var queueClient = new QueueClient(serviceInfo);
            _queueClient = queueClient;
        }

        private readonly IRepository<UserAccount> _userAccountRepository = EngineContext.Current.Resolve<IRepository<UserAccount>>();
        //private readonly IRepository<UserAccount> _userAccountRepository; //= EngineContext.Current.Resolve<IRepository<UserAccount>>();
        //public TestController(IRepository<UserAccount> userAccountRepository)
        //{
        //    _userAccountRepository = userAccountRepository;
        //}

        [HttpGet]
        public async Task<IActionResult> TestGetAll()
        {
            var result = await _userAccountRepository.GetAllAsync();
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> TestQueue()
        {
            await Task.CompletedTask;
            _queueClient.SendMessage("TES2", "message from test 1");

            return Ok("CONNECTED");
        }
    }
}
