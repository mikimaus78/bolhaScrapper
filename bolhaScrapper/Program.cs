using bolhaScrapper.Builders;
using bolhaScrapper.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using bolhaScrapper.Workers;
using System.Globalization;

namespace bolhaScrapper
{
    class Program
    {
        static void Main(string[] args)
        {
            /* Program displaya pozdrav in uporabnost programa
             * Program displaya vse opcije katere informacije lahko dobiš od bolhe - case switch uporaba?
             * Uporabnik izbere eno opcijo - verjetno a, b, c, d, e, f,... zaenkrat narediti 1 opcijo, ki deluje in kasneje dodal več funkcionalnosti
             * program zamenja črko a - za pripravljeno besedo, ki se uporabi v programu za pridobitev informacij
             * iz bolhe potegnemo ven informacije z webscrappanjem - http://prntscr.com/lxjs7j  to je primer uporabe pri kategoriji http://www.bolha.com/racunalnistvo
             * 
             * z builderjem in data bomo pripravili informacije, ki jo nato integriramo v worker mapo Scraper, ki jo uporabimo za extraction informacij z vsemi elementi
             * builder mapa za metode ki se uporabljajo za grajenje
             * data mapa za hranjenje get set gradnikov, ki jih uporabimo v builder mapi - uporabljamo polymorphisem za kreiranje Build metode
             * worker mapa 
             * 
             * 
             * bolj detailiran postopek https://www.evernote.com/shard/s473/sh/297963ae-63f8-4741-88c3-515d935a2c78/79b96e254cfe2f152015368bada151c9
             * 
             * prikaz vseh zadetkov? prvih 5?
             * 
             */

            try
            {
                string[] str = new string[]
                     {
                            @" _           _ _            _____                                      ",
                            @"| |         | | |          / ____|                                     ",
                            @"| |__   ___ | | |__   __ _| (___   ___ _ __ __ _ _ __  _ __   ___ _ __ ",
                            @"| '_ \ / _ \| | '_ \ / _` |\___ \ / __| '__/ _` | '_ \| '_ \ / _ \ '__|",
                            @"| |_) | (_) | | | | | (_| |____) | (__| | | (_| | |_) | |_) |  __/ |   ",
                            @"|_.__/ \___/|_|_| |_|\__,_|_____/ \___|_|  \__,_| .__/| .__/ \___|_|   ",
                            @"                                                | |   | |              ",
                            "                                                |_|   |_|              \n"
                     };
                var index = 3;
                foreach (var item in str)
                {
                    for (int i = 0; i < item.Length; i++)
                    {

                        Console.Write(item[i]);

                        Console.ForegroundColor = (ConsoleColor)index;
                        index++;
                        if (index == 15)
                            index = 3;
                        if (i == item.Length - 1)
                        {
                            Console.Write("\n");
                            continue;
                        }
                    }
                }

                Console.WriteLine("Pozdravljeni v programu WebScrapanja na Slovenskem ozemlju.\nWebScrappamo spletno stran www.bolha.com\nTrenutno možnosti webscrappanja so naslednje - prosimo za natančno upoštevanje navodil za pravilno delovanje!\n");
                Console.WriteLine("Prosimo izberite eno od spodnjih opcij z vpisom črke pred željeno kategorijo!");
                Console.WriteLine("# a - Nepremicnine\n# b - Avto-Moto\n# c - Računalništvo\n");
                Console.Write("Vaša črka je : ");
                var kategorija = Console.ReadLine();
                switch (kategorija)
                {
                    case "a":
                        kategorija = "nepremicnine/";
                        break;
                    case "b":
                        kategorija = "avto-moto/?listingType=list";
                        break;
                    case "c":
                        kategorija = "racunalnistvo/";
                        break;
                    default:
                        Console.WriteLine("Niste upoštevali pravil! Poiskusi ponovno!");
                        break;
                }


                using (WebClient client = new WebClient())
                {
                    client.Headers["User-Agent"] = "Mozilla/5.0 (Windows; U; MSIE 9.0; Windows NT 9.0; en-US)";

                    string content = client.DownloadString($"http://www.bolha.com/{kategorija}");

                    ScrapeData scrapeData = new ScrapeGradilec()
                        .WithData(content)
                        .WithRegex(@"<h3><a title=\""(.*?)\"" href=\""(.*?)\"">(.*?)</a></h3>")
                        .WithRegexOption(RegexOptions.ExplicitCapture)
                        .WithPart(new ScrapeGradilniDeli()
                            .WithRegex(@"title=\""(.*?)\""")
                            .WithRegexOption(RegexOptions.Singleline)
                            .Builder())

                        .WithPart(new ScrapeGradilniDeli()
                            .WithRegex(@"href=\""(.*?)\""")
                            .WithRegexOption(RegexOptions.Singleline)
                            .Builder())
                        
                        .Build();

                    Scraper scraper = new Scraper();

                    var scrapedElementi = scraper.Scrape(scrapeData);

                    if (scrapedElementi.Any())
                    {
                        foreach(var scrapedElement in scrapedElementi)
                        {
                            CultureInfo ci = new CultureInfo("en-US");
                            bool result = scrapedElement.StartsWith("http", true, ci);
                            bool resultSlash = scrapedElement.StartsWith("/", true, ci);

                            if (result)
                            {
                                Console.OutputEncoding = System.Text.Encoding.ASCII;
                                Console.WriteLine(scrapedElement + "\n");
                            }
                            else if (resultSlash)
                            {
                                Console.OutputEncoding = System.Text.Encoding.ASCII;
                                Console.WriteLine("http://www.bolha.com/" + scrapedElement + "\n");
                            }
                            else
                            {
                                Console.OutputEncoding = System.Text.Encoding.ASCII;
                                Console.WriteLine("##############################################################\n");
                                Console.WriteLine(scrapedElement + "\n");
                            }

                        }
                    }
                    else
                    {
                        Console.WriteLine("Nismo našli ničesar :(");
                    }
                        
                    Console.WriteLine("\n\n\n\nNašli smo {0} zadetkov!\n###########################################################", scrapedElementi.Count);

                }
                Console.ReadLine();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }

    }
}



//string searching = "<div class=\"ad featured\"><div class=\"coloumn image\"><table><tbody><tr><td><a title=\""(.*?)"" href=\"(.*?)"">(.*?)<div class="price"><span>(.*?)</span></div>(.*?)</div>"




//.WithRegex(@"<div class=\""ad featured\"">(?:[^\""']|\""[^\""]*\""|'[^']*')*<h3><a title=\""(.*?)\"" href=\""(.*?)\"">(.*?)<\/a><\/h3>(?:[^\\""']|\""[^\""]*\""|'[^']*')*<div class=\""price\""><span>(.*?)<\/span><\/div>")


//<h3><a title=\""(.*?)\"" href=\""(.*?)\"">(.*?)</a></h3>


//string roflmao = "<div class=\"ad featured\">(.*?)<a title=\"(.*?)"" href=\""(.*?)"">(.*?)</a>(.*?)<div class=\""price\""><span>(.*?)</span></div>(.*?)</div>"