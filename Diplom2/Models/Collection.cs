using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Diplom2.Models
{
    public class Collection
    {
        public int CollectionId { get; set; }

        [Required]
        public string CollectionName { get; set; }

        [Required]
        [MaxLength(255)]
        public string ShortDescription { get; set; }

        [Required]
        public string Theme { get; set; }

        public string PhotoURL { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ICollection<Item> Items { get; set; }

        public virtual ICollection<Number> Numbers { get; set; }

        public virtual ICollection<Line> Lines { get; set; }

        public virtual ICollection<Text> Texts { get; set; }

        public virtual ICollection<Date> Dates { get; set; }

        public virtual ICollection<Logical> Logicals { get; set; }
    }
}
