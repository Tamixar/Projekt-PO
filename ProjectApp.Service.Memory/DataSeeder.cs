using ProjectApp.Abstractions;
using ProjectApp.ServiceAbstractions;
using ProjektApp.DataModel;

namespace ProjectApp.Services;

public class DataSeeder : IDataSeeder
{
    private readonly IKlientRepository _klientRepository;
    private readonly IMGRepository _mgRepository;
    private readonly IUnitOfWork _uow;

    public DataSeeder(
        IKlientRepository klientRepository,
        IMGRepository mgRepository,
        IUnitOfWork uow)
    {
        _klientRepository = klientRepository;
        _mgRepository = mgRepository;
        _uow = uow;
    }

    public SeedResult Seed()
    {
        var player1 = EnsurePlayer("Jan", "Kowalski", "jan.kowalski@example.com", 1);
        var player2 = EnsurePlayer("Adam", "Nowak", "adam.nowak@example.com", 2);
        var player3 = EnsurePlayer("Piotr", "Zieliński", "piotr.zielinski@example.com", 3);

        var teamA = EnsureTeam("FC", "Demo", "fc.demo@example.com", 1);
        var teamB = EnsureTeam("AC", "Sample", "ac.sample@example.com", 2);

        _uow.SaveChanges();

        return new SeedResult
        {
            TeamAId = GuidFromId(teamA.Id),
            TeamBId = GuidFromId(teamB.Id),
            Player1Id = GuidFromId(player1.Id),
            Player2Id = GuidFromId(player2.Id),
            Player3Id = GuidFromId(player3.Id)
        };
    }

    private Klient EnsurePlayer(string imie, string nazwisko, string email, int fallbackId)
    {
        var existing = _klientRepository.Query()
            .FirstOrDefault(k => k.Email != null && k.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

        if (existing != null)
            return existing;

        var nextId = _klientRepository.Query().Any()
            ? _klientRepository.Query().Max(k => k.Id ?? 0) + 1
            : fallbackId;

        var player = new Klient(imie, nazwisko, email, "Seed123!")
        {
            Id = nextId
        };

        _klientRepository.Add(player);
        return player;
    }

    private MG EnsureTeam(string imie, string nazwisko, string email, int fallbackId)
    {
        var existing = _mgRepository.Query()
            .FirstOrDefault(m => m.Email != null && m.Email.Equals(email, StringComparison.OrdinalIgnoreCase));

        if (existing != null)
            return existing;

        var nextId = _mgRepository.Query().Any()
            ? _mgRepository.Query().Max(m => m.Id ?? 0) + 1
            : fallbackId;

        var team = new MG(imie, nazwisko, email, "Seed123!", "Mistrz Gry")
        {
            Id = nextId,
            Systemy = new List<string> { "D&D", "Warhammer" }
        };

        _mgRepository.Add(team);
        return team;
    }

    private static Guid GuidFromId(int? id)
    {
        Span<byte> bytes = stackalloc byte[16];
        bytes.Clear();
        bytes[0] = (byte)((id ?? 0) & 0xFF);
        bytes[1] = (byte)(((id ?? 0) >> 8) & 0xFF);
        return new Guid(bytes);
    }
}
