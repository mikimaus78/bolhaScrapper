using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace bolhaScrapper.Data
{
    class ScrapeData
    {
        public ScrapeData()
        {
            Parts = new List<ScrapeDataDeli>();
        }

        public string Data { get; set; }
        public string Regex { get; set; }
        public RegexOptions RegexOption{ get; set; }
        public List<ScrapeDataDeli> Parts { get; set; }

    }
}
