using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace skcyDMSCataloguing.Models
{
    public class PrjVelocity2
    {
        public int ID { get; set; }

        public string CustDataCIFNo { get; set; }
        public CustData CustData { get; set; }
    }
}
