using ProjectApp.ServiceAbstractions;

namespace ProjektApp.Tests;

public class TeamServiceTests : IClassFixture<InMemoryServicesFixture>
{
    private readonly IMGService _mgService;

    public TeamServiceTests(InMemoryServicesFixture fixture)
    {
        _mgService = fixture.MGService;
    }

    [Fact]
    public void GetAll_ShouldReturnSeededGameMasters()
    {
        var gameMasters = _mgService.GetAll();
        Assert.Equal(2, gameMasters.Count);
        Assert.Contains(gameMasters, mg => mg.Imie == "FC" && mg.Nazwisko == "Demo");
        Assert.Contains(gameMasters, mg => mg.Imie == "AC" && mg.Nazwisko == "Sample");
    }
}
