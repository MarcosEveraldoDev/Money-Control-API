namespace WebApplication2.Models
{
    public class SaleItem
    {
        public int Id { get; set; }
        public int SaleId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }

        // Relacionamento com a tabela Sale
        public Sale Sale { get; set; }
        public Products Product { get; set; }
    }

}
