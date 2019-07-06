 ---------------Azure function --------------------

1. Azure portal -> Creating Resource Group (In west US)
2. Go to Marketplace -> create resource -> azure CosmosDB 
3. Create new solution in VS, add a project -> azure function
4. Choose trigger as -> CosmosDB trigger, V2 .net core
5. Add connection settings from the portal of the CosmosDB resource
6. Add a file -> local.settings.json -> add connection strings setting
7. Cross check the configs of the function in vs
8. Add lease setting – CreateLeaseCollectionIfNotExists = true in the function defintion
9. Create a Database and Collection in the CosmosDB created in Step 2.
10. Add an item in collection, you can refer the git solution for sample jsons
11. Debug azure function, add appropriate breakpoints
12. Add a new item in CosmosDB
13. Breakpoint hits – verify the added item
14. 

-------------- Publishing azure function--------------------------
Go to marketplace and Create a function app [Explain publishing process overview]
Get the publish profile of the function app from azure portal
Form visual studio, right click and publish the function app
Add cosmos db connection string in app setting 
Import Profile and publish [Explain other ways of publishing]


------------------ Event grid-------------------
In portal create Event grid topic of Event grid schema type [Explain integration with logic app] [Event grid concepts] {Learn diff types}
Create an event subscription 
Create storage account, create queue in it
Go back to event grid subscription, add the queue as the endpoint [Explain why]
Add code for sending out events to event grid
Add event grid configurations to function
Debug and test azure function- event should reflect in event grid

----------- Logic app------------
Go to marketplace and create logic app
Create blank logic app
Select trigger- Event grid – when a resource event occurs
Configure trigger with subscription, Resource type – event grid topics, Resource name, event type items {Learn diff types}
Add action- switch control
Switch on – Event type
On critical change – Add action – Outlook 365 - send an email 
Add dynamic content to action and hit save
Test [By changing cosmos item -> check event grid -> check logic app run]












