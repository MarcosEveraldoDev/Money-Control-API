using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Data;
using WebApplication2.Models;
using WebApplication2.Models.DTOs;
using WebApplication2.Services;

namespace WebApplication2.Controllers
{

    [ApiController]
    [Route("api/[Controller]")]
    [DisableCors]
    public class PayController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SaleService _salesService;
        public PayController(UserManager<User> userManager,
                               RoleManager<IdentityRole> roleManager,
                               AppDbContext context, SaleService salesService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _salesService = salesService;
        }

        [HttpPost("confirm-payment-plan")]
        public async Task<IActionResult> Pay(string Id, string level)
        {

            var user = await _userManager.FindByIdAsync(Id);

            if (user == null)
                return NotFound("Usuario nao Encontrado");

            switch (level)
            {
                case "Bronze":
                    await _userManager.AddToRoleAsync(user, "Bronze");
                    break;
                case "Prata":
                    await _userManager.AddToRoleAsync(user, "Prata");
                    break;
                case "Ouro":
                    await _userManager.AddToRoleAsync(user, "Ouro");
                    break;
                default:
                    return BadRequest("Invalid level");
            }

            return Json(user);
        }

        [HttpPost("purchase")]
        public async Task<IActionResult> Purchase([FromBody] PurchaseDTO purchase)
        {
            var result = await _salesService.ProcessSaleAsync(purchase);

            return Json(result);
        }

        [HttpPost("cancel-purchase")]
        public async Task<IActionResult> CancelPurchase([FromBody] int saleId)
        {
            var result = await _salesService.CancelSaleAsync(saleId);
            return Json(result);
        }
    }
}
