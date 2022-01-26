using System;
using System.ComponentModel.DataAnnotations;

namespace Diplom2.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }

        public DateTime DateTime { get; set; }

        public ApplicationUser User { get; set; }

        public Item Item { get; set; }

        
    }
}
