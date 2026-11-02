using ProjectApp.Abstractions;
using ProjektApp.DataModel;

namespace ProjectApp.DataAccess.Memory.Repositories;

public class GraPlanszowaRepositoryMemory : IGraPlanszowaRepository
{
    private readonly MemoryDbContext _db;

    public GraPlanszowaRepositoryMemory(MemoryDbContext db) => _db = db;

    public IQueryable<GraPlanszowa> Query() => _db.GryPlanszowe.AsQueryable();

    public GraPlanszowa? Get(int id) => _db.GryPlanszowe.FirstOrDefault(g => g.Id == id);

    public void Add(GraPlanszowa entity) => _db.GryPlanszowe.Add(entity);

    public void Remove(GraPlanszowa entity) => _db.GryPlanszowe.Remove(entity);
}