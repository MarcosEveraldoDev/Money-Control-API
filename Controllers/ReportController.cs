using ClosedXML.Excel;
using FastReport;
using FastReport.Data;
using FastReport.Export.PdfSimple;
using FastReport.Utils;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Data;
using WebApplication2.Services;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [DisableCors]
    public class ReportController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ReportService _relatorioService;

        public ReportController(AppDbContext context, ReportService relatorioService)
        {
            _context = context;
            _relatorioService = relatorioService;
        }

        [HttpGet("Sales-QuestPDF")]
        public async Task<IActionResult> GerarRelatorioVendas()
        {
            var vendas = await _relatorioService.ObterItensVendaAsync();
            RelatorioVendasPdf.GerarPdf(vendas);

            return Ok("Relatorio gerado com sucesso");
        }

        [HttpGet("Sales-FastReport")]
        public async Task<IActionResult> GerarRelatorioVendasFastReport()
        {
            try
            {
                string reportPath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Reports", "Report01.frx");

                using (Report report = new Report())
                {

                    RegisteredObjects.AddConnection(typeof(MsSqlDataConnection));

                    report.Load(reportPath);

                    report.Prepare();

                    using (MemoryStream ms = new MemoryStream())
                    {
                        PDFSimpleExport pdfExport = new PDFSimpleExport();

                        pdfExport.ShowProgress = false;
                        pdfExport.OpenAfterExport = false;

                        report.Export(pdfExport, ms);
                        ms.Position = 0;

                        return File(ms.ToArray(), "application/pdf", "RelatorioVendas.pdf");
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao gerar relatório: {ex.Message}");
            }
        }

        [HttpGet("Sales-Excel")]
        public async Task<IActionResult> GerarRelatorioVendasExcel()
        {

            var vendas = await _relatorioService.ObterItensVendaAsync();
            ReportSaleExcel.Generate(vendas, System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Reports"));

            using (MemoryStream ms = new MemoryStream())
            {
                using (XLWorkbook workbook = new XLWorkbook(System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Reports", "RelatorioVendas.xlsx")))
                {
                    workbook.SaveAs(ms);
                    ms.Position = 0;
                    return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "RelatorioVendas.xlsx");
                }
            }
        }
    }
}
