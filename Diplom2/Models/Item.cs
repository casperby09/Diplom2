using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Diplom2.Models
{
    public class Item
    {
        public int ItemId { get; set; }

        public string ItemName { get; set; }

        public virtual ICollection<TagItem> Tags { get; set; }

        public virtual Collection Collection { get; set; }

        public virtual ICollection<ValueNumber> ValueNumbers { get; set; }

        public virtual ICollection<ValueLine> ValueLines { get; set; }

        public virtual ICollection<ValueText> ValueTexts { get; set; }

        public virtual ICollection<ValueDate> ValueDates { get; set; }

        public virtual ICollection<ValueLogical> ValueLogicals { get; set; }

        public virtual ICollection<Like> Likes { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
