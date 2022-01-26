namespace Diplom2.Models
{
    public class ValueNumber
    {
        public int Id { get; set; }

        public double Value { get; set; }

        public virtual Number Number { get; set; }

        public virtual Item Item { get; set; }
    }
}
