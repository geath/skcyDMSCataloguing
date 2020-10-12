using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace skcyDMSCataloguing.Models
{
    public class CustRelData
    {
        public int ID { get; set; }
              
        public int CustDataID { get; set; }
        public CustData CustData { get; set; }

        public int CustAccDataID { get; set; }
        public  CustAccData CustAccData { get; set; }
                        
    }
}