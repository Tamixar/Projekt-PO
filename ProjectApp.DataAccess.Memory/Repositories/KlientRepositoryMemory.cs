using ProjectApp.Abstractions;
using ProjektApp.DataModel;

namespace ProjectApp.DataAccess.Memory.Repositories;

public class KlientRepositoryMemory : IKlientRepository
{
    private readonly MemoryDbContext _db;

    public KlientRepositoryMemory(MemoryDbContext db) => _db = db;

    public IQueryable<Klient> Query() => _db.Klienci.AsQueryable();

    public Klient? Get(int id) => _db.Klienci.FirstOrDefault(k => k.Id == id);

    public void Add(Klient entity) => _db.Klienci.Add(entity);

    public void Remove(Klient entity) => _db.Klienci.Remove(entity);
}