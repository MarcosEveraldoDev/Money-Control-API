using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models.DTOs;
using WebApplication2.Services;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {

        private readonly ProductsService _productsService;
        private readonly StockService _stockService;

        public ProductsController(ProductsService productsService, StockService stockService)
        {
            _productsService = productsService;
            _stockService = stockService;
        }


        [HttpGet("GetId")]
        [EnableCors("MyAllowSpecificOrigins")]
        //  [Authorize("User , Admin , Bronze , Prata , Ouro")]
        public async Task<IActionResult> GetId(int id)
        {

            var product = await _productsService.GetId(id);

            return Json(product);
        }

        [HttpGet("GetAll")]
        [DisableCors]

        //    [Authorize("User , Admin , Bronze , Prata , Ouro")]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productsService.GetAll();

            return Json(products);
        }

        [HttpPost("Register")]
        [EnableCors("MyAllowSpecificOrigins")]

        // [Authorize("User , Admin , Bronze , Prata , Ouro")]
        public async Task<IActionResult> Register([FromBody] ProductsDTO products)
        {
            try
            {
                var product = await _productsService.Register(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Json(products);
        }

        [HttpPut("Update")]
        [EnableCors("MyAllowSpecificOrigins")]

        // [Authorize("User, Admin, Bronze, Prata, Ouro")]
        public async Task<IActionResult> UpdateId(int id, [FromBody] ProductsDTO products)
        {
            try
            {
                var product = await _productsService.Update(id, products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Json(products);
        }

        [HttpDelete("Delete")]
        [EnableCors("MyAllowSpecificOrigins")]

        // [Authorize("User, Admin, Bronze, Prata, Ouro")]
        public async Task<IActionResult> DeleteId(int id)
        {
            try
            {
                var product = await _productsService.Delete(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Json("Produto deletado com sucesso!");
        }



    }
}
