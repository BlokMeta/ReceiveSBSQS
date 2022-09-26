using ReceiveSBSQS.Models;

namespace ReceiveMessagesSQS_SB.Services
{
    public interface IReceivers
    {
        Task<ReceiveQueueModel> ReceiveMessageAsync(bool isAmazon = true, bool isAzure = true);
    }
}