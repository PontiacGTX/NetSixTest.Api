using MediatR;
using NetSixTest.Data.Entity;
using NetSixTest.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NetSixTest.DataAccess.Request
{
    public class ProductListRequest : IRequest<IList<Product>>
    {
    
        public class ProductosListRequestHandler : IRequestHandler<ProductListRequest, IList<Product>>
        {
            protected AppDbContext _ctx { get; }
            public ProductosListRequestHandler(AppDbContext context)
            {
                _ctx = context;
            }

            public async Task<IList<Product>> Handle(ProductListRequest request, CancellationToken cancellationToken)
            {
                
                var productos = await _ctx.Products.Include(x=>x.Category).Select(x=>x).ToListAsync();
                return productos;
            }
        }


    }
}
