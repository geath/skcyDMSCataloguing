using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace skcyDMSCataloguing.ViewModels
{
    public class RolesAssignedToUserViewModel
    {
        public string RoleId { get; set; }       
        public string RoleName { get; set; }
        public bool IsSelected { get; set; }
    }
}
