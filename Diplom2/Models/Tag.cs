using System.Collections.Generic;

namespace Diplom2.Models
{
    public class Tag
    {
        public int TagId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<TagItem> Items { get; set; }
        
    }
}
