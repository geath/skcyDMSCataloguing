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
        public int CountedBoxes { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}
