using System.Collections.Generic;

namespace Diplom2.Models
{
    public class Logical
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual Collection Collection { get; set; }

        public virtual ICollection<ValueLogical> ValueLogicals { get; set; }
    }
}
