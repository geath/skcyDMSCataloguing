using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace skcyDMSCataloguing.Models
{
    public class BoxCreator
    {
        public int ID { get; set; }
        public int CreatorSynthesisID { get; set; }
        public string CreatorName { get; set; }

        public  ICollection<Box> Boxes { get; set; }
    }
}