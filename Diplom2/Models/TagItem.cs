namespace Diplom2.Models
{
    public class TagItem
    {
        public int TagId { get; set; }
        public virtual Tag Tag { get; set; }


        public int ItemId { get; set; }
        public virtual Item Item { get; set; }
    }
}
