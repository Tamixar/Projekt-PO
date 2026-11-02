using ProjectApp.Abstractions;
using ProjektApp.DataModel;

namespace ProjectApp.DataAccess.Memory.Repositories;

public class PodrecznikRepositoryMemory : IPodrecznikRepository
{
    private readonly MemoryDbContext _db;

    public PodrecznikRepositoryMemory(MemoryDbContext db) => _db = db;

    public IQueryable<Podrecznik> Query() => _db.Podreczniki.AsQueryable();

    public Podrecznik? Get(int id) => _db.Podreczniki.FirstOrDefault(p => p.Id == id);

    public void Add(Podrecznik entity) => _db.Podreczniki.Add(entity);

    public void Remove(Podrecznik entity) => _db.Podreczniki.Remove(entity);
}
