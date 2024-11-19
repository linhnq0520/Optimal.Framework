using MassTransit;
using Optimal.Framework.Client;
using Optimal.Framework.Infrastructure;
using Optimal.Framework.Messaging.Contracts;

namespace Optimal.Framework.Messaging.Services
{
    public interface IMessageService
    {
        Task PublishAsync<T>(T message)
            where T : class;
        Task SendAsync<T>(T message, string queueName)
            where T : class;
    }

    public class MessageService : IMessageService
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IBus _bus;
        private readonly ServiceInfo _serviceInfo;

        public MessageService(IPublishEndpoint publishEndpoint, IBus bus)
        {
            _publishEndpoint = publishEndpoint;
            _bus = bus;
            _serviceInfo = Singleton<AppSettings>.Instance.Get<ServiceInfo>();
        }

        public async Task PublishAsync<T>(T message)
            where T : class
        {
            await _publishEndpoint.Publish(message);
        }

        public async Task SendAsync<T>(T message, string queueName)
            where T : class
        {
            var endpoint = await _bus.GetSendEndpoint(
                new Uri($"rabbitmq://{_serviceInfo.broker_hostname}/{queueName}?type=direct")
            );
            await endpoint.Send(message);
        }
    }

    public class WorkflowMessageConsumer : IConsumer<WorkflowMessage>
    {
        public Task Consume(ConsumeContext<WorkflowMessage> context)
        {
            Console.WriteLine(
                $"Received message: {context.Message.Request} at {context.Message.Timestamp}"
            );
            return Task.CompletedTask;
        }
    }
}
