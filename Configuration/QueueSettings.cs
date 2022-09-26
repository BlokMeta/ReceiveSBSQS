namespace ReceiveSBSQS.Configuration
{
    public class QueueSettings
    {

        public static string? azureConnectionString { get; set; }
        public static string? azureQueueName { get; set; }

        public static string? awsAccessKey { get; set; }
        public static string? awsSecret { get; set; }
        public static string? awsQueuUrl { get; set; }
        public static string? awsRegion { get; set; }
    }
}
