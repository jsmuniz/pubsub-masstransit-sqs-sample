using System;
using MassTransit;
using MassTransit.AmazonSqsTransport.Configuration;
using PubSub.Messages;

namespace PubSub.Publisher
{
    class Program
    {
        static void Main(string[] args)
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

                x.Message<StringMessage>(cfg =>
                {
                    cfg.SetEntityName("string-message-topic");
                });

                x.Message<DateTimeMessage>(cfg =>
                {
                    cfg.SetEntityName("datetime-message-topic");
                });
            });

            bus.StartAsync().Wait();

            while (true)
            {
                Console.WriteLine(@"a) Publish string
b) Publish DateTime
q) Quit");

                var keyChar = char.ToLower(Console.ReadKey(true).KeyChar);

                switch (keyChar)
                {
                    case 'a':
                        bus.Publish(new StringMessage("Hello there, I'm a publisher!")).Wait();
                        break;

                    case 'b':
                        bus.Publish(new DateTimeMessage()).Wait();
                        break;


                    case 'q':
                        goto consideredHarmful;

                    default:
                        Console.WriteLine("There's no option ({0})", keyChar);
                        break;
                }
            }

            consideredHarmful:
            Console.WriteLine("Quitting!");
        }
    }

}
