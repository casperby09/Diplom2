using System.Collections.Generic;

namespace Diplom2.Models
{
    public class Date
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual Collection Collection { get; set; }
        
        public virtual ICollection<ValueDate> ValueDates { get; set; }
    }
}
