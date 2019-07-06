using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using CoffeeDB;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft;
using Newtonsoft.Json;

namespace DbAudit
{
    public static class CoffeeFunction
    {
        [FunctionName("DbAudit")]
        public static async Task RunAsync([CosmosDBTrigger(
            databaseName: "user",
            collectionName: "Orders",
            ConnectionStringSetting = "ConnectionStringSetting",
            LeaseCollectionName = "orderleases",
            CreateLeaseCollectionIfNotExists = true
            )]IReadOnlyList<Document> input, ILogger log)
        {
            if (input != null && input.Count > 0)
            {
                // docs added

                // setup event grid
                HttpClient client = new HttpClient()
                {
                    BaseAddress = new Uri("https://userevents.westus2-1.eventgrid.azure.net/api/events")
                };
                client.DefaultRequestHeaders.Add("aeg-sas-key", "Mq6jW5AEuUSoPvc2oRIii+ZIhyY66yGsuWhcGt7s3ZI=");

                foreach(var document in input)
                {
                    var order = JsonConvert.DeserializeObject<OrderModel>(document.ToString());
                    foreach (var item in order.OrderItems)
                    {
                        var egevent = new
                        {
                            Id = 1,
                            Subject = "new item",
                            EventType = item.ItemType.ToString(),
                            EventTime = DateTime.UtcNow,
                            Data = "new item"
                        };
                        //if (item.ItemType == ItemType.Merchandise)
                        //{

                        //}
                        //else if (item.ItemType == ItemType.Food)
                        //{
                        //    var egevent = new
                        //    {
                        //        Id = document.Id + "_" + item.GetHashCode() + "_" + ItemType.Food,
                        //        Subject = item.Quantity + "_" + item.ItemName,
                        //        EventType = ItemType.Food,
                        //        EventTime = DateTime.UtcNow,
                        //        Data = "Counter 1"
                        //    };
                        //}
                        //else if (item.ItemType == ItemType.Beverage)
                        //{
                        //    var egevent = new
                        //    {
                        //        Id = document.Id + "_" + item.GetHashCode() + "_" + ItemType.Beverage,
                        //        Subject = item.Quantity + "_" + item.ItemName,
                        //        EventType = ItemType.Beverage,
                        //        EventTime = DateTime.UtcNow,
                        //        Data = "Counter 1"
                        //    };
                        //}

                        var x = await client.PostAsJsonAsync("", new[] { egevent });
                    }
                }

                
        }
        }
    }
}
