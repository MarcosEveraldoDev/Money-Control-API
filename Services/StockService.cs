using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Services
{
    public class StockService
    {

        private readonly AppDbContext _context;

        public StockService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Products> Update(int id, int quantity)
        {

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                throw new Exception("Produto nao encontrado");
            }
            product.Quantity = quantity;
            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<Decimal> GetStockBalance(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                throw new Exception("Produto nao encontrado");
            }

            var stock = product.Price * product.Quantity;

            return stock;
        }
    }
}
