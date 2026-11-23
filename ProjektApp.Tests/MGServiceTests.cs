using ProjectApp.ServiceAbstractions;

namespace ProjektApp.Tests;

public class MGServiceTests : IClassFixture<InMemoryServicesFixture>
{
    private readonly IMGService _mgService;

    public MGServiceTests(InMemoryServicesFixture fixture)
    {
        _mgService = fixture.MGService;
    }

    [Fact]
    public void GetAll_ShouldReturnSeededGameMasters()
    {
        var mistrzowieGry = _mgService.GetAll();

        Assert.Equal(2, mistrzowieGry.Count);
        Assert.Contains(mistrzowieGry, mg => mg.Imie == "FC" && mg.Nazwisko == "Demo");
        Assert.Contains(mistrzowieGry, mg => mg.Imie == "AC" && mg.Nazwisko == "Sample");
    }
}
