using Amazon;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace helloSNS
{
    class Program
    {
        const string TopicARN = "arn:aws:sns:us-west-1:[account]:weather";
        static RegionEndpoint Region = Amazon.RegionEndpoint.[region];

        static async Task Main(string[] args)
        {
            var action = args.Length == 0 ? string.Empty : args[0];

            switch (action)
            {
                case "publisher":
                    await Publisher();
                    break;
                case "subscriber":
                    await Subscriber();
                    break;
                default:
                    Console.WriteLine("Usage: specify action on command line - dotnet run -- <action> (publisher or subscriber)");
                    break;
            }
        }

        static async Task Publisher()
        {
            Random rand = new Random();
            var city = (new string[] { "Seattle", "San Francisco", "Dallas", "Denver", "Boston", "New York", "Miami" })[rand.Next(7)];
            var temp = rand.Next(100) + 20;

            string message = $"{DateTime.Now.ToShortTimeString()} {city} {temp} degrees F";

            var notificationClient = new AmazonSimpleNotificationServiceClient(region: Region);

            var request = new PublishRequest
            {
                Message = message,
                TopicArn = TopicARN
            };

            try
            {
                Console.WriteLine("Publishing...");
                var response = await notificationClient.PublishAsync(request);

                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    Console.WriteLine("Message sent to topic:");
                    Console.WriteLine(message);
                }
                else
                {
                    Console.WriteLine($"HTTP status {response.HttpStatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in publish action:");
                Console.WriteLine(ex.Message);
            }
        }
        static async Task Subscriber()
        {
            Console.WriteLine("Connecting to SQS");

            var client = new AmazonSQSClient(region: Region);
            var queueUrl = (await client.GetQueueUrlAsync("weather")).QueueUrl;

            Console.WriteLine("Listening for messages");

            var request = new ReceiveMessageRequest()
            {
                QueueUrl = queueUrl,
                MaxNumberOfMessages = 10,
                WaitTimeSeconds = 10
            };

            while (true)
            {
                var response = await client.ReceiveMessageAsync(request);

                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    var messages = response.Messages;
                    if (messages.Count > 0)
                    {
                        Console.WriteLine($"{messages.Count} messages received");
                        foreach (var msg in messages)
                        {
                            Console.WriteLine(msg.Body);
                        }

                        Console.WriteLine($"Deleting queue messages");
                        foreach (var msg in messages)
                        {
                            await client.DeleteMessageAsync(queueUrl, msg.ReceiptHandle);
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"HTTP status {response.HttpStatusCode}");
                }
            }

        }

    }
}



