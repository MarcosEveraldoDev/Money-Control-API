using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Data;
using WebApplication2.Services;

namespace WebApplication2.Controllers
{
    [DisableCors]
    public class StockController : Controller
    {

        private readonly StockService _stockService;
        private readonly AppDbContext _context;

        public StockController(StockService stockService, AppDbContext context)
        {
            _stockService = stockService;
            _context = context;
        }

        [HttpPut("UpdateStock/{id}")]
        public async Task<IActionResult> UpdateStock(int id, int quantity)
        {
            var product = await _stockService.Update(id, quantity);

            var objectReponse = new
            {
                Status = "Success",
                Message = "Estoque atualizado com sucesso!",
                Product = product
            };

            return Json(objectReponse);
        }

        [HttpGet("GetStock/{id}")]
        public async Task<IActionResult> GetStock(int id)
        {
            var stockBalance = await _stockService.GetStockBalance(id);

            var objectReponse = new
            {
                Status = "Success",
                Message = "Estoque atualizado com sucesso!",
                StockBlance = stockBalance
            };

            return Json(objectReponse);
        }
    }
}
