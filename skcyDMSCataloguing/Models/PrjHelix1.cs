using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace skcyDMSCataloguing.Models
{
    public class PrjHelix1
    {
        public int ID { get; set; }        
        public string Helix1Pool { get; set; }

        public string CustDataCIFNo { get; set; }
        public CustData CustData { get; set; }
    }
}
