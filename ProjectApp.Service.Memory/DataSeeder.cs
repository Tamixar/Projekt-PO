using ProjectApp.Abstractions;
using ProjectApp.ServiceAbstractions;
using ProjektApp.DataModel;

namespace ProjectApp.Services;

public class DataSeeder : IDataSeeder
{
    private readonly IKlientRepository _klientRepository;
    private readonly IMGRepository _mgRepository;
    private readonly IUnitOfWork _uow;

    public DataSeeder(IKlientRepository klientRepository, IMGRepository mgRepository, IUnitOfWork uow)
    {
        _klientRepository = klientRepository;
        _mgRepository = mgRepository;
        _uow = uow;
    }

    public SeedResult Seed()
    {
        var klient1 = EnsureKlient("Jan", "Kowalski", "jan.kowalski@example.com", 1);
        var klient2 = EnsureKlient("Adam", "Nowak", "adam.nowak@example.com", 2);
        var klient3 = EnsureKlient("Piotr", "Zieliński", "piotr.zielinski@example.com", 3);

        var mg1 = EnsureMG("FC", "Demo", "fc.demo@example.com", 1);
        var mg2 = EnsureMG("AC", "Sample", "ac.sample@example.com", 2);

        _uow.SaveChanges();

        return new SeedResult
        {
            MG1Id = mg1.Id,
            MG2Id = mg2.Id,
            Klient1Id = klient1.Id,
            Klient2Id = klient2.Id,
            Klient3Id = klient3.Id
        };
    }

    private Klient EnsureKlient(string imie, string nazwisko, string email, int fallbackId)
    {
        var existing = _klientRepository.Query().FirstOrDefault(k =>
            k.Email != null && k.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        if (existing != null)
            return existing;

        var nextId = _klientRepository.Query().Any()
            ? _klientRepository.Query().Max(k => k.Id ?? 0) + 1
            : fallbackId;

        var klient = new Klient(imie, nazwisko, email, "Seed123!") { Id = nextId };
        _klientRepository.Add(klient);
        return klient;
    }

    private MG EnsureMG(string imie, string nazwisko, string email, int fallbackId)
    {
        var existing = _mgRepository.Query().FirstOrDefault(mg =>
            mg.Email != null && mg.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        if (existing != null)
            return existing;

        var nextId = _mgRepository.Query().Any()
            ? _mgRepository.Query().Max(mg => mg.Id ?? 0) + 1
            : fallbackId;

        var mg = new MG(imie, nazwisko, email, "Seed123!", "Mistrz Gry")
        {
            Id = nextId,
            Systemy = new List<string> { "D&D", "Warhammer" }
        };
        _mgRepository.Add(mg);
        return mg;
    }
}

public class MGService : IMGService
{
    private readonly IMGRepository _mistrzowieGry;

    public MGService(IMGRepository mistrzowieGry)
    {
        _mistrzowieGry = mistrzowieGry;
    }

    public IReadOnlyList<MG> GetAll() => _mistrzowieGry.Query()
        .OrderBy(mg => mg.Nazwisko)
        .ThenBy(mg => mg.Imie)
        .ToList();
}
