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

        foreach(var planet in planets)
        {
            Console.WriteLine(planet);
        }

        Console.WriteLine();
        Console.WriteLine("The statistics of which property would you like to see?");
        Console.WriteLine("population");
        Console.WriteLine("diameter");
        Console.WriteLine("surface water");

        var userInput = Console.ReadLine();

        if(userInput == "population")
        {
            ShowStatistics(planets, "population", planet => planet.Population);
        }
        else if (userInput == "diameter")
        {
            ShowStatistics(planets, "diameter", planet => planet.Diameter);
        }
        else if (userInput == "surface water")
        {
            ShowStatistics(planets, "surface wate", planet => planet.SurfaceWater);
        }
        else
        {
            Console.WriteLine("Invalid choice!");
        }
    }

    private void ShowStatistics(IEnumerable<Planet> planets, string propertyName, Func<Planet, int?> propertySelector)
    {
        var maxPlanet = planets.MaxBy(propertySelector);
        Console.WriteLine($"Max {propertyName} is: {propertySelector(maxPlanet)} (planet: {maxPlanet.Name})");

        var minPlanet = planets.MinBy(propertySelector);
        Console.WriteLine($"Min {propertyName} is: {propertySelector(minPlanet)} (planet: {minPlanet.Name})");
    }

    private IEnumerable<Planet> ToPlanet(List<Root>? root)
    {
        if(root is null)
        {
            throw new ArgumentNullException(nameof(root));
        }
        
        var planets = new List<Planet>();

        foreach(var planetDto in root)
        {
            Planet planet = (Planet)planetDto;
            planets.Add(planet);
        }

        return planets;
    }
}

public readonly record struct Planet
{
    public string Name { get; }
    public int? Diameter { get; }
    public int? SurfaceWater { get; }
    public int? Population { get; }

    public Planet(string name, int? diameter, int? surfaceWater, int? population)
    {
        if(name is null)
        {
            throw new ArgumentNullException($"{nameof(name)} is null");
        }

        Name = name;
        Diameter = diameter;
        SurfaceWater = surfaceWater;
        Population = population;
    }

    public static explicit operator Planet(Root planetDto)
    {
        var name = planetDto.name;
        int? diameter = ToIntOrNull(planetDto.diameter);
        int? surfaceWater = ToIntOrNull(planetDto.surface_water);
        int? population = ToIntOrNull(planetDto.population);

        return new Planet(name, diameter, surfaceWater, population);
    }

    private static int? ToIntOrNull(string input)
    {
        int? result = null;
        if (int.TryParse(input, out int resultParsed))
        {
            result = resultParsed;
        }

        return result;
    }
}
