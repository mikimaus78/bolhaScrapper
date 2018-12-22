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
                Console.WriteLine("Katero kategorijo želite iskati?\na - nepremicnine");
                //var prvaKategorija = Console.ReadLine();


                using (WebClient client = new WebClient())
                {
                    client.Headers["User-Agent"] = "Mozilla/5.0 (Windows; U; MSIE 9.0; Windows NT 9.0; en-US)";

                    string content = client.DownloadString("http://www.bolha.com/nepremicnine/");

                    ScrapeData scrapeData = new ScrapeGradilec()
                        .WithData(content)
                        .WithRegex(@"<div class=\""ad featured\"">(?:[^\""']|\""[^\""]*\""|'[^']*')*<h3><a title=\""(.*?)\"" href=\""(.*?)\"">(.*?)<\/a><\/h3>(?:[^\\""']|\""[^\""]*\""|'[^']*')*<div class=\""price\""><span>(.*?)<\/span><\/div>")
                        .WithRegexOption(RegexOptions.ExplicitCapture)
                        .WithPart(new ScrapeGradilniDeli()
                            .WithRegex(@"title=\""(.*?)\""")
                            .WithRegexOption(RegexOptions.Singleline)
                            .Builder())

                        .WithPart(new ScrapeGradilniDeli()
                            .WithRegex(@"<span>(.*?)</span>")
                            .WithRegexOption(RegexOptions.Singleline)
                            .Builder())
                        
                        .Build();

                    Scraper scraper = new Scraper();

                    var scrapedElementi = scraper.Scrape(scrapeData);

                    if (scrapedElementi.Any())
                    {
                        foreach(var scrapedElement in scrapedElementi)
                        {
                            Console.WriteLine(scrapedElement + "\n");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Nismo našli ničesar :(");
                    }
                        

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









//string roflmao = "<div class=\"ad featured\">(.*?)<a title=\"(.*?)"" href=\""(.*?)"">(.*?)</a>(.*?)<div class=\""price\""><span>(.*?)</span></div>(.*?)</div>"