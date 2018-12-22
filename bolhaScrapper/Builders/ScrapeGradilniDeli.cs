using bolhaScrapper.Data;
using System;
using System.Text.RegularExpressions;

namespace bolhaScrapper.Builders
{
    class ScrapeGradilniDeli
    {
        private string _regex { get; set; }
        private RegexOptions _regexOption { get; set; }

        public ScrapeGradilniDeli()
        {
            SetDefaults();
        }

        private void SetDefaults()
        {
            _regex = string.Empty;
            _regexOption = RegexOptions.None;
        }

        public ScrapeGradilniDeli WithRegex (string regex)
        {
            _regex = regex;
            return this;
        }
        public ScrapeGradilniDeli WithRegexOption (RegexOptions regexOption)
        {
            _regexOption = regexOption;
            return this;
        }

        public ScrapeDataDeli Builder()
        {
            ScrapeDataDeli scrapeDataDeli = new ScrapeDataDeli();
            scrapeDataDeli.Regex = _regex;
            scrapeDataDeli.RegexOption = _regexOption;
            return scrapeDataDeli;
        }
    }
}