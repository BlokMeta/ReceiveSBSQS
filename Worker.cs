using ReceiveMessagesSQS_SB.Services;

namespace SenderQueueMessageServices.NetCore
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _config;
        private readonly IReceivers _processor;
        public Worker(IConfiguration config, ILogger<Worker> logger, IReceivers processor)
        {
            _config = config;
            _logger = logger;
            _processor = processor;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker running at: (time)", DateTimeOffset.Now);
            await _processor.ReceiveMessageAsync();
            await Task.Delay(5000, stoppingToken);

        }
    }
}
