using bolhaScrapper.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace bolhaScrapper.Builders
{
    class ScrapeGradilec
    {
        private string _data;
        private string _regex;
        private RegexOptions _regexOption;
        private List<ScrapeDataDeli> _part;

        public ScrapeGradilec()
        {
            SetDefaults();
        }

        private void SetDefaults()
        {
            _data = string.Empty;
            _regex = string.Empty;
            _regexOption = RegexOptions.None;
            _part = new List<ScrapeDataDeli>();
        }

        public ScrapeGradilec WithData(string data)
        {
            _data = data;
            return this;
        }
        public ScrapeGradilec WithRegex(string regex)
        {
            _regex = regex;
            return this;
        }
        public ScrapeGradilec WithRegexOption(RegexOptions regexOption)
        {
            _regexOption = regexOption;
            return this;
        }
        public ScrapeGradilec WithPart(ScrapeDataDeli bolhaPart)
        {
            _part.Add(bolhaPart);
            return this;
        }

        public ScrapeData Build()
        {
            ScrapeData scrapeData = new ScrapeData();
            scrapeData.Data = _data;
            scrapeData.Regex = _regex;
            scrapeData.RegexOption = _regexOption;
            scrapeData.Parts = _part;
            return scrapeData;
        }
    }
}
