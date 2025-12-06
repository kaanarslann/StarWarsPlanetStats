using StarWarsPlanetsStats.ApiDataAccess;
using StarWarsPlanetStats.ApiDataAccess;


try
{
    await new StarWarsPlanetsStatsApp(
        new PlanetsFromApiReader(
            new ApiDataReader(),
            new MockStarWarsApiDataReader()),
        new PlanetsStatistics()).Run();
}
catch(Exception ex)
{
    Console.WriteLine("An error occured. Exception message: " + ex.Message);
}

Console.WriteLine("Closing...");
Console.ReadLine();
