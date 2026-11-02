using ProjektApp.DataModel;

namespace ProjectApp.ServiceAbstractions;

public interface ISesjaRpgService
{
    int UtworzSesje(string system, DateTime data, int mistrzGryId, int maxLiczbaGraczy);
    Sesja? Get(int id);
    IReadOnlyList<Sesja> GetAll();
    IReadOnlyList<Sesja> Search(string? system = null, DateTime? from = null, DateTime? to = null);
    bool ZapiszGracza(int sesjaId, int klientId);
    bool UsunGracza(int sesjaId, int klientId);
    bool ZmienStatus(int sesjaId, string nowyStatus);
    bool Delete(int sesjaId);
}
