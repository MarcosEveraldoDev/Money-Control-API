using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Models;
using WebApplication2.Models.DTOs;

namespace WebApplication2.Services
{
    public class SaleService
    {

        private readonly AppDbContext _context;

        public SaleService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<object> ProcessSaleAsync(PurchaseDTO purchase, bool isCancel = false)
        {
            if (purchase == null || purchase.Items == null || !purchase.Items.Any())
            {
                return new { Status = "Error", Message = "Dados inválidos!" };
            }

            decimal totalPrice = 0;
            var saleItems = new List<object>();
            var saleStatus = "Completed";

            if (isCancel)
            {
                saleStatus = "Cancelled";
            }

            foreach (var item in purchase.Items)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product == null)
                {
                    return new { Status = "Error", Message = $"Produto com ID {item.ProductId} não encontrado!" };
                }

                var totalItem = product.Price * item.Quantity;
                totalPrice += totalItem;

                if (isCancel)
                {
                    product.Quantity += item.Quantity;
                }
                else
                {
                    product.Quantity -= item.Quantity;
                }

                _context.Products.Update(product);

                saleItems.Add(new
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price,
                    Total = totalItem
                });
            }

            var sale = new Sale
            {
                UserId = purchase.UserId,
                SaleDate = DateTime.Now,
                Status = saleStatus,
                TotalPrice = totalPrice,
                Items = saleItems.Select(si => new SaleItem
                {
                    ProductId = (int)((dynamic)si).ProductId,
                    Quantity = (int)((dynamic)si).Quantity,
                    UnitPrice = (decimal)((dynamic)si).UnitPrice,
                    TotalPrice = (decimal)((dynamic)si).Total
                }).ToList()
            };

            if (saleStatus == "Completed")
            {
                _context.Sale.Add(sale);
            }
            else if (saleStatus == "Cancelled")
            {
                _context.Sale.Update(sale);
            }

            await _context.SaveChangesAsync();

            return new
            {
                Status = saleStatus,
                UserId = purchase.UserId,
                Items = saleItems,
                TotalPrice = totalPrice
            };
        }



        public async Task<object> CancelSaleAsync(int saleId)
        {
            var sale = await _context.Sale
                .Include(s => s.Items)
                .FirstOrDefaultAsync(s => s.Id == saleId);

            if (sale == null)
            {
                return new { Status = "Error", Message = "Venda não encontrada!" };
            }

            if (sale.Status == "Canceled")
            {
                return new { Status = "Error", Message = "Venda já foi cancelada." };
            }

            sale.Status = "Canceled";

            foreach (var item in sale.Items)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product != null)
                {
                    product.Quantity += item.Quantity;
                }
            }

            await _context.SaveChangesAsync();

            return new { Status = "Success", Message = "Venda cancelada com sucesso!" };
        }
    }
}
