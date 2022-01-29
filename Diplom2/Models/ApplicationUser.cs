using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Diplom2.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public bool Status { get; set; } = false;

        public virtual ICollection<Collection> Collections { get; set; }

        public virtual ICollection<Like> Likes { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual bool ThemeSite { get; set; }
    }
}
