using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Optimal.Framework.Client;
using Optimal.Framework.Controller;

namespace Test2.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]

    public class Test2Controller : BaseController
    {
        private readonly QueueClient _queueClient;
        public Test2Controller()
        {
            var serviceInfo = new ServiceInfo
            {
                broker_hostname = "localhost",
                broker_port = 5672,
                broker_virtual_host = "/",
                broker_user_name = "guest",
                broker_user_password = "guest",
                broker_queue_name = "TEST2",
                broker_response_queue_name = "TES1",
                ssl_active = false
            };

            var queueClient = new QueueClient(serviceInfo);
            _queueClient = queueClient;
        }

        [HttpGet]
        public async Task<IActionResult> TestQueue()
        {
            await Task.CompletedTask;
            var result ="";
            _queueClient.MessageReceived += (sender, message) =>
            {
                Console.WriteLine($"Message received: {message}");
                result = message;
                // Xử lý tin nhắn tại đây
            };

            return Ok(result);
        }
    }
}
