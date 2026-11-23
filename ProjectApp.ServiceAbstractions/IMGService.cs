using ProjektApp.DataModel;

namespace ProjectApp.ServiceAbstractions;

public interface IMGService
{
    IReadOnlyList<MG> GetAll();
}
