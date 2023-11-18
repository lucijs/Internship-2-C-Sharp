using System.Collections.Immutable;
using System.Diagnostics;

var Artikli = new Dictionary<string , (int Kolicina, double Cijena, DateTime DatumIsteka)>()
{
    {"Jabuka", (10,7, new DateTime(2024, 11, 15))},
    {"Oreo", (23,10, new DateTime(2025,01,01)) },
    {"Snickers", (17,9,new DateTime(2023,01,15))},
    {"LinoLada", (9, 30, new DateTime(2024, 12,13)) }
};

var Radnici = new Dictionary<string, DateTime>() {
    {"Lucija Fradelic", new DateTime(2002,07,24) },
    {"Ante Antić", new DateTime(1965,03,03) },
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
    int initialNumber = 10;
    Console.WriteLine("1 - Artikli\n2 - Radnici\n3 - Racuni\n4 - Statistika\n0 - Izlaz iz aplikacije");

    int.TryParse(Console.ReadLine(), out initialNumber);

    switch (initialNumber)
    {
        case 0:
            return ;
        case 1:
            InitialPageForArticles(Articles, Workers, Reciepts);
            break;

        case 2:
            Console.WriteLine("radnici");
            break;

        case 3:
            Console.WriteLine("racuni");
            break;

        case 4:
            Console.WriteLine("statistika");
            break;

        default:
            Console.WriteLine("Upisani broj nije jedan od ponudenih. Pokusajte ponovno.");
            BeginningPage(Articles, Workers, Reciepts);
            break;
    }
}

static void InitialPageForArticles(Dictionary<string, (int Kolicina, double Cijena, DateTime DatumIsteka)> Articles, Dictionary<string, DateTime> Workers, Dictionary<int, (DateTime DatumIzdavanja, Dictionary<string, double> SviProizvodi)> Reciepts)
{
    int initialNumber = 10;
    Console.WriteLine("1 - Unos artikla\n2 - Brisanje artikla\n3 - Uređivanje artikla\n4 - Ispis\n0 - Povratak na glavni izbornik");
    int.TryParse(Console.ReadLine(), out initialNumber);

    switch (initialNumber)
    {
        case 0:
            BeginningPage(Articles, Workers, Reciepts);
            return;
        case 1:

            break;

        case 2:
            Console.WriteLine("radnici");
            break;

        case 3:
            Console.WriteLine("racuni");
            break;

        case 4:
            ArticlesListing(Articles, Workers, Reciepts);
            break;

        default:
            Console.WriteLine("Upisani broj nije jedan od ponudenih. Pokusajte ponovno.");
            break;
    }
}

static void ArticlesListing(Dictionary<string, (int Kolicina, double Cijena, DateTime DatumIsteka)> Articles, Dictionary<string, DateTime> Workers, Dictionary<int, (DateTime DatumIzdavanja, Dictionary<string, double> SviProizvodi)> Reciepts)
{
    Console.WriteLine("a. ispis svih artikla\nb. ispis svih artikla sortirano po imenu\nc. ispis svih artikla sortirano po datumu silazno\nd. ispis svih artikla sortirano po datumu uzlazno\ne. svih artikala sortirano po količini\nf. najprodavaniji artikli\ng. najmanje prodavani artikli\n0 povratak na glavni izbornik");
    var chosenSign = Console.ReadLine().ToLower();
    switch (chosenSign)
    {
        case "a":
            ListOutTheArticles(MakeArticleList(Articles), Articles);
            break;
        case "b":
            var list = MakeArticleList(Articles);
            list.Sort();
            ListOutTheArticles(list, Articles);
            break;
        case "c":
            ListOutTheArticlesUsingDates(MakeDatesList(Articles), Articles);
            break;
        case "d":
            var listOfDates = MakeDatesList(Articles);
            listOfDates.Reverse();
            ListOutTheArticlesUsingDates(listOfDates, Articles);
            break;
        case "e":
            ListOutTheArticlesUsingAmount(MakeAmountList(Articles), Articles);
            break;
        case "f":
            BestSeller(SoldPieces(Articles, Reciepts), Articles);
            break;
        case "g":
            WorstSeller(SoldPieces(Articles, Reciepts), Articles);
            break;
        case "0":
            BeginningPage(Articles, Workers, Reciepts);
            break;
        default:
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