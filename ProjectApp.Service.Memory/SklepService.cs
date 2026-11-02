using ProjectApp.Abstractions;
using ProjectApp.ServiceAbstractions;
using ProjektApp.DataModel;

namespace ProjectApp.Services;

public class SklepService : ISklepService
{
    private readonly IGraPlanszowaRepository _gry;
    private readonly IPodrecznikRepository _podreczniki;
    private readonly IZamowienieRepository _zamowienia;
    private readonly IKlientRepository _klienci;
    private readonly IUnitOfWork _uow;

    public SklepService(
        IGraPlanszowaRepository gry,
        IPodrecznikRepository podreczniki,
        IZamowienieRepository zamowienia,
        IKlientRepository klienci,
        IUnitOfWork uow)
    {
        _gry = gry;
        _podreczniki = podreczniki;
        _zamowienia = zamowienia;
        _klienci = klienci;
        _uow = uow;
    }

    public int UtworzZamowienie(int klientId, Dictionary<int, int> pozycjeKoszyka)
    {
        var klient = _klienci.Get(klientId);
        if (klient == null) throw new InvalidOperationException("Klient nie istnieje.");

        var pozycje = new Dictionary<Towar, int>();
        decimal suma = 0;

        foreach (var wpis in pozycjeKoszyka)
        {
            Towar? towar = (Towar?)_gry.Get(wpis.Key) ?? _podreczniki.Get(wpis.Key);
            if (towar == null || !towar.CzyDostepny(wpis.Value))
                throw new InvalidOperationException($"Towar o ID {wpis.Key} jest niedostępny w żądanej liczbie.");

            towar.StanMagazynowy -= wpis.Value;
            suma += (towar.Cena ?? 0) * wpis.Value;
            pozycje.Add(towar, wpis.Value);
        }

        var zamowienie = new Zamowienie
        {
            Id = _zamowienia.Query().Any() ? _zamowienia.Query().Max(z => z.Id) + 1 : 1,
            DataZamowienia = DateTime.Now,
            WartoscCalkowita = suma,
            Pozycje = pozycje
        };

        klient.Zamowienia.Add(zamowienie);
        _zamowienia.Add(zamowienie);
        _uow.SaveChanges();

        return zamowienie.Id;
    }

    public Zamowienie? GetZamowienie(int id) => _zamowienia.Get(id);

    public IReadOnlyList<Zamowienie> GetZamowieniaKlienta(int klientId)
    {
        var klient = _klienci.Get(klientId);
        return klient?.Zamowienia ?? new List<Zamowienie>();
    }

    public IReadOnlyList<GraPlanszowa> GetGryPlanszowe() => _gry.Query().ToList();

    public IReadOnlyList<Podrecznik> GetPodreczniki() => _podreczniki.Query().ToList();

    public bool ZaktualizujStanMagazynowy(int towarId, int nowaIlosc)
    {
        Towar? towar = (Towar?)_gry.Get(towarId) ?? _podreczniki.Get(towarId);
        if (towar == null) return false;

        towar.StanMagazynowy = nowaIlosc;
        _uow.SaveChanges();
        return true;
    }

    public bool UsunTowar(int towarId)
    {
        var gra = _gry.Get(towarId);
        if (gra != null)
        {
            _gry.Remove(gra);
            _uow.SaveChanges();
            return true;
        }

        var podrecznik = _podreczniki.Get(towarId);
        if (podrecznik != null)
        {
            _podreczniki.Remove(podrecznik);
            _uow.SaveChanges();
            return true;
        }

        return false;
    }
}