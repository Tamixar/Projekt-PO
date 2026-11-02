using ProjektApp.DataModel;

namespace ProjectApp.DataAccess.Memory
{
    public class MemoryDbContext
    {

        public List<Klient> Klienci { get; } = new();
        public List<Pracownik> Pracownicy { get; } = new();
        public List<MG> MistrzowieGry { get; } = new();
        public List<GraPlanszowa> GryPlanszowe { get; } = new();
        public List<Podrecznik> Podreczniki { get; } = new();
        public List<Zamowienie> Zamowienia { get; } = new();
        public List<Sesja> Sesje { get; } = new();

        public int SaveChanges() => 0;
    }
}
