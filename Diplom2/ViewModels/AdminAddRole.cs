using Diplom2.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Diplom2.ViewModels
{
    public class AdminAddRole
    {
        public ApplicationUser User { get; set; }

        public List<IdentityRole> Role { get; set; }

        public IList<string> RoleUser { get; set; }
    }
}
