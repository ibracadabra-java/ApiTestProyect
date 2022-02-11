using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiTestProyect.Models
{
    public class Market
    {
        public string symbol { get; set; }
        public string name { get; set; }
        public string country { get; set; }
        public string industry { get; set; }
        public int ipoYear { get; set; }
        public long marketCap { get; set; }
        public string sector { get; set; }
        public int volume { get; set; }
        public decimal netChange { get; set; }
        public decimal netChangePercent { get; set; }        
        public decimal lastPrice { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        public int id { get; set; }

    }
}
