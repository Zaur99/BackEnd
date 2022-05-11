using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CommAPP.Models.ViewModels.Roles
{
    public class RoleModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}
