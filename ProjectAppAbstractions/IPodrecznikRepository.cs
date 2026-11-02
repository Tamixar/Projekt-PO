using ProjektApp.DataModel;

namespace ProjectApp.Abstractions;

public interface IPodrecznikRepository
{
    IQueryable<Podrecznik> Query();
    Podrecznik? Get(int id);
    void Add(Podrecznik entity);
    void Remove(Podrecznik entity);
}