using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Web;

namespace skcyDMSCataloguing.Models
{
    public class CustData
    {
       
        public int ID {get; set;}    
        [Required]
      
        public string CIFNo {get; set;}
        public string CustomerName {get; set;}
        public string CustomerNo {get; set;}
        public string CustomerIDN {get; set;}
        public CardType CardType { get; set; }
        public string OldCIFNo { get; set; }
        public CustomerActive CustomerActive { get; set; }
        public CardActive CardActive { get; set; }
        public DMSProject DMSProject { get; set; }


        public ICollection<CustRelData> CustRelDataEntries {get; set;}

        public ICollection<Folder> Folders {get; set;}
        public ICollection<PrjHelix1> PrjHelixes1 {get; set;}
        public ICollection<PrjVelocity1> PrjVelocities1 {get; set;}
        public ICollection<PrjVelocity2> PrjVelocities2 {get; set;}
    }

    public enum CardType
    {
        CREDIT,
        DEBIT,
        PREPAID,
        UNKNOWN,
        NonApplicable
    }

    public enum CustomerActive
    {
        ACTIVE,
        INACTIVE,
        UNKNOWN
    }

    public enum CardActive
    {
        ACTIVE,
        INACTIVE,
        UNKNOWN
    }

    public enum DMSProject
    {
        BOCNPLs,
        BOCCards,
        UNKNOWN
    }
}