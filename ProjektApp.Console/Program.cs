using ProjectApp.Abstractions;
using ProjectApp.DataAccess.Memory;
using ProjectApp.DataAccess.Memory.Repositories;
using ProjectApp.ServiceAbstractions;
using ProjectApp.Services;

var db = new MemoryDbContext();

IKlientRepository klientRepo = new KlientRepositoryMemory(db);
IGraPlanszowaRepository graRepo = new GraPlanszowaRepositoryMemory(db);
IPodrecznikRepository podrecznikRepo = new PodrecznikRepositoryMemory(db);
IZamowienieRepository zamowienieRepo = new ZamowienieRepositoryMemory(db);
ISesjaRepository sesjaRepo = new SesjaRepositoryMemory(db);
IMGRepository mgRepo = new MGRepositoryMemory(db);
IPracownikRepository pracownikRepo = new PracownikRepositoryMemory(db);

IUnitOfWork uow = new UnitOfWorkMemory(db);

IKlientService klientSvc = new KlientService(klientRepo, uow);
ISklepService sklepSvc = new SklepService(graRepo, podrecznikRepo, zamowienieRepo, klientRepo, uow);
ISesjaRpgService sesjaSvc = new SesjaRpgService(sesjaRepo, mgRepo, klientRepo, uow);

IDataSeeder seeder = new DataSeeder(klientRepo, mgRepo, uow);
SeedResult seedResult = seeder.Seed();

Console.WriteLine(">>> Dane seedowane. Dostępni mistrzowie gry i klienci:");
Console.WriteLine($"MG1Id: {seedResult.MG1Id}");
Console.WriteLine($"MG2Id: {seedResult.MG2Id}");
Console.WriteLine($"Klient1Id: {seedResult.Klient1Id}");
Console.WriteLine($"Klient2Id: {seedResult.Klient2Id}");
Console.WriteLine($"Klient3Id: {seedResult.Klient3Id}");
