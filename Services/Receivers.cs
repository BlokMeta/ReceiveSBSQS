using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using ReceiveSBSQS.Configuration;
using ReceiveSBSQS.Models;

namespace ReceiveMessagesSQS_SB.Services
{
    public class Receivers : IReceivers
    {
        //AWS

        ReceiveQueueModel model = new();

        public async Task<string> AwsReceiveAsync()
        {
            var _sqsClient = new AmazonSQSClient(QueueSettings.awsAccessKey, QueueSettings.awsSecret, RegionEndpoint.EUWest2);
            var receiveMessageRequest = new ReceiveMessageRequest
            {
                QueueUrl = QueueSettings.awsQueuUrl
            };
            //receiveMessageRequest.MaxNumberOfMessages = 10;
            //receiveMessageRequest.VisibilityTimeout = 10;
            //receiveMessageRequest.WaitTimeSeconds = 3;
            var response = await _sqsClient.ReceiveMessageAsync(receiveMessageRequest);

            if (response.Messages.Count > 0)
            {
                model.AwsMessage = response.Messages[0].Body;
                await _sqsClient.DeleteMessageAsync(QueueSettings.awsQueuUrl, response.Messages[0].ReceiptHandle);
            }
            return model.AwsMessage;
        }

        public async Task<string> AzureReceiveAsync()
        {
            ServiceBusClient client = new ServiceBusClient(QueueSettings.azureConnectionString);
            var receiver = client.CreateReceiver(QueueSettings.azureQueueName);
            var administrationClient = new ServiceBusAdministrationClient(QueueSettings.azureConnectionString);
            var props = await administrationClient.GetQueueRuntimePropertiesAsync(QueueSettings.azureQueueName);
            var messageCount = props.Value.ActiveMessageCount;

            if (messageCount > 0)
            {
                try
                {
                    var message = await receiver.ReceiveMessageAsync();
                    model.AzureMessage = message.Body.ToString();
                    await receiver.CompleteMessageAsync(message);
                }
                finally
                {
                    await receiver.DisposeAsync();
                    await client.DisposeAsync();
                }
            }
            return model.AzureMessage;
        }

        public async Task<ReceiveQueueModel> ReceiveMessageAsync(bool isAmazon = true, bool isAzure = true)
        {
            model.AwsMessage = "AWS Simple Queue Service has no message!";
            model.AzureMessage = "Azure Service Bus has no message!";

            if (isAmazon == true && isAzure == true)
            {
                await AwsReceiveAsync();
                await AzureReceiveAsync();
            }
            else
            {
                if (isAmazon == true)
                    await AwsReceiveAsync();

                else await AzureReceiveAsync();
            }
            return model;
        }

    }
}
