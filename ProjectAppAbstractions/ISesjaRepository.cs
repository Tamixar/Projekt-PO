using ProjektApp.DataModel;

namespace ProjectApp.Abstractions
{
    public interface ISesjaRepository
    {
        IQueryable<Sesja> Query();
        Sesja? Get(int id);
        void Add(Sesja entity);
        void Remove(Sesja entity);
    }
}