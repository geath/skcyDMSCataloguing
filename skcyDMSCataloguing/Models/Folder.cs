using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Required]
        [Remote("CIFHandledBy",controller:"Folder")]        
        public string CustDataCIFNo { get; set; }
        public CustData CustData { get; set; }
    }
}
