using skcyDMSCataloguing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace skcyDMSCataloguing.ViewModels
{
    public class AssignedAccountData
    {
        public int CIFID { get; set; }
        public string CIFNo { get; set; }
        public string CIFCustomerName { get; set; }
        public bool Assigned { get; set; }
        
    }
}
