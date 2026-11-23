using ProjectApp.ServiceAbstractions;

namespace ProjektApp.Tests;

public class TeamServiceTests : IClassFixture<InMemoryServicesFixture>
{
    private readonly ITeamService _teamSvc;

    public TeamServiceTests(InMemoryServicesFixture fixture)
    {
        _teamSvc = fixture.TeamService;
    }

    [Fact]
    public void GetAll_ShouldReturnSeededTeams()
    {
        var teams = _teamSvc.GetAll();

        Assert.NotEmpty(teams);
        Assert.Contains(teams, t => t.Name == "FC Demo");
        Assert.Contains(teams, t => t.Name == "AC Sample");
    }
}
