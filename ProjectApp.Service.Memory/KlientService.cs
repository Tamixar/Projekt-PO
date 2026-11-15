using ProjectApp.Abstractions;
using ProjectApp.ServiceAbstractions;
using ProjektApp.DataModel;

namespace ProjectApp.Services;

/// <summary>Implementacja usług klienta dla pamięciowego repozytorium.</summary>
public class KlientService : IKlientService
{
    private readonly IKlientRepository _klienci;
    private readonly IUnitOfWork _uow;

    public KlientService(IKlientRepository klienci, IUnitOfWork uow)
    {
        _klienci = klienci;
        _uow = uow;
    }

    public int Utworz(string imie, string nazwisko, string email, string haslo)
    {
        WalidujWymagane(imie, nameof(imie));
        WalidujWymagane(nazwisko, nameof(nazwisko));
        WalidujWymagane(email, nameof(email));
        WalidujWymagane(haslo, nameof(haslo));

        if (EmailZajety(email))
            throw new InvalidOperationException("Klient z takim adresem e-mail już istnieje.");

        var klient = new Klient(imie.Trim(), nazwisko.Trim(), email.Trim(), haslo)
        {
            Id = _klienci.Query().Any() ? _klienci.Query().Max(k => k.Id ?? 0) + 1 : 1
        };

        _klienci.Add(klient);
        _uow.SaveChanges();
        return klient.Id!.Value;
    }

    public Klient? Get(int id) => _klienci.Get(id);

    public IReadOnlyList<Klient> GetAll() => _klienci.Query()
        .OrderBy(k => k.Nazwisko)
        .ThenBy(k => k.Imie)
        .ToList();

    public IReadOnlyList<Klient> Search(string? fraza = null)
    {
        var query = _klienci.Query();
        if (!string.IsNullOrWhiteSpace(fraza))
        {
            var szukanaFraza = fraza.Trim();
            query = query.Where(k =>
                (k.Imie != null && k.Imie.Contains(szukanaFraza, StringComparison.OrdinalIgnoreCase)) ||
                (k.Nazwisko != null && k.Nazwisko.Contains(szukanaFraza, StringComparison.OrdinalIgnoreCase)) ||
                (k.Email != null && k.Email.Contains(szukanaFraza, StringComparison.OrdinalIgnoreCase)));
        }

        return query.OrderBy(k => k.Nazwisko).ThenBy(k => k.Imie).ToList();
    }

    public bool AktualizujDane(int id, string imie, string nazwisko, string email)
    {
        WalidujWymagane(imie, nameof(imie));
        WalidujWymagane(nazwisko, nameof(nazwisko));
        WalidujWymagane(email, nameof(email));

        var klient = _klienci.Get(id);
        if (klient is null) return false;

        var nowyEmail = email.Trim();
        if (_klienci.Query().Any(k => k.Id != id &&
            string.Equals(k.Email, nowyEmail, StringComparison.OrdinalIgnoreCase)))
            throw new InvalidOperationException("Klient z takim adresem e-mail już istnieje.");

        klient.Imie = imie.Trim();
        klient.Nazwisko = nazwisko.Trim();
        klient.Email = nowyEmail;
        _uow.SaveChanges();
        return true;
    }

    public bool Delete(int id)
    {
        var klient = _klienci.Get(id);
        if (klient is null) return false;

        _klienci.Remove(klient);
        _uow.SaveChanges();
        return true;
    }

    private bool EmailZajety(string email) => _klienci.Query().Any(k =>
        string.Equals(k.Email, email.Trim(), StringComparison.OrdinalIgnoreCase));

    private static void WalidujWymagane(string wartosc, string nazwa)
    {
        if (string.IsNullOrWhiteSpace(wartosc))
            throw new ArgumentException("Wartość nie może być pusta.", nazwa);
    }
}
