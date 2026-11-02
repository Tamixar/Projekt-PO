using ProjectApp.Abstractions;
using ProjectApp.ServiceAbstractions;
using ProjektApp.DataModel;

namespace ProjectApp.Services;

public class SesjaRpgService : ISesjaRpgService
{
    private readonly ISesjaRepository _sesje;
    private readonly IMGRepository _mistrzowieGry;
    private readonly IKlientRepository _klienci;
    private readonly IUnitOfWork _uow;

    public SesjaRpgService(
        ISesjaRepository sesje,
        IMGRepository mistrzowieGry,
        IKlientRepository klienci,
        IUnitOfWork uow)
    {
        _sesje = sesje;
        _mistrzowieGry = mistrzowieGry;
        _klienci = klienci;
        _uow = uow;
    }

    public int UtworzSesje(string system, DateTime data, int mistrzGryId, int maxLiczbaGraczy)
    {
        var mg = _mistrzowieGry.Get(mistrzGryId);
        if (mg == null) throw new InvalidOperationException("Mistrz gry nie istnieje.");

        var sesja = new Sesja
        {
            Id = _sesje.Query().Any() ? _sesje.Query().Max(s => s.Id ?? 0) + 1 : 1,
            System = system,
            DataOdbycia = data,
            Prowadzacy = mg,
            LiczbaGraczy = maxLiczbaGraczy,
            Status = "Do przygotowania"
        };

        _sesje.Add(sesja);
        _uow.SaveChanges();
        return sesja.Id.Value;
    }

    public Sesja? Get(int id) => _sesje.Get(id);

    public IReadOnlyList<Sesja> GetAll() => _sesje.Query().ToList();

    public IReadOnlyList<Sesja> Search(string? system = null, DateTime? from = null, DateTime? to = null)
    {
        var query = _sesje.Query();

        if (!string.IsNullOrWhiteSpace(system))
            query = query.Where(s => s.System != null && s.System.Contains(system, StringComparison.OrdinalIgnoreCase));

        if (from.HasValue)
            query = query.Where(s => s.DataOdbycia >= from.Value);

        if (to.HasValue)
            query = query.Where(s => s.DataOdbycia <= to.Value);

        return query.ToList();
    }

    public bool ZapiszGracza(int sesjaId, int klientId)
    {
        var sesja = _sesje.Get(sesjaId);
        var klient = _klienci.Get(klientId);

        if (sesja == null || klient == null) return false;
        if (sesja.LiczbaGraczy.HasValue && sesja.Gracze.Count >= sesja.LiczbaGraczy.Value) return false;

        sesja.DodajGracza(klient);
        _uow.SaveChanges();
        return true;
    }

    public bool UsunGracza(int sesjaId, int klientId)
    {
        var sesja = _sesje.Get(sesjaId);
        var klient = _klienci.Get(klientId);

        if (sesja == null || klient == null) return false;

        sesja.UsunGracza(klient);
        _uow.SaveChanges();
        return true;
    }

    public bool ZmienStatus(int sesjaId, string nowyStatus)
    {
        var sesja = _sesje.Get(sesjaId);
        if (sesja == null) return false;

        sesja.Status = nowyStatus;
        _uow.SaveChanges();
        return true;
    }

    public bool Delete(int sesjaId)
    {
        var sesja = _sesje.Get(sesjaId);
        if (sesja == null) return false;

        _sesje.Remove(sesja);
        _uow.SaveChanges();
        return true;
    }
}