namespace Optimal.Framework.Client
{
    public class ClientConfiguration
    {
        public string HostGrpcURL { get; set; }
        public string GrpcToken { get; set; } = "";

        public string YourGrpcURL { get; set; }

        public string YourServiceID { get; set; }

        public string YourInstanceID { get; set; } = Guid.NewGuid().ToString();
    }
}