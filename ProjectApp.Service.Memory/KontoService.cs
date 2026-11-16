using ProjectApp.Abstractions;
using ProjectApp.ServiceAbstractions;
using ProjektApp.DataModel;

namespace ProjectApp.Services;

public class KontoService : IKontoService
{
    private readonly IKlientRepository _klienci;
    private readonly IPracownikRepository _pracownicy;
    private readonly IMGRepository _mistrzowieGry;
    private readonly IUnitOfWork _uow;

    private Uzytkownik? _zalogowanyUzytkownik;

    public KontoService(
        IKlientRepository klienci,
        IPracownikRepository pracownicy,
        IMGRepository mistrzowieGry,
        IUnitOfWork uow)
    {
        _klienci = klienci;
        _pracownicy = pracownicy;
        _mistrzowieGry = mistrzowieGry;
        _uow = uow;
    }

    public Uzytkownik? ZalogowanyUzytkownik => _zalogowanyUzytkownik;

    // --- Rejestracja kont dla wszystkich trzech ról (pkt 2.1, 3.1, 3.2, 3.5, 3.6) ---

    public int ZarejestrujKlienta(string imie, string nazwisko, string email, string haslo)
    {
        WalidujDanePodstawowe(imie, nazwisko, email, haslo);
        SprawdzUnikalnoscEmail(email);

        var klient = new Klient(imie.Trim(), nazwisko.Trim(), email.Trim(), haslo)
        {
            Id = GenerujIdKlienta()
        };

        _klienci.Add(klient);
        _uow.SaveChanges();
        return klient.Id!.Value;
    }

    public int ZarejestrujPracownika(
        string imie,
        string nazwisko,
        string email,
        string haslo,
        string stanowisko,
        decimal pensja,
        double ocena = 0.0)
    {
        WalidujDanePodstawowe(imie, nazwisko, email, haslo);
        if (string.IsNullOrWhiteSpace(stanowisko))
            throw new ArgumentException("Stanowisko nie może być puste.", nameof(stanowisko));
        if (pensja < 0)
            throw new ArgumentException("Pensja nie może być ujemna.", nameof(pensja));

        SprawdzUnikalnoscEmail(email);

        var pracownik = new Pracownik(imie.Trim(), nazwisko.Trim(), email.Trim(), haslo)
        {
            Id = GenerujIdPracownika(),
            Stanowisko = stanowisko.Trim(),
            Pensja = pensja,
            Ocena = ocena
        };

        _pracownicy.Add(pracownik);
        _uow.SaveChanges();
        return pracownik.Id!.Value;
    }

    public int ZarejestrujMistrzaGry(
        string imie,
        string nazwisko,
        string email,
        string haslo,
        string stanowisko,
        decimal pensja,
        List<string> systemy,
        decimal bonus = 0m,
        double ocena = 0.0)
    {
        WalidujDanePodstawowe(imie, nazwisko, email, haslo);
        if (string.IsNullOrWhiteSpace(stanowisko))
            throw new ArgumentException("Stanowisko nie może być puste.", nameof(stanowisko));
        if (pensja < 0 || bonus < 0)
            throw new ArgumentException("Pensja i bonus nie mogą być ujemne.");

        SprawdzUnikalnoscEmail(email);

        var mg = new MistrzGry(imie.Trim(), nazwisko.Trim(), email.Trim(), haslo)
        {
            Id = GenerujIdMistrzaGry(),
            Stanowisko = stanowisko.Trim(),
            Pensja = pensja,
            Ocena = ocena,
            Systemy = systemy ?? new List<string>(),
            Bonus = bonus
        };

        _mistrzowieGry.Add(mg);
        _uow.SaveChanges();
        return mg.Id!.Value;
    }

    // --- Logowanie, wylogowanie i wgląd we własny profil (pkt 2.1, 3.1) ---

