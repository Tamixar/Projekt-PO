using ProjectApp.Abstractions;
using ProjektApp.DataModel;

namespace ProjectApp.DataAccess.Memory.Repositories;

public class PracownikRepositoryMemory : IPracownikRepository
{
    private readonly MemoryDbContext _db;

    public PracownikRepositoryMemory(MemoryDbContext db) => _db = db;

    public IQueryable<Pracownik> Query() => _db.Pracownicy.AsQueryable();

    public Pracownik? Get(int id) => _db.Pracownicy.FirstOrDefault(p => p.Id == id);

    public void Add(Pracownik entity) => _db.Pracownicy.Add(entity);

    public void Remove(Pracownik entity) => _db.Pracownicy.Remove(entity);
}