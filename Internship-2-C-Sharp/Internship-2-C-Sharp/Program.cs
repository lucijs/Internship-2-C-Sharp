using System.Collections.Immutable;
using System.Diagnostics;
using System.Text;

var Artikli = new Dictionary<string , (int Kolicina, double Cijena, DateTime DatumIsteka)>()
{
    {"Jabuka", (10,7, new DateTime(2024, 11, 15))},
    {"Oreo", (23,10, new DateTime(2025,01,01)) },
    {"Snickers", (17,9,new DateTime(2023,01,15))},
    {"LinoLada", (9, 30, new DateTime(2024, 12,13)) }
};

var Radnici = new Dictionary<string, DateTime>() {
    {"Lucija Fradelic", new DateTime(2002,07,24) },
    {"Ante Antić", new DateTime(1955,03,03) },
    {"Ana Anić", new DateTime(1977,07,09) },
    {"Klara Klaric", new DateTime(1996,06,03) }
};

var Racuni = new Dictionary<int, (DateTime DatumIzdavanja, Dictionary<string, double> SviProizvodi)>() {
    {1234, (new DateTime(2023,11,17),new Dictionary<string, double>() {{"Jabuka", 14},{"Oreo",30 } }) },
    {1235, (new DateTime(2023,10,05),new Dictionary<string, double>() {{"LinoLada", 90},{"Oreo",50 } }) }

};

