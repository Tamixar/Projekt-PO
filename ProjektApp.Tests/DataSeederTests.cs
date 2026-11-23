using ProjectApp.Abstractions;
using ProjectApp.DataAccess.Memory;
using ProjectApp.DataAccess.Memory.Repositories;
using ProjectApp.ServiceAbstractions;
using ProjectApp.Services;

namespace ProjektApp.Tests;

public class DataSeederTests
{
    [Fact]
    public void Seed_AddsThreePlayersAndTwoTeams()
    {
        var db = new MemoryDbContext();
        var seeder = CreateSeeder(db);

        var result = seeder.Seed();

        Assert.Equal(3, db.Klienci.Count);
        Assert.Equal(2, db.MistrzowieGry.Count);
        Assert.NotEqual(Guid.Empty, result.Player1Id);
        Assert.NotEqual(Guid.Empty, result.TeamAId);
    }

    [Fact]
    public void Seed_IsIdempotent()
    {
        var db = new MemoryDbContext();
        var seeder = CreateSeeder(db);

        seeder.Seed();
        seeder.Seed();

        Assert.Equal(3, db.Klienci.Count);
        Assert.Equal(2, db.MistrzowieGry.Count);
    }

    [Fact]
    public void Seed_ReturnsIdsForSeededEntities()
    {
        var db = new MemoryDbContext();
        var seeder = CreateSeeder(db);

        var result = seeder.Seed();

        Assert.Contains(db.Klienci, k => k.Imie == "Jan" && k.Nazwisko == "Kowalski");
        Assert.Contains(db.Klienci, k => k.Imie == "Adam" && k.Nazwisko == "Nowak");
        Assert.Contains(db.Klienci, k => k.Imie == "Piotr" && k.Nazwisko == "Zieliński");
        Assert.Equal(2, db.MistrzowieGry.Count);
        Assert.Equal(db.MistrzowieGry[0].Id, ConvertGuidToId(result.TeamAId));
    }

    [Fact]
    public void Seed_CanBeUsedWithExistingData()
    {
        var db = new MemoryDbContext();
        db.Klienci.Add(new ProjektApp.DataModel.Klient("Maria", "Testowa", "maria@test.pl", "haslo") { Id = 42 });
        var seeder = CreateSeeder(db);

        seeder.Seed();

        Assert.Equal(4, db.Klienci.Count);
        Assert.Equal(42, db.Klienci.First(k => k.Email == "maria@test.pl").Id);
    }

    private static IDataSeeder CreateSeeder(MemoryDbContext db)
    {
        IKlientRepository klientRepository = new KlientRepositoryMemory(db);
        IMGRepository mgRepository = new MGRepositoryMemory(db);
        IUnitOfWork uow = new UnitOfWorkMemory(db);
        return new DataSeeder(klientRepository, mgRepository, uow);
    }

    private static int? ConvertGuidToId(Guid guid)
    {
        Span<byte> bytes = guid.ToByteArray();
        return bytes[0] | (bytes[1] << 8);
    }
}
