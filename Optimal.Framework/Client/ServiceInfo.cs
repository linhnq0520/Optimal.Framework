namespace Optimal.Framework.Client
{
    public class ServiceInfo : IConfig
    {
        public string Name => "ServiceInfo";
        public string service_code { get; set; }
        public string broker_hostname { get; set; }
        public int broker_port { get; set; }
        public string broker_virtual_host { get; set; }
        public string broker_queue_name { get; set; }
        public string WorkflowDirectExchange { get; set; }
        public string broker_user_name { get; set; }
        public string broker_user_password { get; set; }
        public string broker_response_queue_name { get; set; }
        public bool ssl_active { get; set; }
        public string ssl_cert_base64 { get; set; }
        public string ssl_cert_pass_pharse { get; set; }
        public string ssl_cert_servername { get; set; }
    }
}
