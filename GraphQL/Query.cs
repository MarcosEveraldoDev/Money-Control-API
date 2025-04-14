
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.GraphQL;

public class Query
{

    //[UseDbContext(typeof(AppDbContext))]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Products> GetProducts([ScopedService] AppDbContext context) =>
   context.Products;

    //[UseDbContext(typeof(AppDbContext))]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Sale> GetSales([ScopedService] AppDbContext context) =>
        context.Sale;

    //[UseDbContext(typeof(AppDbContext))]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<SaleItem> GetSaleItems([ScopedService] AppDbContext context) =>
        context.SaleItem;

    //[UseDbContext(typeof(AppDbContext))]
    //[UseProjection]
    //[UseFiltering]
    //[UseSorting]
    //public IQueryable<User> GetUsers([ScopedService] AppDbContext context) =>
    //    context.user;
}
