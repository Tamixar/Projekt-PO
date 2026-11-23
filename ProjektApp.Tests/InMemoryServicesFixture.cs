using ProjectApp.Abstractions;
using ProjectApp.DataAccess.Memory;
using ProjectApp.DataAccess.Memory.Repositories;
using ProjectApp.ServiceAbstractions;
using ProjectApp.Services;

namespace ProjektApp.Tests;

public class InMemoryServicesFixture
{
    private readonly DataSeeder _dataSeeder;

    public MemoryDbContext Db { get; }
    public IPlayerService PlayerService { get; }
    public ITeamService TeamService { get; }

    public InMemoryServicesFixture()
    {
        Db = new MemoryDbContext();

        IKlientRepository teamRepo = new KlientRepositoryMemory(Db);
        IMGRepository playerRepo = new MGRepositoryMemory(Db);
        IUnitOfWork uow = new UnitOfWorkMemory(Db);

        _dataSeeder = new DataSeeder(teamRepo, playerRepo, uow);

        PlayerService = new PlayerService(teamRepo, uow);
        TeamService = new TeamService(playerRepo, uow);

        _dataSeeder.Seed();
    }
}
