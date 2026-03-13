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
        private readonly string registerNewUserEmailQueue;

        private ServiceBusProcessor _emailCartProcessor;
        private ServiceBusProcessor _registerNewUserProcessor;

        //As this is a singleton, we cannot access AppDbContext because it's not of the same scope. So we need to create the EmailService that wraps the AppDbContext to a Singleton with DbContextOptionsBuilder
        // We can now inject EmailService (NOT THE INTERFACE: builder.Services.AddSingleton(new EmailService(optionBuilder.Options));)
        private readonly EmailService _emailService;


        public AzureServiceBusConsumer(IConfiguration configuration, EmailService emailService)
        {
            _configuration = configuration;

            serviceBusConnectionString = _configuration.GetValue<string>("ServiceBus:ConnectionString")!;
            emailCartQueue = _configuration.GetValue<string>("ServiceBus:TopicAndQueueNames:EmailShoppingCart")!;
            registerNewUserEmailQueue = _configuration.GetValue<string>("ServiceBus:TopicAndQueueNames:RegisterNewUser")!;

            var client = new ServiceBusClient(serviceBusConnectionString);

            //procesors that listens to the different queue_topic:
            _emailCartProcessor = client.CreateProcessor(emailCartQueue);
            _registerNewUserProcessor = client.CreateProcessor(registerNewUserEmailQueue);

            //inject email service for dealing with singleton database
            _emailService = emailService;

        }

        public async Task Start()
        {
            _emailCartProcessor.ProcessMessageAsync += OnEmailCartRequestReceive;
            _emailCartProcessor.ProcessErrorAsync += ErrorHandler;

            await _emailCartProcessor.StartProcessingAsync();

            _registerNewUserProcessor.ProcessMessageAsync += OnRegisterNewUserRquestReceive;
            _registerNewUserProcessor.ProcessErrorAsync += ErrorHandler;

            await _registerNewUserProcessor.StartProcessingAsync();
        }

        public async Task Stop()
        {
            await _emailCartProcessor.StopProcessingAsync();
            await _emailCartProcessor.DisposeAsync();

            await _registerNewUserProcessor.StopProcessingAsync();
            await _registerNewUserProcessor.DisposeAsync();
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
            string body = GetBodyFromProcessMessageEventArgs(args);

            EmailCartDto? emailCartDto = JsonConvert.DeserializeObject<EmailCartDto>(body);

            try
            {
                //send email or whatever
                if (emailCartDto != null) await _emailService.EmailCartAndLog(emailCartDto);

                //set message as processed so it can be removed from the queue_topic:
                await args.CompleteMessageAsync(args.Message);

            }
            catch (Exception)
            {
                throw;
            }
        }

        private static string GetBodyFromProcessMessageEventArgs(ProcessMessageEventArgs args)
        {
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);
            return body;
        }

        private async Task OnRegisterNewUserRquestReceive(ProcessMessageEventArgs args)
        {
            //process item in queue_topic
            string body = GetBodyFromProcessMessageEventArgs(args);

            string? email = JsonConvert.DeserializeObject<string>(body);

            try
            {
                if (email != null) await _emailService.RegisterUserEmailAndLog(email);

                await args.CompleteMessageAsync(args.Message);
            }
            catch (Exception)
            {
                throw;
            }

        }


    }
}