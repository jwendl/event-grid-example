using Bogus;
using EventHubDataSender.Models;
using Microsoft.Azure.EventHubs;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace EventHubDataSender
{
    class Program
    {
        private static EventHubClient eventHubClient;
        private const string eventHubConnectionString = "Endpoint=sb://jwegexehns.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=KOZTlDfkOYhC4qNVUkSjrxsCdCShDC8Z67OOiICcxg8=";
        private const string eventHubEntityPath = "jwegexeh";

        private static async Task Main(string[] args)
        {
            // Creates an EventHubsConnectionStringBuilder object from the connection string, and sets the EntityPath.
            // Typically, the connection string should have the entity path in it, but for the sake of this simple scenario
            // we are using the connection string from the namespace.
            var connectionStringBuilder = new EventHubsConnectionStringBuilder(eventHubConnectionString)
            {
                EntityPath = eventHubEntityPath
            };

            eventHubClient = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());

            await SendMessagesToEventHub(10000);

            await eventHubClient.CloseAsync();

            Console.WriteLine("Press ENTER to exit.");
            Console.ReadLine();
        }

        // Creates an event hub client and sends 100 messages to the event hub.
        private static async Task SendMessagesToEventHub(int messageCount)
        {
            for (var messageIndex = 0; messageIndex < messageCount; messageIndex++)
            {
                try
                {
                    //var message = $"Message {messageIndex}";
                    var orderFaker = new Faker<Order>()
                        .RuleFor(o => o.Item, p => p.Commerce.ProductName())
                        .RuleFor(o => o.SerialNumber, p => p.Commerce.Product())
                        .RuleFor(o => o.Quantity, p => p.Random.Int(1, 100))
                        .RuleFor(o => o.Price, p => p.Commerce.Price());

                    var customerFaker = new Faker<Customer>()
                        .RuleFor(c => c.FirstName, p => p.Person.FirstName)
                        .RuleFor(c => c.LastName, p => p.Person.LastName)
                        .RuleFor(c => c.BirthDate, p => p.Person.DateOfBirth)
                        .RuleFor(c => c.Orders, orderFaker.Generate(100));

                    var message = JsonConvert.SerializeObject(customerFaker.Generate());
                    Console.WriteLine($"Sending message: {message}");
                    await eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(message)));
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"{DateTime.Now} > Exception: {exception.Message}");
                }

                await Task.Delay(10);
            }

            Console.WriteLine($"{messageCount} messages sent.");
        }
    }
}
