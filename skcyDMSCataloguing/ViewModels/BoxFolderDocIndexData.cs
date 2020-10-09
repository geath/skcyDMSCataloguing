using skcyDMSCataloguing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace skcyDMSCataloguing.ViewModels
{
    public class BoxFolderDocIndexData
    {
        public IEnumerable<Box> Boxes { get; set; }
        public IEnumerable<Folder> Folders { get; set; }
        public IEnumerable<CustData> CustDatas { get; set; }
    }
}
