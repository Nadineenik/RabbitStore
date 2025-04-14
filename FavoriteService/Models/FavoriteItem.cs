namespace FavoriteService.Models
{
    public class FavoriteItem
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int RabbitId { get; set; }
        public int Quantity { get; set; } = 1;
    }
}
