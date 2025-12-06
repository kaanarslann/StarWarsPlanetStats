public interface IPlanetsReader
{
    Task<IEnumerable<Planet>> Read();
}
