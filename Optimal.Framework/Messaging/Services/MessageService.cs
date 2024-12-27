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
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public MessageService(
            IPublishEndpoint publishEndpoint,
            IBus bus,
            ISendEndpointProvider sendEndpointProvider
        )
        {
            _publishEndpoint = publishEndpoint;
            _bus = bus;
            _serviceInfo = Singleton<AppSettings>.Instance.Get<ServiceInfo>();
            _sendEndpointProvider = sendEndpointProvider;
        }

        public async Task PublishAsync<T>(T message)
            where T : class
        {
            await _publishEndpoint.Publish(message);
        }

        public async Task SendAsync<T>(T message, string queueName)
            where T : class
        {
            try
            {
                var endpointTest = await _sendEndpointProvider.GetSendEndpoint(
                    new Uri(
                        $"rabbitmq://{_serviceInfo.broker_hostname}/{_serviceInfo.WorkflowDirectExchange}?type=direct"
                    )
                );
                var endpoint = await _bus.GetSendEndpoint(
                    new Uri($"rabbitmq://{_serviceInfo.broker_hostname}/{queueName}?type=direct")
                );
                // await endpointTest.Send(message);
                await endpointTest.Send(
                    message,
                    context =>
                    {
                        context.SetRoutingKey(queueName);
                    }
                );
            }
            catch (Exception ex)
            {
                throw;
            }
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
