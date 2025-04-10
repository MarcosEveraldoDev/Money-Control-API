using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;
using WebApplication2.Models.DTOs;

namespace WebApplication2.Services
{
    public class ProductsService
    {

        private readonly AppDbContext _context;

        public ProductsService(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Products?> GetId(int id)
        {
            var product = await _context.Products.FindAsync(id);
            return product;
        }

        public async Task<List<Products>> GetAll()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Products> Register(ProductsDTO products)
        {

            if (products == null)
            {
                throw new ArgumentNullException(nameof(products));
            }

            var product = new Products

            {
                Name = products.Name,
                Price = products.Price,
                Description = products.Description,
                Category = products.Category,
                Quantity = products.Quantity,
                Unity = products.Unity

            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<Products> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                throw new Exception("Product not found");
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<Products> Update(int id, ProductsDTO products)
        {

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                throw new Exception("Produto não encontrado");
            }

            product.Name = products.Name;
            product.Price = products.Price;
            product.Description = products.Description;
            product.Category = products.Category;
            product.Quantity = products.Quantity;
            product.Unity = products.Unity;

            await _context.SaveChangesAsync();

            return product;

        }

    }
}
