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
                return (await _ctx.Products.Where(a => a.Id == query.Id).Include(x=>x.Category).FirstOrDefaultAsync())!;
            }
        }
    }
}
