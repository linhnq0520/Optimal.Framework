using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Optimal.Framework.Client;
using Optimal.Framework.Infrastructure;
using Optimal.Framework.Messaging.Contracts;
using Optimal.Framework.Messaging.Services;

namespace Optimal.Framework.Messaging.Configuration
{
    public static class MessagingStartup
    {
        public static void UseMassTransitRabbitMq(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            var serviceInfo =
                Singleton<AppSettings>.Instance.Get<ServiceInfo>()
                ?? throw new Exception("ServiceInfo is not configured");
            services.AddMassTransit(x =>
            {
                x.AddConsumer<WorkflowMessageConsumer>().ExcludeFromConfigureEndpoints();

                x.UsingRabbitMq(
                    (context, cfg) =>
                    {
                        cfg.Host(
                            "rabbitmq://localhost",
                            h =>
                            {
                                h.Username("guest");
                                h.Password("guest");
                            }
                        );

                        // Disable default topology
                        cfg.ConfigurePublish(p =>
                        {
                            p.UseExecute(context =>
                            {
                                // Prevent auto-creation of exchanges
                                context.SetRoutingKey(context.DestinationAddress.AbsoluteUri);
                            });
                        });
                        cfg.PublishTopology.BrokerTopologyOptions =
                            PublishBrokerTopologyOptions.FlattenHierarchy;

                        cfg.Publish<WorkflowMessage>(e =>
                        {
                            e.BindAlternateExchangeQueue(serviceInfo.WorkflowDirectExchange);
                        });

                        // // Tắt việc tạo exchange tự động cho publish
                        // cfg.Publish<WorkflowMessage>(e =>
                        // {
                        //     e.GetBrokerTopology = false;
                        // });

                        cfg.ReceiveEndpoint(
                            serviceInfo.broker_queue_name,
                            e =>
                            {
                                e.ConfigureConsumeTopology = false;
                                // e.ExchangeType = "direct";
                                // e.ConfigureConsumer<WorkflowMessageConsumer>(context);
                                e.Bind(
                                    serviceInfo.WorkflowDirectExchange,
                                    b =>
                                    {
                                        b.ExchangeType = "direct";
                                        b.RoutingKey = serviceInfo.broker_queue_name;
                                    }
                                );

                                e.ConfigureConsumer<WorkflowMessageConsumer>(context);
                            }
                        );
                    }
                );
            });

            services.AddScoped<IMessageService, MessageService>();
        }
    }
}
