using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Optimal.Framework.Client
{
    public class QueueClient : IDisposable
    {
        public delegate void MessageDeliveringDelegate(string content);
        public event MessageDeliveringDelegate MessageDelivering;
        private bool IsConnected;
        private readonly object objLockSetup = new object();
        private readonly ServiceInfo _serviceInfo;
        private ConnectionFactory _factory;
        private IConnection _connection;
        private IModel _channel;
        private EventingBasicConsumer CommandQueueConsumer;
        private string InstanceID { get; } = Guid.NewGuid().ToString();
        private bool _disposed = false;
        private string _consumerTag;

        public QueueClient(ServiceInfo serviceInfo)
        {
            _serviceInfo = serviceInfo ?? throw new ArgumentNullException(nameof(serviceInfo));
            try
            {
                lock (objLockSetup)
                {
                    if (!IsConnected)
                    {
                        AutoConfigure();
                    }
                }
            }
            catch (Exception pException)
            {
                Console.WriteLine(pException);
            }
        }

        public void Connect()
        {
            var factory = new ConnectionFactory
            {
                HostName = _serviceInfo.broker_hostname,
                Port = _serviceInfo.broker_port,
                VirtualHost = _serviceInfo.broker_virtual_host,
                UserName = _serviceInfo.broker_user_name,
                Password = _serviceInfo.broker_user_password,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(10)
            };

            ConfigureSSL(factory);

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(_serviceInfo.broker_queue_name, durable: true, exclusive: false, autoDelete: false);
        }

        private string GetLocalCertPath(string certBase64)
        {
            if (string.IsNullOrEmpty(certBase64)) return null;

            var tempDir = Path.GetTempPath();
            var certPath = Path.Combine(tempDir, $"queue-client-cert-{Guid.NewGuid()}.pfx");
            var certBytes = Convert.FromBase64String(certBase64);
            File.WriteAllBytes(certPath, certBytes);
            return certPath;
        }

        private void ConfigureSSL(ConnectionFactory factory)
        {
            if (!_serviceInfo.ssl_active) return;

            factory.Ssl.Enabled = true;
            factory.Ssl.CertPath = GetLocalCertPath(_serviceInfo.ssl_cert_base64);
            factory.Ssl.CertPassphrase = _serviceInfo.ssl_cert_pass_pharse;
            factory.Ssl.ServerName = _serviceInfo.ssl_cert_servername;
        }

        private bool CreateFactory()
        {
            try
            {
                _connection?.Close();
                _connection?.Dispose();
            }
            catch
            {
            }
            _factory = new ConnectionFactory
            {
                VirtualHost = _serviceInfo.broker_virtual_host,
                HostName = _serviceInfo.broker_hostname,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(15.0),
                RequestedHeartbeat = TimeSpan.FromSeconds(30.0),
                ClientProvidedName = "QueueClient:" + _serviceInfo.service_code
            };
            if (_serviceInfo.ssl_active)
            {
                ConfigureSSL(_factory);
            }
            else
            {
            }
            return true;
        }
        private void ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            try
            {
                _connection?.Close();
                _connection?.Dispose();
            }
            catch
            {
            }
            IsConnected = false;
        }
        private bool CreateConnection()
        {
            if (_factory == null)
            {
                CreateFactory();
            }
            try
            {
                _channel?.Dispose();
            }
            catch
            {
            }
            try
            {
                _connection?.Close();
                _connection?.Dispose();
            }
            catch
            {
            }
            _factory.HostName = _serviceInfo.broker_hostname;
            _factory.Port = (_serviceInfo.broker_port > 0) ? _serviceInfo.broker_port : _factory.Port;
            _factory.VirtualHost = _serviceInfo.broker_virtual_host;
            _factory.UserName = _serviceInfo.broker_user_name;
            _factory.Password = _serviceInfo.broker_user_password;
            _connection = _factory.CreateConnection();
            _connection.ConnectionShutdown += ConnectionShutdown;
            IsConnected = true;
            return true;
        }

        private bool CreateChannel()
        {
            if (_factory == null)
            {
                CreateFactory();
            }
            if (_connection == null)
            {
                CreateConnection();
            }
            _channel = _connection.CreateModel();
            return true;
        }

        public bool CreateQueue(string pQueueName)
        {
            _channel.QueueDeclare(pQueueName, durable: true, exclusive: false, autoDelete: false, null);
            return true;
        }

        private bool Setup()
        {
            lock (objLockSetup)
            {
                CreateFactory();
                CreateConnection();
                CreateChannel();
                CreateQueue(_serviceInfo.broker_queue_name);
            }
            return true;
        }

        private void CommandMessageComingHandler(object sender, BasicDeliverEventArgs e)
        {
            try
            {
                if (this.MessageDelivering == null)
                {
                    return;
                }
                byte[] array = e.Body.ToArray();
                string @string = Encoding.UTF8.GetString(array, 0, array.Length);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                _channel.BasicAck(e.DeliveryTag, multiple: false);
            }
        }

        public void Subscribe()
        {
            if (_serviceInfo.broker_queue_name.Length != 0)
            {
                CommandQueueConsumer = new EventingBasicConsumer(_channel);
                CommandQueueConsumer.Received += CommandMessageComingHandler;
                _channel.BasicConsume(_serviceInfo.broker_queue_name, autoAck: false, CommandQueueConsumer);
            }
        }

        public bool AutoConfigure()
        {
            lock (objLockSetup)
            {
                Setup();
                if (this.MessageDelivering != null)
                {
                    Subscribe();
                }
            }
            ThreadPool.GetMinThreads(out var workerThreads, out var completionPortThreads);
            ThreadPool.SetMinThreads(200, completionPortThreads);
            return true;
        }

        public void SendMessage(string pQueueName, string pContent)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(pContent);
            _channel.BasicPublish("", pQueueName, null, bytes);
        }

        private string SerializeObjectWithSnakeCaseNaming(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            };
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            };
            return JsonConvert.SerializeObject(obj, settings);
        }

        public void ReplyMessage(string message)
        {

        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
            if (_factory != null)
            {
                _channel.Dispose();
            }
        }
    }
}
