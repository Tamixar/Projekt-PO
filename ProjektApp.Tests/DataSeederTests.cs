using ProjectApp.Abstractions;
using ProjectApp.DataAccess.Memory;
using ProjectApp.DataAccess.Memory.Repositories;
using ProjectApp.ServiceAbstractions;
using ProjectApp.Services;
using ProjektApp.DataModel;

namespace ProjektApp.Tests;

public class DataSeederTests
{
    [Fact]
    public void Seed_AddsThreeKlienciAndTwoGameMasters()
    {
        var db = new MemoryDbContext();
        var seeder = CreateSeeder(db);

        var result = seeder.Seed();

        Assert.Equal(3, db.Klienci.Count);
        Assert.Equal(2, db.MistrzowieGry.Count);
        Assert.NotNull(result.Klient1Id);
        Assert.NotNull(result.MG1Id);
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
    public void Seed_PreservesExistingKlienci()
    {
        var db = new MemoryDbContext();
        db.Klienci.Add(new Klient("Maria", "Testowa", "maria@test.pl", "haslo") { Id = 42 });
        var seeder = CreateSeeder(db);

        seeder.Seed();

        Assert.Equal(4, db.Klienci.Count);
        Assert.Equal(42, db.Klienci.Single(k => k.Email == "maria@test.pl").Id);
    }

    private static IDataSeeder CreateSeeder(MemoryDbContext db)
    {
        IKlientRepository klientRepository = new KlientRepositoryMemory(db);
        IMGRepository mgRepository = new MGRepositoryMemory(db);
        IUnitOfWork uow = new UnitOfWorkMemory(db);
        return new DataSeeder(klientRepository, mgRepository, uow);
    }
}
