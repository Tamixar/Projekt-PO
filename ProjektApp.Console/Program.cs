using ProjektApp.DataModel;

class Program
{
    static void Main()
    {
        Klient klient = new Klient("Jan", "Kowalski", "jan@test.pl", "haslo123");

        GraPlanszowa catan = new GraPlanszowa
        {
            Id = 1,
            Nazwa = "Catan",
            Cena = 180.50m,
            StanMagazynowy = 5,
            LiczbaGraczy = 3,
           
        };

        Podrecznik dnd = new Podrecznik
        {
            Id = 2,
            Nazwa = "D&D Player's Handbook",
            Cena = 220.00m,
            StanMagazynowy = 2,
            System = "D&D 5e"
        };

        MG mistrzGry = new MG("Anna", "Nowak", "anna@test.pl", "admin1", "Mistrz Gry");

        klient.Ulubione.Add(catan);
        Console.WriteLine($"W ulubionych klienta: {klient.Ulubione.First().Nazwa}");

        
        Console.WriteLine($"\n--- ZAKUPY ---");
        klient.DodajDoKoszyka(catan, 1);
        klient.DodajDoKoszyka(dnd, 2);

        Console.WriteLine($"Wartosc koszyka przed zakupem: {klient.Koszyk.WartoscCalkowita} PLN");
        Console.WriteLine($"Stan magazynowy Catan przed: {catan.StanMagazynowy}");

        klient.Kup();

        Console.WriteLine($"Stan magazynowy Catan po zakupie: {catan.StanMagazynowy}");
        Console.WriteLine($"Wartosc koszyka po zakupie: {klient.Koszyk.WartoscCalkowita} PLN");
        Console.WriteLine($"Liczba zrealizowanych zamowien: {klient.Zamowienia.Count}");
        Console.WriteLine($"Wartosc pierwszego zamowienia: {klient.Zamowienia.First().WartoscCalkowita} PLN");


        Console.WriteLine($"\n--- SESJA RPG ---");
        Sesja nowaSesja = mistrzGry.DodajSesje("D&D 5e", DateTime.Now.AddDays(7));
        nowaSesja.LiczbaGraczy = 4;

        mistrzGry.DodajGracza(nowaSesja, klient);

        Console.WriteLine($"Liczba graczy na sesji: {nowaSesja.Gracze.Count}");
        Console.WriteLine($"Liczba rezerwacji klienta: {klient.Rezerwacje.Count}");
    }
}