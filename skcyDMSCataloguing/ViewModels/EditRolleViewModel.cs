using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace skcyDMSCataloguing.ViewModels
{
    public class EditRolleViewModel
    {

        public EditRolleViewModel()
        {
            Users = new List<string>();
            /* if the collection is not initialized , when calling @if (Model.Users.Any())
             * from EditRole View it will throw an exeption because we try to call any 
             * in a null object
             */
        }


        public string Id { get; set; }

        [Required(ErrorMessage ="Role Name is required")]
        public string RoleName { get; set; }

        public List<string> Users { get; set; }
    }
}
