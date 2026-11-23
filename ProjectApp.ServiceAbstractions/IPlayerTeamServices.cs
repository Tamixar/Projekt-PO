using ProjektApp.DataModel;

namespace ProjectApp.ServiceAbstractions;

public interface IPlayerService
{
    IReadOnlyList<Klient> Search(string? fraza = null);
}

public interface ITeamService
{
    IReadOnlyList<MG> GetAll();
}
