using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bolhaScrapper.Data;
using System.Text.RegularExpressions;



namespace bolhaScrapper.Workers
{
    class Scraper
    {
        public List<string> Scrape(ScrapeData scrapeData)
        {
            List<string> scrapedElementi = new List<string>();

            MatchCollection matches = Regex.Matches(scrapeData.Data, scrapeData.Regex, scrapeData.RegexOption);

            foreach(Match match in matches)
            {
                if (!scrapeData.Parts.Any())
                {
                    // dodamo v list nov prvi value iz match vrednosti
                    scrapedElementi.Add(match.Groups[0].Value);
                }
                else
                {
                    foreach(var part in scrapeData.Parts)
                    {
                        Match matchedPart = Regex.Match(match.Groups[0].Value, part.Regex, part.RegexOption);

                        if (matchedPart.Success)
                        {
                            scrapedElementi.Add(matchedPart.Groups[1].Value);
                        }
                    }
                }
            }
            return scrapedElementi;


        }
    }
}
