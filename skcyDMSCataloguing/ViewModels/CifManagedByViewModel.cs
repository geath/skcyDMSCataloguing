using skcyDMSCataloguing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace skcyDMSCataloguing.ViewModels
{
    public class CifManagedByViewModel
    {
        public CustData CustData { get; set; }
        public PrjHelix1 PrjHelix1 { get; set; }
        public PrjVelocity1 PrjVelocity1 { get; set; }
        public PrjVelocity2 PrjVelocity2 { get; set; }

        public bool IsHelix1 { get; set; }
        public bool IsVelocity1 { get; set; }
        public bool IsVelocity2 { get; set; }
    }
}


