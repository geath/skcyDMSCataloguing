using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace skcyDMSCataloguing.Models
{
    public static class ClaimsStore
    {
        public static List<Claim> AllClaims = new List<Claim>()
        {
            
            new Claim("Edit User","Edit User"),
            new Claim("Delete User","Delete User"),
            new Claim("View User","View User"),
            new Claim("Create AdmEntity","Create AdmEntity"),
            new Claim("Modify AdmEntity","Modify AdmEntity"),
            new Claim("Delete AdmEntity","Delete AdmEntity"),
            new Claim("View AdmEntity","View AdmEntity"),

            new Claim("Create BusEntity","Create BusEntity"),
            new Claim("Modify BusEntity","Modify BusEntity"),
            new Claim("Delete BusEntity","Delete BusEntity"),
            new Claim("View BusEntity","View BusEntity")
        };
    }
}
