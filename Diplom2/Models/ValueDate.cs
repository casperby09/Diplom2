using System;

namespace Diplom2.Models
{
    public class ValueDate
    {
        public int Id { get; set; }

        public DateTime Value { get; set; }

        public virtual Date Date { get; set; }

        public virtual Item Item { get; set; }
    }
}