    public Uzytkownik Zaloguj(string email, string haslo)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(haslo))
            throw new ArgumentException("E-mail i hasło są wymagane.");

        var szukanyEmail = email.Trim();

        Uzytkownik? uzytkownik =
            (Uzytkownik?)_klienci.Query().FirstOrDefault(k =>
                string.Equals(k.Email, szukanyEmail, StringComparison.OrdinalIgnoreCase))
            ?? (Uzytkownik?)_mistrzowieGry.Query().FirstOrDefault(mg =>
                string.Equals(mg.Email, szukanyEmail, StringComparison.OrdinalIgnoreCase))
            ?? _pracownicy.Query().FirstOrDefault(p =>
                string.Equals(p.Email, szukanyEmail, StringComparison.OrdinalIgnoreCase));

        if (uzytkownik == null || !uzytkownik.Zaloguj(haslo))
            throw new UnauthorizedAccessException("Nieprawidłowy adres e-mail lub hasło.");

        _zalogowanyUzytkownik = uzytkownik;
        return uzytkownik;
    }

    public void Wyloguj()
    {
        _zalogowanyUzytkownik?.Wyloguj();
        _zalogowanyUzytkownik = null;
    }

    public Uzytkownik? PobierzProfilZalogowanego() => _zalogowanyUzytkownik;

    // --- Weryfikacja uprawnień i ról (pkt 2.1, 2.3, 2.4) ---

    public string? PobierzRoleZalogowanego() => _zalogowanyUzytkownik switch
    {
        MistrzGry => "MistrzGry",
        Pracownik => "Pracownik",
        Klient => "Klient",
        _ => null
    };

    public bool CzyMaUprawnieniaPracownika() =>
        _zalogowanyUzytkownik is Pracownik; // MistrzGry dziedziczy po Pracownik

    public bool CzyMaUprawnieniaMistrzaGry() =>
        _zalogowanyUzytkownik is MistrzGry;

    // --- Zarządzanie pracownikami i MG ---

    public Pracownik? GetPracownik(int id) => _pracownicy.Get(id);

    public IReadOnlyList<Pracownik> GetAllPracownicy() => _pracownicy.Query()
        .OrderBy(p => p.Nazwisko)
        .ThenBy(p => p.Imie)
        .ToList();

    public MistrzGry? GetMistrzGry(int id) => _mistrzowieGry.Get(id);

    public IReadOnlyList<MistrzGry> GetAllMistrzowieGry() => _mistrzowieGry.Query()
        .OrderBy(mg => mg.Nazwisko)
        .ThenBy(mg => mg.Imie)
        .ToList();

    public bool UsunKontoPracownika(int id)
    {
        var pracownik = _pracownicy.Get(id);
        if (pracownik == null) return false;

        _pracownicy.Remove(pracownik);
        _uow.SaveChanges();
        return true;
    }

    public bool UsunKontoMistrzaGry(int id)
    {
        var mg = _mistrzowieGry.Get(id);
        if (mg == null) return false;

        _mistrzowieGry.Remove(mg);
        _uow.SaveChanges();
        return true;
    }

    // --- Metody pomocnicze ---

    private void SprawdzUnikalnoscEmail(string email)
    {
        var szukany = email.Trim();
        bool zajety =
            _klienci.Query().Any(k => string.Equals(k.Email, szukany, StringComparison.OrdinalIgnoreCase)) ||
            _pracownicy.Query().Any(p => string.Equals(p.Email, szukany, StringComparison.OrdinalIgnoreCase)) ||
            _mistrzowieGry.Query().Any(mg => string.Equals(mg.Email, szukany, StringComparison.OrdinalIgnoreCase));

        if (zajety)
            throw new InvalidOperationException("Konto z takim adresem e-mail już istnieje w systemie.");
    }

    private int GenerujIdKlienta() =>
        _klienci.Query().Any() ? _klienci.Query().Max(k => k.Id ?? 0) + 1 : 1;

    private int GenerujIdPracownika() =>
        _pracownicy.Query().Any() ? _pracownicy.Query().Max(p => p.Id ?? 0) + 1 : 1;

    private int GenerujIdMistrzaGry() =>
        _mistrzowieGry.Query().Any() ? _mistrzowieGry.Query().Max(mg => mg.Id ?? 0) + 1 : 1;

    private static void WalidujDanePodstawowe(string imie, string nazwisko, string email, string haslo)
    {
        if (string.IsNullOrWhiteSpace(imie))
            throw new ArgumentException("Imię nie może być puste.", nameof(imie));
        if (string.IsNullOrWhiteSpace(nazwisko))
            throw new ArgumentException("Nazwisko nie może być puste.", nameof(nazwisko));
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("E-mail nie może być pusty.", nameof(email));
        if (string.IsNullOrWhiteSpace(haslo))
            throw new ArgumentException("Hasło nie może być puste.", nameof(haslo));
    }
}
