namespace ProjectApp.ServiceAbstractions;

public interface IDataSeeder
{
    SeedResult Seed();
}

public sealed class SeedResult
{
    public Guid TeamAId { get; init; }
    public Guid TeamBId { get; init; }
    public Guid Player1Id { get; init; }
    public Guid Player2Id { get; init; }
    public Guid Player3Id { get; init; }
}