BeginningPage(Artikli, Radnici, Racuni);
static void BeginningPage(Dictionary<string, (int Kolicina, double Cijena, DateTime DatumIsteka)> Articles, Dictionary<string, DateTime> Workers, Dictionary<int, (DateTime DatumIzdavanja, Dictionary<string, double> SviProizvodi)> Reciepts  )
{
    Console.WriteLine("1 - Artikli\n2 - Radnici\n3 - Racuni\n4 - Statistika\n0 - Izlaz iz aplikacije");

    var initialNumber = Console.ReadLine();

    switch (initialNumber)
    {
        case "0":
            return;
        case "1":
            Console.Clear();
            Articles = InitialPageForArticles(Articles, Workers, Reciepts);
            Console.Clear();
            BeginningPage(Articles, Workers, Reciepts);
            break;

        case "2":
            Console.Clear();
            Workers = InitialPageForWorkers(Articles,Workers,Reciepts);
            Console.Clear();
            BeginningPage(Articles, Workers, Reciepts);
            break;

        case "3":
            Console.WriteLine("racuni");
            break;

        case "4":
            Console.WriteLine("statistika");
            break;

        default:
            Console.Clear();
            Console.WriteLine("Upisani broj nije jedan od ponudenih. Pokusajte ponovno.");
            BeginningPage(Articles, Workers, Reciepts);
            break;
    }
}
static Dictionary<string, DateTime> InitialPageForWorkers(Dictionary<string, (int Kolicina, double Cijena, DateTime DatumIsteka)> Articles, Dictionary<string, DateTime> Workers, Dictionary<int, (DateTime DatumIzdavanja, Dictionary<string, double> SviProizvodi)> Reciepts)
{
    Console.WriteLine("1 - Unos radnika\n2 - Brisanje radnika\n3 - Uređivanje radnika\n4 - Ispis\n0 - Povratak na glavni izbornik");
    var initialNumber = Console.ReadLine();

    switch (initialNumber)
    {
        case "0":
            Console.Clear();
            BeginningPage(Articles, Workers, Reciepts);
            return Workers;
        case "1":
            Console.Clear();
            var (ime, datumRodenja) = AddingAWorker(Articles, Workers, Reciepts);
            if (ime != "")
                Workers.Add(ime, datumRodenja);
            return Workers;
        case "2":
            Console.Clear();
            var lista = DeletingWorkers(Articles, Workers, Reciepts);
            var listaRadnikaZaIspis = new StringBuilder();
            foreach (var item in lista)
                listaRadnikaZaIspis.Append("\n" + item);
            Console.WriteLine("Jeste li sigurni da želite zbrisati sljedeće radnike:" + listaRadnikaZaIspis.ToString() + "\nunesite DA ako želite nastaviti s brisanjem.");
            if (Console.ReadLine().Trim().ToLower() == "da")
            {
                foreach (var item in lista)
                    Workers.Remove(item);
            }
            return Workers;  
        case "3":
            Console.Clear();
            Workers = ChangingTheWorkers(Articles, Workers, Reciepts);
            return Workers;
        case "4":
            Console.Clear();
            WorkersListing(Articles, Workers, Reciepts);
            Console.ReadKey();
            break;
        default:
            Console.Clear();
            Console.WriteLine("Upisani broj nije jedan od ponudenih. Pokusajte ponovno.");
            InitialPageForWorkers(Articles, Workers, Reciepts);
            break;
    }
    return Workers;
}
static void WorkersListing(Dictionary<string, (int Kolicina, double Cijena, DateTime DatumIsteka)> Articles, Dictionary<string, DateTime> Workers, Dictionary<int, (DateTime DatumIzdavanja, Dictionary<string, double> SviProizvodi)> Reciepts)
{
    Console.WriteLine("a. ispis svih radnika\nb. ispis svih radnika kojima je rođendan u tekućem mjesecu\n0 povratak na glavni izbornik");
    var chosenSign = Console.ReadLine().ToLower();
    switch (chosenSign)
    {
        case "a":
            Console.Clear();
            ListOutTheWorkers(MakeWorkersList(Workers) ,Workers);
            break;
        case "b":
            Console.Clear();
            ListOutTheWorkers(WorkersBornThisMonth(Workers), Workers);
            break;
        case "0":
            Console.Clear();
            BeginningPage(Articles, Workers, Reciepts);
            break;
        default:
            Console.Clear();
            Console.WriteLine("Upisan znak nije jedan od ponudenih. Pokusajte ponovno.");
            WorkersListing(Articles, Workers, Reciepts);
            break;
    }
}
static List<string> WorkersBornThisMonth(Dictionary<string, DateTime> Workers)
{ 
    var lista = new List<string>();
    foreach (var item in Workers)
    {
        if (item.Value.Month == DateTime.Now.Month)
            lista.Add(item.Key);
    }
    return lista;
}
static List<string> MakeWorkersList(Dictionary<string, DateTime> Workers)
{
    var listaArtikla = new List<string>();
    foreach (var artikl in Workers)
    {
        listaArtikla.Add(artikl.Key);
    }
    return listaArtikla;
}
static void ListOutTheWorkers(List<string> listaRdanika,Dictionary<string, DateTime> Workers)
{
    foreach (var proizvod in listaRdanika)
        Console.WriteLine($"{proizvod} - {Math.Round(DateTime.Now.Subtract(Workers[proizvod]).TotalDays/365,0)}");
    return;
}
static Dictionary<string, DateTime> ChangingTheWorkers(Dictionary<string, (int Kolicina, double Cijena, DateTime DatumIsteka)> Articles, Dictionary<string, DateTime> Workers, Dictionary<int, (DateTime DatumIzdavanja, Dictionary<string, double> SviProizvodi)> Reciepts)
{
    Console.WriteLine("a. imene i prezimene radnika\nb. datumu rođenja radnika\n0 povratak na glavni izbornik");
    var initial = Console.ReadLine();
    Console.WriteLine($"Navedite ime i prezime radnika kojeg želite izmijenit");
    var radnik = GettingTheName("ime i prezime");
    if (!Workers.ContainsKey(radnik))
    {
        Console.WriteLine($"{radnik} ne postoji. Pokusajte ponovno.");
        ChangingTheWorkers(Articles, Workers, Reciepts);
    }
    Console.WriteLine($"Ako želite izmijeniti podatke radnika {radnik} unesite DA");
    if (Console.ReadLine().Trim().ToLower() != "da")
    {
        Console.Clear();
        InitialPageForWorkers(Articles, Workers, Reciepts);
    }
    switch (initial)
    {
        case "a":
            Console.Clear();
            Console.WriteLine("Unesite novo ime i prezime");
            var ime = GettingTheName("novo ime i prezime");
            if (Workers.ContainsKey(ime))
            {
                Console.WriteLine($"Već postoji radnik {ime}");
                Console.ReadKey();
                Console.Clear();
                InitialPageForWorkers(Articles, Workers, Reciepts);
            }
            Console.WriteLine($"Ako želite promijeniti ime i prezime mradnika s {radnik} na {ime} unesite DA");
            if (Console.ReadLine().Trim().ToLower() == "da")
            {
                var privremena = Workers[radnik];
                Workers.Remove(radnik);
                Workers.Add(ime, privremena);
            }
            return Workers;
        case "b":
            Console.Clear();
            Console.WriteLine("Unesite novi datum, more biti oblika yyyy-mm-dd");

            DateTime novoVrijeme = Workers[radnik];
            DateTime.TryParse(Console.ReadLine(), out novoVrijeme);
            if (novoVrijeme != Workers[radnik])
            {
                Console.WriteLine($"Ako želite promijeniti datum rođenja radnika s {Workers[radnik].ToString()} na {novoVrijeme.ToString()} unesite DA");
                if (Console.ReadLine().Trim().ToLower() == "da")
                    Workers[radnik] = novoVrijeme;
            }
            return Workers;
        case "0":
            Console.Clear();
            BeginningPage(Articles, Workers, Reciepts);
            break;
        default:
            Console.Clear();
            Console.WriteLine("Neispravan unos, unesite jedan od predlozenih znakova.");
            ChangingTheWorkers(Articles, Workers, Reciepts);
            break;
    }
    return Workers;
}
static List<string> DeletingWorkers(Dictionary<string, (int Kolicina, double Cijena, DateTime DatumIsteka)> Articles, Dictionary<string, DateTime> Workers, Dictionary<int, (DateTime DatumIzdavanja, Dictionary<string, double> SviProizvodi)> Reciepts)
{
    var list = new List<string>();
    Console.WriteLine("a. po imenu\nb. sve one starije od 65\n0 povratak na glavni izbornik");
    var initial = Console.ReadLine();
    switch (initial)
    {
        case "a":
            Console.Clear();
            Console.WriteLine("Kojeg radnika želite izbrinsati:");
            var ime = GettingTheName("ime i prezime");
            if (Workers.ContainsKey(ime))
                list.Add(ime);
            else
            {
                Console.Clear();
                Console.WriteLine("Uneseni radnik ne postoji. Pokusajte ponovno");
                DeletingWorkers(Articles, Workers, Reciepts);
            }
            return list;
        case "b":
            Console.Clear();
            return WorkersOlderThen65(Workers);
        case "0":
            Console.Clear();
            BeginningPage(Articles, Workers, Reciepts);
            break;
        default:
            Console.Clear();
            Console.WriteLine("Neispravan upis, pokusajte ponovno");
            DeletingWorkers(Articles, Workers, Reciepts);
            break;
    }
    return list;
}
static List<string> WorkersOlderThen65(Dictionary<string, DateTime> Workers)
{
    var list = new List<string>();
    foreach (var item in Workers)
    {
        if (item.Value.Subtract(DateTime.Now).TotalDays>65*365)
            list.Add(item.Key);
    }
    return list;
}
static (string ime, DateTime datumRodenja) AddingAWorker(Dictionary<string, (int Kolicina, double Cijena, DateTime DatumIsteka)> Articles, Dictionary<string, DateTime> Workers, Dictionary<int, (DateTime DatumIzdavanja, Dictionary<string, double> SviProizvodi)> Reciepts)
{
    Console.WriteLine("Upišite sve informacije o novom radniku");
    string ime = GettingTheName("ime i prezime radnika");
    var provjera = false;
    DateTime datumIsteka = new DateTime();
    do
    {
        Console.WriteLine("datum (mora biti u obliku yyyy-mm-dd): ");
        provjera = DateTime.TryParse(Console.ReadLine(), out datumIsteka);
    }
    while (!provjera);

    Console.WriteLine($"Želite li dodati radnika {ime} - {datumIsteka}\n(Ako želite upišite DA)");
    if (Console.ReadLine().Trim().ToLower() == "da")
        return (ime, datumIsteka);
    return ("", DateTime.Now);

}
static Dictionary<string, (int Kolicina, double Cijena, DateTime DatumIsteka)> InitialPageForArticles(Dictionary<string, (int Kolicina, double Cijena, DateTime DatumIsteka)> Articles, Dictionary<string, DateTime> Workers, Dictionary<int, (DateTime DatumIzdavanja, Dictionary<string, double> SviProizvodi)> Reciepts)
{
    Console.WriteLine("1 - Unos artikla\n2 - Brisanje artikla\n3 - Uređivanje artikla\n4 - Ispis\n0 - Povratak na glavni izbornik");
    var initialNumber = Console.ReadLine();

    switch (initialNumber)
    {
        case "0":
            Console.Clear();
            BeginningPage(Articles, Workers, Reciepts);
            return Articles;
        case "1":
            Console.Clear();
            var (proizvod, kolicina, cijena, datum) = AddArticle(Articles,Workers,Reciepts);
            if(proizvod!="")
                Articles.Add(proizvod, (kolicina, cijena, datum));
            return Articles;
        case "2":
            Console.Clear();
            var lista = DeletingArticles(Articles, Workers, Reciepts);
            var listaProizvodaZaIspis = new StringBuilder();
            foreach (var item in lista)
                listaProizvodaZaIspis.Append("\n"+item);
            Console.WriteLine("Jeste li sigurni da želite zbrisati sljedeće artikle:" + listaProizvodaZaIspis.ToString()+ "\nunesite DA ako želite nastaviti s brisanjem.");
            if (Console.ReadLine().Trim().ToLower() == "da")
            {
                foreach (var item in lista)
                    Articles.Remove(item);
            }
            return Articles;
        case "3":
            Console.Clear();
            Articles = ChangingTheArticles(Articles,Workers,Reciepts);
            return Articles;

        case "4":
            Console.Clear();
            ArticlesListing(Articles, Workers, Reciepts);
            Console.ReadKey();
            break;

        default:
            Console.Clear();
            Console.WriteLine("Upisani broj nije jedan od ponudenih. Pokusajte ponovno.");
            InitialPageForArticles(Articles, Workers, Reciepts);
            break;
    }
    return Articles;
}
static Dictionary<string, (int Kolicina, double Cijena, DateTime DatumIsteka)> ChangingTheArticles(Dictionary<string, (int Kolicina, double Cijena, DateTime DatumIsteka)> Articles, Dictionary<string, DateTime> Workers, Dictionary<int, (DateTime DatumIzdavanja, Dictionary<string, double> SviProizvodi)> Reciepts)
{
    Console.WriteLine("a. zasebno proizvoda\nb. popust/poskupljenje na sve proizvode\n0 povratak na glavni izbornik");
    var initial = Console.ReadLine();
    switch (initial)
    {
        case "a":
            Console.Clear();
            var (proizvod, kol, cijena, datum)=ChangeOneArticle(Articles, Workers, Reciepts);
            Articles[proizvod] = (kol, cijena, datum);
            break;
        case "b":
            Console.Clear();
            Articles = ChangeMorePrices(Articles, Workers, Reciepts);
            break;
        case "0":
            Console.Clear();
            BeginningPage(Articles, Workers, Reciepts);
            break;
        default:
            Console.Clear();
            Console.WriteLine("Neispravan unos, unesite jedan od predlozenih znakova.");
            ChangingTheArticles(Articles, Workers, Reciepts);
            break;
    }
    return Articles;
}
static Dictionary<string, (int Kolicina, double Cijena, DateTime DatumIsteka)> ChangeMorePrices(Dictionary<string, (int Kolicina, double Cijena, DateTime DatumIsteka)> Articles, Dictionary<string, DateTime> Workers, Dictionary<int, (DateTime DatumIzdavanja, Dictionary<string, double> SviProizvodi)> Reciepts)
{
    Console.WriteLine("a. popust\nb. poskupljenje\n0 povratak na prethodni izbornik");
    var ispis = Console.ReadLine();
    switch (ispis)
    {
        case "a":
            var popust = 0.0;
            Console.WriteLine("Koliki popust želite za bude?");
            double.TryParse(Console.ReadLine(), out popust);
            Console.WriteLine($"Jeste li sigurni da zelite pojeftiniti sve proizvode za {popust} posto");
            if (Console.ReadLine().Trim().ToLower() == "da" && popust>0 && popust<100)
            {
                var lista = MakeArticleList(Articles);
                foreach (var item in lista)
                {
                    Articles[item] = (Articles[item].Kolicina, Articles[item].Cijena * (1 - popust / 100), Articles[item].DatumIsteka);        
                }
            }
            return Articles;
        case "b":
            var poskupljenje = 0.0;
            Console.WriteLine("Koliki želite za proizvodi poskupe?");
            double.TryParse(Console.ReadLine(), out poskupljenje);
            Console.WriteLine($"Jeste li sigurni da zelite poskupiti sve proizvode za {poskupljenje} posto");
            if (Console.ReadLine().Trim().ToLower() == "da" && poskupljenje > 0)
            {
                var lista = MakeArticleList(Articles);
                foreach (var item in lista)
                {
                    Articles[item] = (Articles[item].Kolicina, Articles[item].Cijena * (1 +poskupljenje / 100), Articles[item].DatumIsteka);
                }
            }
            return Articles;
        case "0":
            Console.Clear();
            ChangingTheArticles(Articles, Workers, Reciepts);
            break;
        default:
            Console.Clear();
            Console.WriteLine("Neispravan unos, unesite jedan od predlozenih znakova.");
            ChangeMorePrices(Articles, Workers, Reciepts);
            break;
    }
    return Articles;
}
static (string artikl, int kolicina, double cijena, DateTime datum) ChangeOneArticle(Dictionary<string, (int Kolicina, double Cijena, DateTime DatumIsteka)> Articles, Dictionary<string, DateTime> Workers, Dictionary<int, (DateTime DatumIzdavanja, Dictionary<string, double> SviProizvodi)> Reciepts)
{
    Console.WriteLine("Navedite ime proizvoda koji želite izmijenit");
    var proizovd = GettingTheName("naziv proizvoda");
    if (!Articles.ContainsKey(proizovd))
    {
        Console.WriteLine($"{proizovd} ne postoji. Pokusajte ponovno.");
        Console.ReadKey();
        Console.Clear();
        ChangingTheArticles(Articles, Workers, Reciepts);
    }
    Console.WriteLine($"Ako želite izmijeniti proizvod {proizovd} unesite DA");
    if (Console.ReadLine().Trim().ToLower() == "da")
    {
        Console.Clear();
        Console.WriteLine("Što zelite izmijeniti?\na. količinu\nb. cijenu\nc. datum isteka\n0 povratak na prethodni izbornik");
        var ispis = Console.ReadLine();
        switch (ispis)
        {
            case "a":
                Console.WriteLine("Koja je nova kolicina koju zelite postaviti?");
                int kol=-1;
                int.TryParse(Console.ReadLine(), out kol);
                if (kol > 0)
                {
                    Console.WriteLine($"Ako želite promijeniti kolicinu {proizovd} s {Articles[proizovd].Kolicina} na {kol} unesite DA");
                    if (Console.ReadLine().Trim().ToLower() == "da")
                    {
                        return (proizovd, kol, Articles[proizovd].Cijena, Articles[proizovd].DatumIsteka);
                    }
                }
                break;
            case "b":
                Console.WriteLine("Koja je nova cijena koju zelite postaviti?");
                double price = -1;
                double.TryParse(Console.ReadLine(), out price);
                if (price > 0)
                {
                    Console.WriteLine($"Ako želite promijeniti cijenu {proizovd} s {Articles[proizovd].Cijena} na {price} unesite DA");
                    if (Console.ReadLine().Trim().ToLower() == "da")
                    {
                        return (proizovd, Articles[proizovd].Kolicina, price, Articles[proizovd].DatumIsteka);
                    }
                }
                break;
            case "c":
                Console.WriteLine("Koji je novi datum isteka koji zelite postaviti?\n(mora biti u obliku yyyy-mm-dd)");
                DateTime novoVrijeme = Articles[proizovd].DatumIsteka;
                DateTime.TryParse(Console.ReadLine(), out novoVrijeme);
                if (novoVrijeme!= Articles[proizovd].DatumIsteka)
                {
                    Console.WriteLine($"Ako želite promijeniti datum isteka {proizovd} s {Articles[proizovd].DatumIsteka} na {novoVrijeme} unesite DA");
                    if (Console.ReadLine().Trim().ToLower() == "da")
                    {
                        return (proizovd, Articles[proizovd].Kolicina, Articles[proizovd].Cijena, novoVrijeme);
                    }
                }
                break;
            case "0":
                Console.Clear();
                ChangingTheArticles(Articles, Workers, Reciepts);
                break;
            default:
                Console.Clear();
                Console.WriteLine("Neispravan unos, unesite jedan od predlozenih znakova.");
                ChangeOneArticle(Articles, Workers, Reciepts);
                break;
        }
    }
    return (proizovd, Articles[proizovd].Kolicina, Articles[proizovd].Cijena, Articles[proizovd].DatumIsteka);
}
static List<string> DeletingArticles(Dictionary<string, (int Kolicina, double Cijena, DateTime DatumIsteka)> Articles, Dictionary<string, DateTime> Workers, Dictionary<int, (DateTime DatumIzdavanja, Dictionary<string, double> SviProizvodi)> Reciepts)
{
    var list = new List<string>();
    Console.WriteLine("a. po imenu artikla\nb. sve one kojima je istekao datum trajanja\n0 povratak na glavni izbornik");
    var initial = Console.ReadLine();
    switch (initial)
    {
        case "a":
            Console.Clear();
            Console.WriteLine("Koji proizvod želite izbrinsati (unesite ime):");
            var ime = GettingTheName("naziv proizvoda");
            if (Articles.ContainsKey(ime))
                list.Add(ime);
            else
            {
                Console.Clear();
                Console.WriteLine("Uneseni proizvid ne postoji. Pokusajte ponovno");
                DeletingArticles(Articles, Workers, Reciepts);
            }
            return list;
        case "b":
            Console.Clear();
            return ArticlesWhichExpired(Articles);
        case "0":
            Console.Clear();
            BeginningPage(Articles,Workers,Reciepts);
            break;
        default:
            Console.Clear();
            Console.WriteLine("Neispravan upis, pokusajte ponovno");
            DeletingArticles(Articles, Workers, Reciepts);
            break;
    }
    return list;
}
static List<string> ArticlesWhichExpired(Dictionary<string, (int Kolicina, double Cijena, DateTime DatumIsteka)> Articles)
{
    var list = new List<string>();
    foreach (var item in Articles)
    {
        if (item.Value.DatumIsteka.Subtract(DateTime.Now).TotalDays < 0)
            list.Add(item.Key);
    }
    return list;
}
static (string proizvod, int kolicina, double cijena, DateTime datum) AddArticle(Dictionary<string, (int Kolicina, double Cijena, DateTime DatumIsteka)> Articles, Dictionary<string, DateTime> Workers, Dictionary<int, (DateTime DatumIzdavanja, Dictionary<string, double> SviProizvodi)> Reciepts)
{
    Console.WriteLine("Upišite sve informacije o novom proizvodu");
    string proizvod = GettingTheName("naziv proizvoda");
    if (Articles.ContainsKey(proizvod))
    {
        Console.WriteLine("Postoji proizvod s tim nazivom.");
        Console.ReadKey();
        Console.Clear();
        InitialPageForArticles(Articles,Workers,Reciepts);
    }
    int kolicina;
    var provjera = false;
    do
    {
        Console.WriteLine("količina: ");
        provjera = int.TryParse(Console.ReadLine(), out kolicina);
    }
    while (!provjera);
    double cijena;
    do
    {
        Console.WriteLine("cijena: ");
        provjera = double.TryParse(Console.ReadLine(), out cijena);
    }
    while (!provjera);
    DateTime datumIsteka = new DateTime();
    do
    {
        Console.WriteLine("datum (mora biti u obliku yyyy-mm-dd): ");
        provjera = DateTime.TryParse(Console.ReadLine(), out datumIsteka);
    }
    while (!provjera);

    Console.WriteLine($"Želite li dodati artikl {proizvod} - {kolicina} - {cijena} - {datumIsteka}\n(Ako želite upišite DA)");
    var odgovor = Console.ReadLine();
    if (odgovor.Trim().ToLower() == "da")
        return (proizvod, kolicina, cijena, datumIsteka);
    return ("", 0, 0.0, DateTime.Now);
}
static string GettingTheName(string rijec)
{
    Console.WriteLine($"upisite {rijec}: ");
    var proizvod = Console.ReadLine();
    while (proizvod.Trim() == "")
    {
        Console.Clear();
        Console.WriteLine("Neispravan unos. Unesite ponovno: ");
        proizvod = Console.ReadLine();
    }
    return proizvod;
}
static void ArticlesListing(Dictionary<string, (int Kolicina, double Cijena, DateTime DatumIsteka)> Articles, Dictionary<string, DateTime> Workers, Dictionary<int, (DateTime DatumIzdavanja, Dictionary<string, double> SviProizvodi)> Reciepts)
{
    Console.WriteLine("a. ispis svih artikla\nb. ispis svih artikla sortirano po imenu\nc. ispis svih artikla sortirano po datumu silazno\nd. ispis svih artikla sortirano po datumu uzlazno\ne. svih artikala sortirano po količini\nf. najprodavaniji artikli\ng. najmanje prodavani artikli\n0 povratak na glavni izbornik");
    var chosenSign = Console.ReadLine().ToLower();
    switch (chosenSign)
    {
        case "a":
            Console.Clear();
            ListOutTheArticles(MakeArticleList(Articles), Articles);
            break;
        case "b":
            var list = MakeArticleList(Articles);
            list.Sort();
            Console.Clear();
            ListOutTheArticles(list, Articles);
            break;
        case "c":
            Console.Clear();
            ListOutTheArticlesUsingDates(MakeDatesList(Articles), Articles);
            break;
        case "d":
            var listOfDates = MakeDatesList(Articles);
            listOfDates.Reverse();
            Console.Clear();
            ListOutTheArticlesUsingDates(listOfDates, Articles);
            break;
        case "e":
            Console.Clear();
            ListOutTheArticlesUsingAmount(MakeAmountList(Articles), Articles);
            break;
        case "f":
            Console.Clear();
            BestSeller(SoldPieces(Articles, Reciepts), Articles);
            break;
        case "g":
            Console.Clear();
            WorstSeller(SoldPieces(Articles, Reciepts), Articles);
            break;
        case "0":
            Console.Clear();
            BeginningPage(Articles, Workers, Reciepts);
            break;
        default:
            Console.Clear();
            Console.WriteLine("Upisan znak nije jedan od ponudenih. Pokusajte ponovno.");
            ArticlesListing(Articles, Workers, Reciepts);
            break;
    }
}
static void WorstSeller(Dictionary<string, double> list, Dictionary<string, (int Kolicina, double Cijena, DateTime DatumIsteka)> Articles)
{
    var proizvod = "";
    var min = 0.0;
    foreach (var item in list)
    {
        if (min > item.Value || min == 0)
        {
            min = item.Value;
            proizvod = item.Key;
        }
    }
    Console.WriteLine($"{proizvod}({Articles[proizvod].Kolicina}) - {Articles[proizvod].Cijena} - {(Math.Round(Articles[proizvod].DatumIsteka.Subtract(DateTime.Now).TotalDays, 2) > 0 ? "do isteka " + Math.Round(Articles[proizvod].DatumIsteka.Subtract(DateTime.Now).TotalDays, 2) : "od isteka " + Math.Abs(Math.Round(Articles[proizvod].DatumIsteka.Subtract(DateTime.Now).TotalDays, 2)))} - prodano ih je {min}");
    return;
}
static void BestSeller(Dictionary<string, double> list, Dictionary<string, (int Kolicina, double Cijena, DateTime DatumIsteka)> Articles)
{
    var (proizvod,max) = ("",0.0);
    foreach (var item in list)
    {
        if (max < item.Value)
        {
            max = item.Value;
            proizvod = item.Key;
        }
    }
    Console.WriteLine($"{proizvod}({Articles[proizvod].Kolicina}) - {Articles[proizvod].Cijena} - {(Math.Round(Articles[proizvod].DatumIsteka.Subtract(DateTime.Now).TotalDays, 2) > 0 ? "do isteka " + Math.Round(Articles[proizvod].DatumIsteka.Subtract(DateTime.Now).TotalDays, 2) : "od isteka " + Math.Abs(Math.Round(Articles[proizvod].DatumIsteka.Subtract(DateTime.Now).TotalDays, 2)))} - prodano ih je {max}");
    return;
}
static Dictionary<string, double> SoldPieces(Dictionary<string, (int Kolicina, double Cijena, DateTime DatumIsteka)> Articles, Dictionary<int, (DateTime DatumIzdavanja, Dictionary<string, double> SviProizvodi)> Reciepts)
{
    var list = new Dictionary<string, double>();
    foreach (var reciept in Reciepts)
    {
        foreach (var item in reciept.Value.SviProizvodi)
        {
            if (list.ContainsKey(item.Key))
                list[item.Key] += item.Value / Articles[item.Key].Cijena;
            else
                list.Add(item.Key, item.Value / Articles[item.Key].Cijena);
        }
    }
    return list;
}
static List<int> MakeAmountList(Dictionary<string, (int Kolicina, double Cijena, DateTime DatumIsteka)> Articles)
{
    var list = new List<int>();

    foreach (var article in Articles)
    {
        list.Add(article.Value.Kolicina);
    }
    list.Sort();
    list.Reverse();
    return list;
}
static void ListOutTheArticlesUsingAmount(List<int> listOfAmounts, Dictionary<string, (int Kolicina, double Cijena, DateTime DatumIsteka)> Articles)
{
    foreach (var amount in listOfAmounts)
    {
        foreach (var proizvod in Articles)
        {
            if (proizvod.Value.Kolicina == amount)
            {
                Console.WriteLine($"{proizvod.Key}({proizvod.Value.Kolicina}) - {proizvod.Value.Cijena} - {(Math.Round(proizvod.Value.DatumIsteka.Subtract(DateTime.Now).TotalDays, 2) > 0 ? "do isteka " + Math.Round(proizvod.Value.DatumIsteka.Subtract(DateTime.Now).TotalDays, 2) : "od isteka " + Math.Abs(Math.Round(proizvod.Value.DatumIsteka.Subtract(DateTime.Now).TotalDays, 2)))}");
                break;
            }
        }
    }
}
static List<string> MakeArticleList(Dictionary<string, (int Kolicina, double Cijena, DateTime DatumIsteka)> Articles)
{
    var listaArtikla = new List<string>();
    foreach (var artikl in Articles)
    {
        listaArtikla.Add(artikl.Key);
    }
    return listaArtikla;
}
static List<DateTime> MakeDatesList(Dictionary<string, (int Kolicina, double Cijena, DateTime DatumIsteka)> Articles)
{ 
    var listaDatuma = new List<DateTime>();
    foreach (var artikl in Articles)
    {
        listaDatuma.Add(artikl.Value.DatumIsteka);
    }
    listaDatuma.Sort();
    return listaDatuma;
}
static void ListOutTheArticles(List<string> Articles, Dictionary<string, (int Kolicina, double Cijena, DateTime DatumIsteka)> Artikli)
{
    foreach (var proizvod in Articles)
        Console.WriteLine($"{proizvod}({Artikli[proizvod].Kolicina}) - {Artikli[proizvod].Cijena} - {(Math.Round(Artikli[proizvod].DatumIsteka.Subtract(DateTime.Now).TotalDays, 2) > 0 ? "do isteka " + Math.Round(Artikli[proizvod].DatumIsteka.Subtract(DateTime.Now).TotalDays, 2) : "od isteka " + Math.Abs(Math.Round(Artikli[proizvod].DatumIsteka.Subtract(DateTime.Now).TotalDays, 2)))}");
    return;
}
static void ListOutTheArticlesUsingDates(List<DateTime> Dates, Dictionary<string, (int Kolicina, double Cijena, DateTime DatumIsteka)> Articles)
{
    foreach (var date in Dates)
    { 
        foreach (var proizvod in Articles) 
        {
            if (proizvod.Value.DatumIsteka == date)
            { 
                Console.WriteLine($"{proizvod.Key}({proizvod.Value.Kolicina}) - {proizvod.Value.Cijena} - {(Math.Round(proizvod.Value.DatumIsteka.Subtract(DateTime.Now).TotalDays, 2) > 0 ? "do isteka " + Math.Round(proizvod.Value.DatumIsteka.Subtract(DateTime.Now).TotalDays, 2) : "od isteka " + Math.Abs(Math.Round(proizvod.Value.DatumIsteka.Subtract(DateTime.Now).TotalDays, 2)))}");
                break;
            }
        }
    }
}