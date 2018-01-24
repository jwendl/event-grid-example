using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;

namespace CustomEventGridEventPublish
{
    class Program
    {
        static void Main(string[] args)
        {
            //Prerequisites 
            //Create an Custom Event Grid Topic in Azure Portal and get topic url and one Access Key

            // Publish events to topic
            //string topicHostname = new Uri("<EventGridTopic>").Host;
            //TopicCredentials topicCredentials = new TopicCredentials("<AccessKey>");

            // Publish events to topic
            string topicHostname = new Uri("https://imagecreated.westcentralus-1.eventgrid.azure.net/api/events").Host;
            TopicCredentials topicCredentials = new TopicCredentials("PZxp1zSWtPvjRxzmq786NpbS3oqlmSoHTDC7y6frI9k=");

            //Create EventGridEvent
            EventGridEvent myEvent = new EventGridEvent()
            {
                Id = Guid.NewGuid().ToString(),
                EventTime = DateTime.UtcNow,
                EventType = "CustomEventType",
                Subject = "CustomSubject",
                Data = new CustomData()
                {
                    Field1 = "Value1",
                    Field2 = "Value2"
                },
                DataVersion = "1.0"
            };

            //EventGridEvent list
            List<EventGridEvent> eventList = new List<EventGridEvent>();
            eventList.Add(myEvent);

            //EventGridClient
            EventGridClient client = new EventGridClient(topicCredentials);

            Console.WriteLine("Publishing to Azure Event Grid");
            //Publish Event
            client.PublishEventsAsync(topicHostname, eventList).Wait();
            Console.WriteLine("Published successfully!");
            Console.Read();
        }
    }
                
}
