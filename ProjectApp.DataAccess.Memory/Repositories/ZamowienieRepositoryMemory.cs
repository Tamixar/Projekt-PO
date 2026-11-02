using ProjectApp.Abstractions;
using ProjektApp.DataModel;

namespace ProjectApp.DataAccess.Memory.Repositories;

public class ZamowienieRepositoryMemory : IZamowienieRepository
{
    private readonly MemoryDbContext _db;

    public ZamowienieRepositoryMemory(MemoryDbContext db) => _db = db;

    public IQueryable<Zamowienie> Query() => _db.Zamowienia.AsQueryable();

    public Zamowienie? Get(int id) => _db.Zamowienia.FirstOrDefault(z => z.Id == id);

    public void Add(Zamowienie entity) => _db.Zamowienia.Add(entity);

    public void Remove(Zamowienie entity) => _db.Zamowienia.Remove(entity);
}