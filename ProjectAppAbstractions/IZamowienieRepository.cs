using ProjektApp.DataModel;

namespace ProjectApp.Abstractions
{
    public interface IZamowienieRepository
    {
        IQueryable<Zamowienie> Query();
        Zamowienie? Get(int id);
        void Add(Zamowienie entity);
        void Remove(Zamowienie entity);
    }
}