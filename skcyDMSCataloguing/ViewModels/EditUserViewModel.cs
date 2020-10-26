using Microsoft.AspNetCore.Authorization.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace skcyDMSCataloguing.ViewModels
{
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            Claims = new List<string>();
            Roles = new List<string>();
        }

        public string Id { get; set; }
        [Required]
        public string UserEmail { get; set; }
        [Required]
        [EmailAddress]
        public string UserName { get; set; }

        public List<string> Claims { get; set; }
        public IList<string> Roles { get; set; }
    }
}
