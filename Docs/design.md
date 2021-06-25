# Design  

And external entity is publishing a data feed of permited food trucks.  The format we will look at is a CSV record dump, which is guaranteed to be malformed.  So right of the bat a more sophisticated csv parser is needed.  
The current [parser](../FoodTruckNearMe/MobileFoodFacilityPermitLoader.cs) assumes that at least the data we are interested in is OK. 

To facilitate developer happiness a drop of a [csv dump](../FoodTruckNearMe/Mobile_Food_Facility_Permit.csv) is served up locally and will facilitate integration testing during builds.  The download controller can be found [here](../FoodTruckNearMe/Controllers/TestFileDownload.cs)

## Downloader  
In the current design, a background task is run that will download a newer version of food truck permits and cache it for a time (usually a day).  The current data set is around 131 records, so storing it in memory in the app works.  If it gets larger then storing that drop in a distrubuted cache like CosmosDB or Redis will have to be entertained.  At the moment the applicaton is downloaded the drop from itself.  

The background task keeps 2 drops in memory and exposes a helper method to fetch the newest drop.  I primarily do that to avoid a lock on every request.  

## Abstractions  
The primary abstraction the application relies on is the following [interface](https://github.com/fluffy-bunny/FoodTrucksNearMe/blob/55eb7a88101552f956ec4a6db66ee52ad05e372d/Contracts/IFoodTruckService.cs#L33)  
```c#
public interface IFoodTruckService
{
    Task<ListFoodTruckPermitsResponse> ListFoodTruckPermitsAsync(ListFoodTruckPermitsRequest request);
}
```
From the apps perspective, it simply wants a services that implements this interface and it doesn't matter how that data is retrieved.  In the current design the data being retrieved is from an inmemory Array of FoodTruckPermits that I use LINQ as my query language.   If we decide that its better for the data to live in a distrubted cache, a different implementation of the interface can be swapped in.  

## Testing  
Simple unit testing of code that doesn't make calls to external services is easy enough to add.  I am more interested in intergration testing and that is where the asp.net core [TestServer](../FoodTruckNearMe_TestServer) comes in.  This allows tests to be http requests just like our customers will use, and thanks to dependency inject we can swap out anything we want.  Usually I would MOCK an implemenation of the ```IFoodTruckService``` service, but since I added a downloader controller that serves up a drop of real data, it makes things a lot easier on me.  

An example of swapping out a service for a MOCK can be seen [here](https://github.com/fluffy-bunny/FoodTrucksNearMe/blob/55eb7a88101552f956ec4a6db66ee52ad05e372d/FoodTruckNearMe_TestServer/CustomWebApplicationFactory.cs#L22)  






