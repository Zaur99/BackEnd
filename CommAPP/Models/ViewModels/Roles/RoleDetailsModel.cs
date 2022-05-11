
using Comm.DataAccess;
using Comm.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommAPP.Models.ViewModels.Roles
{
    public class RoleDetailsModel
    {
      
        public IdentityRole Role { get; set; }
        public List<ApplicationUser> Members { get; set; }
        public List<ApplicationUser> NonMembers { get; set; }
    }
}
