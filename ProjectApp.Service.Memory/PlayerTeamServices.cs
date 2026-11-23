using ProjectApp.Abstractions;
using ProjectApp.ServiceAbstractions;
using ProjektApp.DataModel;

namespace ProjectApp.Services;

public class PlayerService : IPlayerService
{
    private readonly IKlientRepository _players;

    public PlayerService(IKlientRepository players, IUnitOfWork uow)
    {
        _players = players;
    }

    public IReadOnlyList<Klient> Search(string? fraza = null)
    {
        var query = _players.Query();

        if (!string.IsNullOrWhiteSpace(fraza))
        {
            var searchPhrase = fraza.Trim();
            query = query.Where(p =>
                (p.Imie != null && p.Imie.Contains(searchPhrase, StringComparison.OrdinalIgnoreCase)) ||
                (p.Nazwisko != null && p.Nazwisko.Contains(searchPhrase, StringComparison.OrdinalIgnoreCase)) ||
                (p.Email != null && p.Email.Contains(searchPhrase, StringComparison.OrdinalIgnoreCase)));
        }

        return query.ToList();
    }
}

public class TeamService : ITeamService
{
    private readonly IMGRepository _teams;

    public TeamService(IMGRepository teams, IUnitOfWork uow)
    {
        _teams = teams;
    }

    public IReadOnlyList<MG> GetAll() => _teams.Query().ToList();
}
