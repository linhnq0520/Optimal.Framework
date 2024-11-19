using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Optimal.Framework.Client;
using Optimal.Framework.Infrastructure;
using Optimal.Framework.Messaging.Services;

namespace Optimal.Framework.Messaging.Configuration
{
    public class MessagingStartup : IOptimalStartup
    {
        public int Order => 1;

        public void Configure(IApplicationBuilder application) { }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            var serviceInfo = Singleton<AppSettings>.Instance.Get<ServiceInfo>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<WorkflowMessageConsumer>();

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

                        cfg.ReceiveEndpoint(
                            serviceInfo.broker_queue_name,
                            e =>
                            {
                                e.ConfigureConsumeTopology = false;
                                e.ExchangeType = "direct";
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
