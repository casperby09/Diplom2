namespace Diplom2.Models
{
    public class ValueText
    {
        public int Id { get; set; }

        public string Value { get; set; }

        public virtual Text Text { get; set; }

        public virtual Item Item { get; set; }
    }
}
