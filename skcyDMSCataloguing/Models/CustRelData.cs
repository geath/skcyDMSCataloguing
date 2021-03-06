﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace skcyDMSCataloguing.Models
{
    public class CustRelData
    {
        public int ID { get; set; }
        
        public string CustOldAccountNo {get; set;}
        public string CustAccountType {get; set;}
        public string CustAccountStatus {get; set;}
        public string CustAccountRelationCode {get; set;}

        public string CustCIFNo {get; set;}
        public CustData CustData {get; set;}

        public string CustAccountNo {get; set;}
        public  CustAccData CustAccData {get; set;}                        
    }
}