using ProjektApp.DataModel;

namespace ProjectApp.Abstractions
{
    public interface IMGRepository
    {
        IQueryable<MG> Query();
        MG? Get(int id);
        void Add(MG entity);
        void Remove(MG entity);
    }
}
