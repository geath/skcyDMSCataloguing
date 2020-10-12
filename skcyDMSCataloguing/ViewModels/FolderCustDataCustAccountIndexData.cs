using skcyDMSCataloguing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace skcyDMSCataloguing.ViewModels
{
    public class FolderCustDataCustAccountIndexData
    {
        public IEnumerable<Folder> Folders { get; set; }
        public IEnumerable<CustData> CustDatas { get; set; }
        public IEnumerable<CustAccData> CustAccDatas { get; set; }

    }
}
