using StarWarsPlanetsStats.ApiDataAccess;
using StarWarsPlanetStats.ApiDataAccess;
using StarWarsPlanetStats.DTOs;
using System.Text.Json;

try
{
    await new StarWarsPlanetsStatsApp(new ApiDataReader(), new MockStarWarsApiDataReader()).Run();
}
catch(Exception ex)
{
    Console.WriteLine("An error occured. Exception message: " + ex.Message);
}

Console.WriteLine("Closing...");
Console.ReadLine();

public class StarWarsPlanetsStatsApp
{
    private readonly IApiDataReader _apiDataReader;
    private readonly IApiDataReader _secondaryApiDataReader;

    public StarWarsPlanetsStatsApp(IApiDataReader apiDataReader, IApiDataReader secondaryApiDataReader)
    {
        _apiDataReader = apiDataReader;
        _secondaryApiDataReader = secondaryApiDataReader;
    }

    public async Task Run()
    {
        string? json = null;
        try
        {
            json = await _apiDataReader.Read("https://swapi.info/", "api/planets");
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine("API request was unsuccessful. Switching to mock data. Exception message: " + ex.Message);
        }
        if (json is null)
        {
            json = await _secondaryApiDataReader.Read("https://swapi.info/", "api/planets");
        }


        var root = JsonSerializer.Deserialize<List<Root>>(json);
        var planets = ToPlanet(root);
    }

    private IEnumerable<Planet> ToPlanet(List<Root>? root)
    {
        if(root is null)
        {
            throw new ArgumentNullException(nameof(root));
        }
        throw new NotImplementedException();
    }
}

public readonly record struct Planet
{
    public string Name { get; }
    public int? Diameter { get; }
    public int? SurfacecWater { get; }
    public int? Population { get; }

    public Planet(string name, int? diameter, int? surfacecWater, int? population)
    {
        if(name is null)
        {
            throw new ArgumentNullException($"{nameof(name)} is null");
        }
        Name = name;
        Diameter = diameter;
        SurfacecWater = surfacecWater;
        Population = population;
    }
}