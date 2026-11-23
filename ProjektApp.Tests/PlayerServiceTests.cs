using ProjectApp.ServiceAbstractions;

namespace ProjektApp.Tests;

public class PlayerServiceTests : IClassFixture<InMemoryServicesFixture>
{
    private readonly IPlayerService _playerSvc;

    public PlayerServiceTests(InMemoryServicesFixture fixture)
    {
        _playerSvc = fixture.PlayerService;
    }

    [Fact]
    public void Search_ShouldFindJanKowalski()
    {
        var results = _playerSvc.Search("Jan");

        Assert.Contains(results, p => p.FullName == "Jan Kowalski");
    }
}
