using ProjectApp.Abstractions;
using ProjectApp.ServiceAbstractions;
using ProjektApp.DataModel;

namespace ProjectApp.Services;

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
