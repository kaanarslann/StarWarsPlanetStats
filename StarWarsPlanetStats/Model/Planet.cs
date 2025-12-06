using StarWarsPlanetStats.DTOs;

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
