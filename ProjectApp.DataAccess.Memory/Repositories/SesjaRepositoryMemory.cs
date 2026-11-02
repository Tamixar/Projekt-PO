using ProjectApp.Abstractions;
using ProjektApp.DataModel;

namespace ProjectApp.DataAccess.Memory.Repositories;

public class SesjaRepositoryMemory : ISesjaRepository
{
    private readonly MemoryDbContext _db;

    public SesjaRepositoryMemory(MemoryDbContext db) => _db = db;

    public IQueryable<Sesja> Query() => _db.Sesje.AsQueryable();

    public Sesja? Get(int id) => _db.Sesje.FirstOrDefault(s => s.Id == id);

    public void Add(Sesja entity) => _db.Sesje.Add(entity);

    public void Remove(Sesja entity) => _db.Sesje.Remove(entity);
}