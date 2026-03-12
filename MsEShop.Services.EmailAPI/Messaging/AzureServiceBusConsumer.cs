using Azure.Messaging.ServiceBus;
using MsEShop.Services.EmailAPI.Models.Dto;
using Newtonsoft.Json;
using System.Text;

namespace MsEShop.Services.EmailAPI.Messaging
{
    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly IConfiguration _configuration;

        private readonly string serviceBusConnectionString;
        private readonly string emailCartQueue;

        private ServiceBusProcessor _emailCartProcessor;

        public AzureServiceBusConsumer(IConfiguration configuration)
        {
            _configuration = configuration;

            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBus:ConnectionString")!;
            emailCartQueue = _configuration.GetValue<string>("ServiceBus:TopicAndQueueNames:EmailShoppingCart")!;

            var client = new ServiceBusClient(serviceBusConnectionString);

            //procesor that listens to the queue_topic:
            _emailCartProcessor = client.CreateProcessor(emailCartQueue);

        }

        public async Task Start()
        {
            _emailCartProcessor.ProcessMessageAsync += OnEmailCartRequestReceive;
            _emailCartProcessor.ProcessErrorAsync += ErrorHandler;

            await _emailCartProcessor.StartProcessingAsync();
        }

        public async Task Stop()
        {
            await _emailCartProcessor.StopProcessingAsync();
            await _emailCartProcessor.DisposeAsync();
        }



        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            //process error
            Console.WriteLine("Error occurred: " + args.Exception.ToString());

            return Task.CompletedTask;
        }

        private async Task OnEmailCartRequestReceive(ProcessMessageEventArgs args)
        {
            //process item in queue_topic
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            EmailCartDto? emailCartDto = JsonConvert.DeserializeObject<EmailCartDto>(body);

            try
            {
                //send email or whatever
                //...

                //set message as processed so it can be removed from the queue_topic:
                await args.CompleteMessageAsync(args.Message);

            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}