namespace WebApplication2.Models.DTOs
{
    public class PurchaseDTO
    {
        public string UserId { get; set; }
        public List<PurchaseItemDTO> Items { get; set; } = new();
        public string PaymentMethod { get; set; }

    }
}
