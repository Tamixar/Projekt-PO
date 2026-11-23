namespace ProjectApp.ServiceAbstractions;

public interface IDataSeeder
{
    SeedResult Seed();
}

public sealed class SeedResult
{
    public int? MG1Id { get; init; }
    public int? MG2Id { get; init; }
    public int? Klient1Id { get; init; }
    public int? Klient2Id { get; init; }
    public int? Klient3Id { get; init; }
}
