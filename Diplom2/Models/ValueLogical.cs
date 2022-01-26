namespace Diplom2.Models
{
    public class ValueLogical
    {
        public int Id { get; set; }

        public bool Value { get; set; }

        public virtual Logical Logical { get; set; }

        public virtual Item Item { get; set; }
    }
}
