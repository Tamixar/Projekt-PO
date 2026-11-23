using ProjectApp.Abstractions;
using ProjectApp.DataAccess.Memory;
using ProjectApp.DataAccess.Memory.Repositories;
using ProjectApp.ServiceAbstractions;
using ProjectApp.Services;

namespace ProjectApp.Services;

public class MGService : IMGService
{
    private readonly IMGRepository _mistrzowieGry;

    public MGService(IMGRepository mistrzowieGry)
    {
        _mistrzowieGry = mistrzowieGry;
    }

    public IReadOnlyList<ProjektApp.DataModel.MG> GetAll() => _mistrzowieGry.Query()
        .OrderBy(mg => mg.Nazwisko)
        .ThenBy(mg => mg.Imie)
        .ToList();
}
