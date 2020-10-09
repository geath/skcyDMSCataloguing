using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace skcyDMSCataloguing.Models
{
    public class CustData
    {
       
        public int ID { get; set; }
        public string CIFNo { get; set; }
        public string CustomerName { get; set; }
        public string CustomerNo { get; set; }
        public string CustomerIDN { get; set; }       

        public ICollection<CustRelData> CustRelDataEntries { get; set; }

        public ICollection<Folder> Folders { get; set; }
        public ICollection<PrjHelix1> PrjHelixes1 { get; set; }
        public ICollection<PrjVelocity1> PrjVelocities1 { get; set; }
        public ICollection<PrjVelocity2> PrjVelocities2 { get; set; }
    }
}