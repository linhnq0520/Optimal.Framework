using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Optimal.Framework.Client
{
    public class QueueClient : IDisposable
    {
        private readonly ServiceInfo _serviceInfo;
        private IConnection _connection;
        private IModel _channel;
        private EventingBasicConsumer _consumer;
        private bool _disposed = false;
        private string _consumerTag;
        public event EventHandler<string> MessageReceived;
        public event EventHandler<string> ResponseReceived;

        public QueueClient(ServiceInfo serviceInfo)
        {
            _serviceInfo = serviceInfo ?? throw new ArgumentNullException(nameof(serviceInfo));
            Connect();
            Subscribe();
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

        private void ConfigureSSL(ConnectionFactory factory)
        {
            if (!_serviceInfo.ssl_active) return;

            factory.Ssl.Enabled = true;
            factory.Ssl.CertPath = GetLocalCertPath(_serviceInfo.ssl_cert_base64);
            factory.Ssl.CertPassphrase = _serviceInfo.ssl_cert_pass_pharse;
            factory.Ssl.ServerName = _serviceInfo.ssl_cert_servername;
        }

        public void Subscribe()
        {
            _consumer = new EventingBasicConsumer(_channel);
            _consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($" [x] Received {message}");

                MessageReceived?.Invoke(this, message);

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _consumerTag = _channel.BasicConsume(_serviceInfo.broker_queue_name, false, _consumer);
        }

        public void SubscribeResponse()
        {
            _consumer = new EventingBasicConsumer(_channel);
            _consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var response = Encoding.UTF8.GetString(body);
                Console.WriteLine($" [x] Received response: {response}");

                // Trigger event ResponseReceived
                ResponseReceived?.Invoke(this, response);

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _consumerTag = _channel.BasicConsume(_serviceInfo.broker_response_queue_name, false, _consumer);
        }

        public void ProcessMessage(string message)
        {
            // Logic xử lý message
            Console.WriteLine($"Processing message: {message}");
            // ...

            // Tạo response
            //string response = $"Response for message: {message}";

            // Gửi response vào queue response
            //SendMessage(_serviceInfo.broker_response_queue_name, response);
        }

        public void SendMessage(string queueName, string message)
        {
            if (string.IsNullOrEmpty(message))
                throw new ArgumentNullException(nameof(message));

            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(string.Empty, queueName, null, body);
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

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                try
                {
                    // Hủy đăng ký consumer nếu đang hoạt động
                    if (_channel != null && _channel.IsOpen && !string.IsNullOrEmpty(_consumerTag))
                    {
                        _channel.BasicCancel(_consumerTag);
                    }

                    _channel?.Close();
                    _channel?.Dispose();
                    _connection?.Close();
                    _connection?.Dispose();

                    // Xóa tệp chứng chỉ tạm thời nếu có
                    if (!string.IsNullOrEmpty(_serviceInfo.ssl_cert_base64))
                    {
                        var tempCertPath = GetLocalCertPath(_serviceInfo.ssl_cert_base64);
                        if (File.Exists(tempCertPath))
                        {
                            File.Delete(tempCertPath);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception
                    Console.WriteLine($"Error during dispose: {ex.Message}");
                }
            }

            _disposed = true;
        }
    }
}