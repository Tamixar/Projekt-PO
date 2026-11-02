using ProjektApp.DataModel;

namespace ProjectApp.Abstractions
{
    public interface IGraPlanszowaRepository
    {
        IQueryable<GraPlanszowa> Query();
        GraPlanszowa? Get(int id);
        void Add(GraPlanszowa entity);
        void Remove(GraPlanszowa entity);
    }
}
