using ProjektApp.DataModel;

namespace ProjectApp.ServiceAbstractions;

public interface ISklepService
{
    int UtworzZamowienie(int klientId, Dictionary<int, int> pozycjeKoszyka);
    Zamowienie? GetZamowienie(int id);
    IReadOnlyList<Zamowienie> GetZamowieniaKlienta(int klientId);
    IReadOnlyList<GraPlanszowa> GetGryPlanszowe();
    IReadOnlyList<Podrecznik> GetPodreczniki();
    bool ZaktualizujStanMagazynowy(int towarId, int nowaIlosc);
    bool UsunTowar(int towarId);
}