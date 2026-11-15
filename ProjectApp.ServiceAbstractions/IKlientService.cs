using ProjektApp.DataModel;

namespace ProjectApp.ServiceAbstractions;

/// <summary>Operacje aplikacyjne dotyczące klientów.</summary>
public interface IKlientService
{
    int UtworzKlienta(string imie, string nazwisko, string email, string haslo);
    Klient? Get(int id);
    IReadOnlyList<Klient> GetAll();
    IReadOnlyList<Klient> Search(string? fraza = null);
    bool AktualizujKlienta(int id, string imie, string nazwisko, string email);
    bool Delete(int id);
}
