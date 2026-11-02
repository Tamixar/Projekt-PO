using ProjektApp.DataModel;

namespace ProjectApp.Abstractions
{
    public interface IPracownikRepository
    {
        IQueryable<Pracownik> Query();
        Pracownik? Get(int id);
        void Add(Pracownik entity);
        void Remove(Pracownik entity);
    }
}