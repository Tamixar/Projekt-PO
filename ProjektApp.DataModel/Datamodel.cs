namespace ProjektApp.DataModel
{
    public abstract class Uzytkownik
    {
        public int? Id { get; set; }
        public string? Imie { get; set; }
        public string? Nazwisko { get; set; }
        public string? Email { get; set; }
        protected string? Haslo { get; set; }

        protected Uzytkownik() { }
        protected Uzytkownik(string imie, string nazwisko, string email, string haslo)
        {
            Imie = imie;
            Nazwisko = nazwisko;
            Email = email;
            Haslo = haslo;
        }
        public virtual void Zaloguj(string haslo1)
        {
            if (haslo1 != Haslo) throw new UnauthorizedAccessException("Zle haslo");
        }
        public virtual void Wyloguj() { }
        public virtual void Wyswietl() { }

    }


    public class Klient : Uzytkownik
    {
        public DateTime Rezerwacje { get; set; }
        public List<string> Zamowienia { get; set; } = new List<string>();

        public Koszyk Koszyk { get; set; }
        public Koszyk Ulubione { get; set; } 

        public Klient(string imie, string nazwisko, string email, string haslo)
            : base(imie, nazwisko, email, haslo)
        {
            Koszyk = new Koszyk(this);
            Ulubione = new Koszyk(this);
        }

        public void DodajDoKoszyka(Towar produkt, int ilosc)
        {
            Koszyk.DodajTowar(produkt, ilosc);
        }

        public void UsunZKoszyka(Towar produkt)
        {
            Koszyk.UsunTowar(produkt);
        }

        public void Kup()
        {
            foreach (var element in Koszyk.Zawartosc)
            {
                if (!element.Key.CzyDostepny(element.Value))
                    throw new InvalidOperationException($"Brak towaru: {element.Key.Nazwa}");
            }

            foreach (var element in Koszyk.Zawartosc)
            {
                element.Key.StanMagazynowy -= element.Value;
            }
            Koszyk.Zawartosc.Clear();
            Koszyk.WartoscCalkowita = 0;
        }

        public void Wynajmij() {  }
        public void Rezerwuj(Sesja sesja) {  }
    }


    public class Pracownik : Uzytkownik
    {
        public string? Stanowisko { get; set; }
        public decimal? Pensja { get; set; }
        public double? Ocena { get; set; }

        public Pracownik() { }
        public Pracownik(string imie, string nazwisko, string email, string haslo, string stanowisko)
            : base(imie, nazwisko, email, haslo)
        {
            Stanowisko = stanowisko;
        }

        public void DodajTowar(Towar produkt, int ilosc) { }
        public void UsunTowar(Towar produkt) { }
        public void ZapiszWGrafiku() {  }
        public void SprawdzStan(Towar produkt) {  }
    }


    public class MG : Pracownik
    {
        public List<string>? Systemy { get; set; } = new List<string>();
        private double? Bonus { get; set; }

        public MG(string imie, string nazwisko, string email, string haslo, string stanowisko)
            : base(imie, nazwisko, email, haslo, stanowisko) { }

        public Sesja DodajSesje(string system, DateTime data)
        {
            return new Sesja { System = system, DataOdbycia = data, Prowadzacy = this };
        }
        public void DodajGracza(Sesja sesja, Klient gracz)
        {
            sesja.DodajGracza(gracz);
        }
        public void UsunGracza(Sesja sesja, Klient gracz)
        {
            sesja.UsunGracza(gracz);
        }
        public void PrzygotujScenariusz(Sesja sesja)
        {
            sesja.Status = "Przygotowana";
        }
    }


    public class Koszyk
    {
        public Klient? Wlasciciel { get; private set; }
        public Dictionary<Towar, int> Zawartosc { get; set; } = new Dictionary<Towar, int>();
        public double? WartoscCalkowita { get; set; }

        public Koszyk(Klient wlasciciel)
        {
            Wlasciciel = wlasciciel;
        }

        public void DodajTowar(Towar towar, int ilosc)
        {
            if (Zawartosc.ContainsKey(towar))
            {
                Zawartosc[towar] += ilosc;
                PrzeliczWartosc();
            }
            else
            {
                Zawartosc.Add(towar, ilosc);
                PrzeliczWartosc();
            }
        }

        public void UsunTowar(Towar towar)
        {
            if (Zawartosc.ContainsKey(towar))
            {
                Zawartosc.Remove(towar);
                PrzeliczWartosc();
            }

        }

        private void PrzeliczWartosc()
        {
            WartoscCalkowita = 0;

            foreach (var element in Zawartosc)
            {
                WartoscCalkowita += element.Key.Wartosc(element.Value);
            }
        }
    }

    public abstract class Towar
    {
        public int? Id { get; set; }
        public string? Nazwa { get; set; }
        public double? Cena { get; set; }
        public int? StanMagazynowy { get; set; }
        public string? Producent { get; set; }

        public bool CzyDostepny(int wymaganaIlosc = 1)
        {

            if (StanMagazynowy >= wymaganaIlosc)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public double Wartosc(int ilosc)
        {
            if (Cena != null)
            {
                return (double)(Cena * ilosc);
            }
            else
            {
                return 0.0;
            }
        }
    }

    public class GraPlanszowa : Towar
    {
        public int? LiczbaGraczy { get; set; }
        public int? CzasGry { get; set; }
        public string? Gatunek { get; set; }
    }

    public class Podrecznik : Towar
    {
        public string? System { get; set; }
        public float? Edycja { get; set; }
        public DateTime? DataWydania { get; set; }
    }

    public class Sesja
    {
        public int? Id { get; set; }
        public string? System { get; set; }
        public MG? Prowadzacy { get; set; }
        public List<Klient> Gracze { get; set; } = new List<Klient>();
        public int? LiczbaGraczy { get; set; }
        public DateTime? DataOdbycia { get; set; }
        public string Status { get; set; } = "Do przygotowania";

        public void DodajGracza(Klient gracz)
        {
            if (!Gracze.Contains(gracz))
            {
                Gracze.Add(gracz);
            }
        }

        public void UsunGracza(Klient gracz)
        {
            Gracze.Remove(gracz);
        }
    }

}
