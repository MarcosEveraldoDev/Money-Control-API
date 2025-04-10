using ACBrLib.Boleto;
using ACBrLib.Core.Boleto;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Boleto;
namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TicketController : Controller
    {

        Config config = new Config();

        [HttpPost("GerarBoleto")]
        public async Task<IActionResult> GerarPdf([FromServices] ACBrBoleto boleto, [FromServices] IWebHostEnvironment hostingEnvironment)
        {
            var codigo = "Boleto";
            var path = Path.GetTempPath();
            var nomeArquivo = $@"{codigo}.pdf";

            boleto.Config.Impressao.MostrarPreview = false;
            boleto.Config.Impressao.MostrarProgresso = false;
            boleto.Config.Impressao.MostrarSetup = false;
            boleto.Config.Impressao.DirLogo = Path.Combine(hostingEnvironment.ContentRootPath, "Logos");

            boleto.Config.Impressao.NomeArquivo = Path.Combine(path, nomeArquivo);


            boleto.IncluirTitulos(config.ini, BoletoTpSaida.PDF);

            var fs = new FileStream(Path.Combine(path, nomeArquivo), FileMode.Open, FileAccess.Read);

            return File(fs, "application/pdf", nomeArquivo);

        }


    }
}
