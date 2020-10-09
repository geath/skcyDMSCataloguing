using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
 

namespace skcyDMSCataloguing.Models
{
    public class Box
    {
        public int ID { get; set; }
        public string BoxDescription  { get; set; }
        
        public DateTime DateBoxCreated { get; set; }
        public int BoxCreatedBy { get; set; }

        public int BoxCreatorID { get; set; }
        public virtual BoxCreator BoxCreator { get; set; }

        public ICollection<Folder> Folders { get; set; }        
    }
}