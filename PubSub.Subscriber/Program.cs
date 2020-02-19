using System;
using System.Threading.Tasks;
using GreenPipes;
using MassTransit;
using MassTransit.AmazonSqsTransport.Configuration;
using PubSub.Messages;

namespace PubSub.Subscriber
{
    class Program
    {
        static void Main()
        {
            var bus = Bus.Factory.CreateUsingAmazonSqs(x =>
            {
                const string region = "<REPLACE WITH THE AWS DESIRED REGION>";
                const string accessKey = "<REPLACE WITH YOUR AWS ACCESS-KEY>";
                const string secretKey = "<REPLACE WITH YOUR AWS SECRET KEY>";

                x.Host(region, h =>
                {
                    h.AccessKey(accessKey);
                    h.SecretKey(secretKey);
                });


                x.ReceiveEndpoint("string-message-input-queue", e =>
                {

                    e.UseMessageRetry(r => r.Immediate(5));

                    e.Subscribe("string-message-topic", callback =>{});

                    e.Consumer(() => new Handler());
                });

                x.ReceiveEndpoint("datetime-message-input-queue", e =>
                {
                    e.Subscribe("datetime-message-topic", callback =>{});

                    e.Consumer(() => new Handler());
                });
            });

            bus.StartAsync().Wait();

            Console.WriteLine("Listening to messages...");
            Console.WriteLine("Press enter to quit");
            Console.ReadLine();
        }
    }

    class Handler : IConsumer<StringMessage>, IConsumer<DateTimeMessage>
    {
        public Task Consume(ConsumeContext<StringMessage> context)
        {
            Console.WriteLine("Got string: {0}", context.Message.Message);
            return Task.CompletedTask;
        }

        public Task Consume(ConsumeContext<DateTimeMessage> context)
        {
            Console.WriteLine("Got date: {0}", context.Message.Date);
            return Task.CompletedTask;
        }
    }
}
