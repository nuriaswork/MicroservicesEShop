using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System.Text;

namespace MsEshop.MessageBus
{
    public class MessageBus : IMessageBus
    {
        public async Task PublishMessage(object message, string topic_queue_name)
        {
            await using var client = new ServiceBusClient("connection_to_Azure_ServiceBus");

            ServiceBusSender sender = client.CreateSender(topic_queue_name);

            var jsonMessage = JsonConvert.SerializeObject(message);

            ServiceBusMessage serviceBusMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage))
            {
                CorrelationId = Guid.NewGuid().ToString()
            };

            await sender.SendMessageAsync(serviceBusMessage);

            await client.DisposeAsync();
        }
    }
}
