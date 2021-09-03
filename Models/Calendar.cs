using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trademarkia.Models
{
    public class Calendar
    {
        public int eventid { get; set; }

        public string eventtitle { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
        public string? eventtime { get; set; }


    }
}
