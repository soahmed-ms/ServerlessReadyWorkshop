using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CoffeeDB;
using CoffeeDB.Models;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
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
                DocumentClient docClient = new DocumentClient(new Uri("https://userdatabase.documents.azure.com:443/"), "TeGmsXpXvYlHJwSYPQ0EtepKoiW28huKUsmpvYlUMtVyIZiUmeKzAo3e0RzTBc8JVxJYFYTkKcGoGZz89BmFBA==");
                var collectionLink = UriFactory.CreateDocumentCollectionUri("userdatabase", "Customers");
                FeedOptions queryOptions = new FeedOptions { MaxItemCount = -1 };
                // docs added

                // setup event grid
                HttpClient client = new HttpClient()
                {
                    BaseAddress = new Uri("https://userevents.westus2-1.eventgrid.azure.net/api/events")
                };
                client.DefaultRequestHeaders.Add("aeg-sas-key", "Mq6jW5AEuUSoPvc2oRIii+ZIhyY66yGsuWhcGt7s3ZI=");

                foreach(var document in input)
                {
                    OrderModel order = JsonConvert.DeserializeObject<OrderModel>(document.ToString());

                    Customer queryCustomer = docClient.CreateDocumentQuery<Customer>(collectionLink, queryOptions)
                                            .Where(f => f.PhoneNumber == order.CustomerContactNumber).FirstOrDefault();


                    if (queryCustomer != null)
                    {
                        queryCustomer.Points += 100;
                    }
                    else
                    {
                        Customer newCustomer = new Customer() { Name = order.CustomerName, PhoneNumber = order.CustomerContactNumber, Points = 100, Email = order.CustomerEmail };
                        await docClient.CreateDocumentAsync(collectionLink, newCustomer);
                    }

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
