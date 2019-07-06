using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace FunctionApp1
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async System.Threading.Tasks.Task RunAsync([CosmosDBTrigger(
            databaseName: "coffeedb",
            collectionName: "coffeecontainer",
            ConnectionStringSetting = "ConnectionStringSetting",
            LeaseCollectionName = "leases",
            CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> input, ILogger log)
        {
            if (input != null && input.Count > 0)
            {
                log.LogInformation("Documents modified " + input.Count);
                log.LogInformation("First document Id " + input[0].Id);
            }

            // TODO : add the code for extracting the changes 

            // TODO : code to populate the event

            HttpClient client = new HttpClient()
            {
                BaseAddress = new Uri("https://coffeeeventgridtopic.westus2-1.eventgrid.azure.net/api/events")
            };
            client.DefaultRequestHeaders.Add("aeg-sas-key", "pSUCIsPwbtBPHQypvbchH5Y8wrinXKgr1Ky9VpV7T+s=");

            var egevent = new
            {
                Id = 1234,
                Subject = "test",
                EventType = "CriticalChange",
                EventTime = DateTime.UtcNow,
                Data = ""
            };
            var x = await client.PostAsJsonAsync("", new[] { egevent });
        }
    }
}
