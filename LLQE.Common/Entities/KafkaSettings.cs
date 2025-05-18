namespace LLQE.Common.Entities
{
    public class KafkaSettings
    {
        public string BootstrapServers { get; set; }
        public List<string> ResponseTopics { get; set; }
        public List<string> RequestTopics { get; set; }
        public Dictionary<string, string> NodeResponseTopics { get; set; }
    }
}
