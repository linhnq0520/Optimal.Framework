using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Optimal.Framework.Client;
using Optimal.Framework.Controller;
using Optimal.Framework.Messaging.Contracts;
using Optimal.Framework.Messaging.Services;

namespace Test2.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class Test2Controller : BaseController
    {
        private readonly IMessageService _messageService;

        public Test2Controller(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet]
        public async Task<IActionResult> TestQueue()
        {
            await Task.CompletedTask;
            var mesage = new WorkflowMessage();
            mesage.Request = "hello linh ne";
            await _messageService.SendAsync(mesage, "notification-service-queue");

            return Ok("sucess");
        }

        [HttpGet]
        public async Task<IActionResult> TestExchange()
        {
            await Task.CompletedTask;
            var mesage = new WorkflowMessage();
            mesage.Request = "hello linh ne";
            await _messageService.SendAsync(mesage, "exchange_notification-service-queue");

            return Ok("sucess");
        }

        [HttpGet]
        public async Task<IActionResult> TestExchange1()
        {
            await Task.CompletedTask;
            var mesage = new WorkflowMessage();
            mesage.Request = "hello linh ne";
            await _messageService.SendAsync(mesage, "notification-service-queue");

            return Ok("sucess");
        }

        [HttpGet]
        public async Task<IActionResult> TestPublish()
        {
            await Task.CompletedTask;
            var mesage = new WorkflowMessage();
            mesage.Request = "hello linh ne";
            await _messageService.PublishAsync(mesage);

            return Ok("sucess");
        }
    }
}
