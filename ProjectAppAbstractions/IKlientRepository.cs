using ProjektApp.DataModel;

namespace ProjectApp.Abstractions
{
    public interface IKlientRepository
    {
        IQueryable<Klient> Query();
        Klient? Get(int id);
        void Add(Klient entity);
        void Remove(Klient entity);
    }
}