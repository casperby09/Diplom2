namespace Diplom2.Models
{
    public class Like
    {
        public int Id { get; set; }
        public Item Item { get; set; }
        public ApplicationUser User { get; set; }
    }
}
