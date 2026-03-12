using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Identity.UI.Services;
using MsEShop.Services.EmailAPI.Models.Dto;
using MsEShop.Services.EmailAPI.Services;
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

        //As this is a singleton, we cannot access AppDbContext because it's not of the same scope. So we need to create the EmailService that wraps the AppDbContext to a Singleton with DbContextOptionsBuilder
        // We can now inject EmailService (NOT THE INTERFACE: builder.Services.AddSingleton(new EmailService(optionBuilder.Options));)
        private readonly EmailService _emailService;


        public AzureServiceBusConsumer(IConfiguration configuration, EmailService emailService)
        {
            _configuration = configuration;

            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBus:ConnectionString")!;
            emailCartQueue = _configuration.GetValue<string>("ServiceBus:TopicAndQueueNames:EmailShoppingCart")!;

            var client = new ServiceBusClient(serviceBusConnectionString);

            //procesor that listens to the queue_topic:
            _emailCartProcessor = client.CreateProcessor(emailCartQueue);
            _emailService = emailService;
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
                await _emailService.EmailCartAndLog(emailCartDto);

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