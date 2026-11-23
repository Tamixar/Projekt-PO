using ProjectApp.ServiceAbstractions;

namespace ProjektApp.Tests;

public class KlientServiceTests : IClassFixture<InMemoryServicesFixture>
{
    private readonly IKlientService _klientService;

    public KlientServiceTests(InMemoryServicesFixture fixture)
    {
        _klientService = fixture.KlientService;
    }

    [Fact]
    public void Search_ShouldFindJanKowalski()
    {
        var results = _klientService.Search("Jan");

        Assert.Contains(results, klient => klient.Imie == "Jan" && klient.Nazwisko == "Kowalski");
    }
}
