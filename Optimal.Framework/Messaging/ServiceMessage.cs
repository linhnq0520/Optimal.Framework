using MassTransit;

namespace Optimal.Framework.Messaging.Contracts
{
    public interface IServiceMessage : IConsumer
    {
        string ServiceCode { get; }
        string MessageType { get; }
        string Content { get; }
        DateTime Timestamp { get; }
    }

    public class ServiceMessage : IServiceMessage
    {
        public string ServiceCode { get; set; }
        public string MessageType { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    public class WorkflowMessage
    {
        public string ExecutionId { get; set; } = Guid.NewGuid().ToString();
        public string WorkflowId { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public object Request { get; set; }
        public object Response { get; set; }
    }
}
