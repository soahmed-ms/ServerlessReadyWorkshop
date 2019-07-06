using System;
using System.Net.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            HttpClient client = new HttpClient()
            {
                BaseAddress = new Uri("https://userevents.westus2-1.eventgrid.azure.net/api/events")
            };
            client.DefaultRequestHeaders.Add("aeg-sas-key", "Mq6jW5AEuUSoPvc2oRIii+ZIhyY66yGsuWhcGt7s3ZI=");

            var egevent = new
            {
                Id = 1234,
                Subject = "test",
                EventType = "CriticalChange",
                EventTime = DateTime.UtcNow,
                Data = ""
            };
            var x = await client..PostAsync()
        }
    }
}
