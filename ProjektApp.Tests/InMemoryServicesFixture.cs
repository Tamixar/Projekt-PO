using ProjectApp.Abstractions;
using ProjectApp.DataAccess.Memory;
using ProjectApp.DataAccess.Memory.Repositories;
using ProjectApp.ServiceAbstractions;
using ProjectApp.Services;

namespace ProjektApp.Tests;

public class InMemoryServicesFixture
{
    public MemoryDbContext Db { get; }
    public IKlientService KlientService { get; }
    public IMGService MGService { get; }

    public InMemoryServicesFixture()
    {
        Db = new MemoryDbContext();

        IKlientRepository klientRepository = new KlientRepositoryMemory(Db);
        IMGRepository mgRepository = new MGRepositoryMemory(Db);
        IUnitOfWork uow = new UnitOfWorkMemory(Db);

        IDataSeeder seeder = new DataSeeder(klientRepository, mgRepository, uow);
        seeder.Seed();

        KlientService = new KlientService(klientRepository, uow);
        MGService = new MGService(mgRepository);
    }
}
