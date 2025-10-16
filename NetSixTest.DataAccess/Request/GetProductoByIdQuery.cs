using MediatR;
using NetSixTest.Data.Entity;
using NetSixTest.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSixTest.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace NetSixTest.DataAccess.Request
{
    public class GetProductByIdQuery : IRequest<Product>
    {
        public int Id { get; set; }
        public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Product>
        {
            protected AppDbContext _ctx { get; }
            public GetProductByIdQueryHandler(AppDbContext context)
            {
                _ctx = context;
            }
            public async Task<Product> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
            {
                return (await _ctx.Products.Where(a => a.Id == query.Id).Include(x=>x.ProductsCategories).Include(x=>x.Pictures)
                    .Select(p=> new Product
                    {
                        Enabled = p.Enabled,
                        Id =p.Id,
                        Name =p.Name,
                        Pictures =p.Pictures,
                        Price =p.Price,
                        ProductsCategories = p.ProductsCategories.Select(pc=> new ProductsCategories{ Id = pc.Id,CategoryId = pc.CategoryId,ProductId =pc.ProductId, Category = new Category { Id = pc.Category.Id, Name = pc.Category.Name, Enabled = pc.Category.Enabled } }).ToList(),
                        Quantity =p.Quantity
                        
                    }).FirstOrDefaultAsync())!;
            }
        }
    }
}
