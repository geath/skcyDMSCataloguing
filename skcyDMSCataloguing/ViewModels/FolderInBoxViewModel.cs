using skcyDMSCataloguing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace skcyDMSCataloguing.ViewModels
{
    public class FolderInBoxViewModel
    {
        public int BoxID { get; set; }
        public IEnumerable<Folder> Folders { get; set; }
        public IEnumerable<CustData> CustDatas { get; set; }
     
    }
}