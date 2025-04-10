namespace WebApplication2.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = "Confirmed";  
        public DateTime SaleDate { get; set; } = DateTime.UtcNow;
        public string PaymentMethod { get; set; } = "Credit Card";

        // Relacionamento com a tabela SaleItem
        public List<SaleItem> Items { get; set; } = new List<SaleItem>();
    }

}
