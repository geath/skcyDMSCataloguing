﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace skcyDMSCataloguing.Models
{
    public class CustAccData
    {
     
        public int ID { get; set; }
        public string CustAccountNo { get; set; }
        
        public ICollection<CustRelData> CustRelDataEntries { get; set; }

       
    }
}