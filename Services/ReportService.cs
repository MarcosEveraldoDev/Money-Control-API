using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using WebApplication2.Data;
using WebApplication2.Models.DTOs;

namespace WebApplication2.Services
{
    public class ReportService
    {
        private readonly AppDbContext _context;

        public ReportService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<SaleItemDTO>> ObterItensVendaAsync()
        {
            return await _context.SaleItem
            .Include(s => s.Product)
                .Select(s => new SaleItemDTO
                {
                    Id = s.Id,
                    SaleId = s.SaleId,
                    ProductName = s.Product.Name,
                    Quantity = s.Quantity,
                    UnitPrice = s.UnitPrice,
                    TotalPrice = s.TotalPrice
                })
                .AsNoTracking()
                .ToListAsync();
        }
    }

    public class RelatorioVendasPdf
    {
        public static void GerarPdf(List<SaleItemDTO> vendas)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            Document.Create(document =>
           {

               document.Page(page =>
               {
                   page.Size(PageSizes.A4);
                   page.Margin(30);
                   page.Header().Row(row =>
                   {
                       row.RelativeItem()
                       .PaddingTop(8)
                          .Text("Relatório de Vendas")
                          .Bold()
                          .FontSize(25)
                          .AlignLeft();

                       row.ConstantItem(130)
                          .AlignRight()
                          .Image(@"C:\Users\Desktop\Desktop\image.png");

                   });


                   page.Content().Column(coluna =>
                   {
                       coluna.Item()
                       .PaddingTop(20);


                       coluna.Item()

                       .Table(table =>
                       {
                           table.ColumnsDefinition(columns =>
                           {
                               columns.ConstantColumn(50);
                               columns.ConstantColumn(220);
                               columns.RelativeColumn();
                               columns.RelativeColumn();
                               columns.RelativeColumn();

                           });

                           table.Header(header =>
                           {
                               header.Cell().Element(CellStyle).Text("Id").Bold().FontSize(15).LineHeight(2).Justify();
                               header.Cell().Element(CellStyle).Text("Produto").Bold().FontSize(15).LineHeight(2).Justify();
                               header.Cell().Element(CellStyle).Text("Preço Unitário").Bold().FontSize(15).LineHeight(2).Justify();
                               header.Cell().Element(CellStyle).Text("Quantidade").Bold().FontSize(15).LineHeight(2).Justify();
                               header.Cell().Element(CellStyle).Text("Total").Bold().FontSize(15).LineHeight(2).Justify();

                               static IContainer CellStyle(IContainer container)
                               {
                                   return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(4).BorderBottom(1).BorderColor(Colors.Black);

                               }

                           });

                           foreach (var venda in vendas)
                           {
                               table.Cell().Element(CellStyle).Text(venda.Id.ToString()).LineHeight(2);
                               table.Cell().Element(CellStyle).Text(venda.ProductName).LineHeight(2);
                               table.Cell().Element(CellStyle).Text($"R$ {venda.UnitPrice:F2}").LineHeight(2);
                               table.Cell().Element(CellStyle).Text(venda.Quantity.ToString()).LineHeight(2);
                               table.Cell().Element(CellStyle).Text($"R$ {venda.TotalPrice:F2}").LineHeight(2);

                               static IContainer CellStyle(IContainer container)
                               {
                                   return container
                                   .Border(1)
                                   .BorderColor(Colors.Grey.Lighten1)
                                   .PaddingHorizontal(5)
                                   .PaddingVertical(2)
                                   .AlignMiddle();
                               }
                           }
                       });
                   });

                   page.Footer()
                       .AlignRight()
                       .Text($"Gerado em {System.DateTime.Now:dd/MM/yyyy HH:mm}");
               });
           }).GeneratePdfAndShow();
        }
    }

    public class ReportSaleExcel
    {
        public static void Generate(List<SaleItemDTO> sale, string pastWay)
        {

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Relatório de Vendas");


            worksheet.Cell(1, 1).Value = "Id";
            worksheet.Cell(1, 2).Value = "Produto";
            worksheet.Cell(1, 3).Value = "Quantidade";
            worksheet.Cell(1, 4).Value = "Preço Unitário";
            worksheet.Cell(1, 5).Value = "Preço Total";



            var headerStyle = worksheet.Range(1, 1, 1, 5);
            headerStyle.Style.Font.Bold = true;
            headerStyle.Style.Fill.BackgroundColor = XLColor.FromHtml("#4F81BD");
            headerStyle.Style.Font.FontColor = XLColor.FromHtml("#FFFFFF");
            headerStyle.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;


            for (int i = 0; i < sale.Count; i++)
            {
                worksheet.Cell(i + 2, 1).Value = sale[i].Id;
                worksheet.Cell(i + 2, 2).Value = sale[i].ProductName;
                worksheet.Cell(i + 2, 3).Value = sale[i].Quantity;
                worksheet.Cell(i + 2, 4).Value = sale[i].UnitPrice;
                worksheet.Cell(i + 2, 5).Value = sale[i].TotalPrice;
            }

            var valueRange = worksheet.Range(2, 4, sale.Count + 1, 4);
            valueRange.Style.NumberFormat.Format = "R$ #,##0.00";

            var valueRangeTotal = worksheet.Range(2, 5, sale.Count + 1, 5);
            valueRangeTotal.Style.NumberFormat.Format = "R$ #,##0.00";


            worksheet.Columns().AdjustToContents();
            worksheet.Column(1).Width = 7;
            worksheet.Column(2).Width = 40;
            worksheet.Column(3).Width = 12;
            worksheet.Column(4).Width = 15;
            worksheet.Column(5).Width = 15;


            string filePath = System.IO.Path.Combine(pastWay, "RelatorioVendas.xlsx");

            workbook.SaveAs(filePath);
        }

    }
}
