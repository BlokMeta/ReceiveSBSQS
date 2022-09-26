namespace ReceiveSBSQS.Models
{
    public class ReceiveQueueModel
    {
        public string AzureMessage { get; set; }
        public string AwsMessage { get; set; }
        public string? Error { get; set; }
    }

}
