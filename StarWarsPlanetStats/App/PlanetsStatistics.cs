public class PlanetsStatistics : IPlanetStatistics
{
    public void Analyze(IEnumerable<Planet> planets)
    {
        Console.WriteLine();

        var propertyNamesToSelector = new Dictionary<string, Func<Planet, int?>>
        {
            ["population"] = planet => planet.Population,
            ["diameter"] = planet => planet.Diameter,
            ["surface water"] = planet => planet.SurfaceWater
        };

        Console.WriteLine("The statistics of which property would you like to see?");
        Console.WriteLine(string.Join(Environment.NewLine, propertyNamesToSelector.Keys));

        var userInput = Console.ReadLine();

        if (userInput is null || !propertyNamesToSelector.ContainsKey(userInput))
        {
            Console.WriteLine("Invalid choice!");
        }
        else
        {
            ShowStatistics(planets, userInput, propertyNamesToSelector[userInput]);
        }
    }
    private static void ShowStatistics(IEnumerable<Planet> planets, string propertyName, Func<Planet, int?> propertySelector)
    {
        var maxPlanet = planets.MaxBy(propertySelector);
        Console.WriteLine($"Max {propertyName} is: {propertySelector(maxPlanet)} (planet: {maxPlanet.Name})");

        var minPlanet = planets.MinBy(propertySelector);
        Console.WriteLine($"Min {propertyName} is: {propertySelector(minPlanet)} (planet: {minPlanet.Name})");
    }
}
