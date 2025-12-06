public class StarWarsPlanetsStatsApp
{
    private readonly IPlanetsReader _planetsReader;
    private readonly IPlanetStatistics _planetsStatistics;

    public StarWarsPlanetsStatsApp(IPlanetsReader planetsReader, IPlanetStatistics planetsStatistics)
    {
        _planetsReader = planetsReader;
        _planetsStatistics = planetsStatistics;
    }

    public async Task Run()
    {
        var planets = await _planetsReader.Read();

        foreach(var planet in planets)
        {
            Console.WriteLine(planet);
        }
        
        _planetsStatistics.Analyze(planets);
    }
}
