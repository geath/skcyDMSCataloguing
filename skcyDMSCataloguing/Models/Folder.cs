﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace skcyDMSCataloguing.Models
{
    public class Folder
    {
        public int ID { get; set; }

        public string FolderName { get; set; }
        public string FolderDescription { get; set; }

        public int BoxID { get; set; }
        public Box Box { get; set; }

        public int CustDataID { get; set; }
        public CustData CustData { get; set; }
    }
}
