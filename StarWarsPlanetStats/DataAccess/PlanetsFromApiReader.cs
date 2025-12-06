using StarWarsPlanetStats.ApiDataAccess;
using StarWarsPlanetStats.DTOs;
using System.Text.Json;

public class PlanetsFromApiReader : IPlanetsReader
{
    private readonly IApiDataReader _apiDataReader;
    private readonly IApiDataReader _secondaryApiDataReader;

    public PlanetsFromApiReader(IApiDataReader apiDataReader, IApiDataReader secondaryApiDataReader)
    {
        _apiDataReader = apiDataReader;
        _secondaryApiDataReader = secondaryApiDataReader;
    }

    public async Task<IEnumerable<Planet>> Read()
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
            json = await _secondaryApiDataReader.Read("https://swapi.dev/", "api/planets");
        }


        var root = JsonSerializer.Deserialize<List<Root>>(json);
        return ToPlanet(root);
    }
    private static IEnumerable<Planet> ToPlanet(List<Root>? root)
    {
        if (root is null)
        {
            throw new ArgumentNullException(nameof(root));
        }

        var planets = new List<Planet>();

        foreach (var planetDto in root)
        {
            Planet planet = (Planet)planetDto;
            planets.Add(planet);
        }

        return planets;
    }
}
