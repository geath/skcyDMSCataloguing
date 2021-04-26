using skcyDMSCataloguing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace skcyDMSCataloguing.ViewModels
{
    public class AssignedAccountData
    {
        
        public string CIFNo { get; set; }
        public string CIFCustomerName { get; set; }
        public bool Assigned { get; set; }
        public int BoxID { get; set; }

        public string OldCIFNo { get; set; }
        public string CardTypePr { get; set; }
        public string CardActivePr { get; set; }
        public string ManagedBy { get; set; }

    }
}
