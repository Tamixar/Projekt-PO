using ProjectApp.Abstractions;
using ProjectApp.DataAccess.Memory;
using ProjektApp.DataModel;
public class MGRepositoryMemory : IMGRepository
{
    private readonly MemoryDbContext _db;

    public MGRepositoryMemory(MemoryDbContext db) => _db = db;

    public IQueryable<MG> Query() => _db.MistrzowieGry.AsQueryable();

    public MG? Get(int id) => _db.MistrzowieGry.FirstOrDefault(m => m.Id == id);

    public void Add(MG entity) => _db.MistrzowieGry.Add(entity);

    public void Remove(MG entity) => _db.MistrzowieGry.Remove(entity);
}