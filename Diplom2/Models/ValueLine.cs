namespace Diplom2.Models
{
    public class ValueLine
    {
        public int Id { get; set; }

        public string Value { get; set; }

        public virtual Line Line { get; set; }

        public virtual Item Item { get; set; }
    }
}
