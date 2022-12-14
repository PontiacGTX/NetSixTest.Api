using MediatR;
using NetSixTest.Data.Entity;
using NetSixTest.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace NetSixTest.DataAccess.Request;

public class ProductRequestByExpression : IRequest<IList<Product>>
{
    public Expression<Func<Product,bool>> Expression { get; set; }
    public class ProductRequestByExpressionHandler : IRequestHandler<ProductRequestByExpression, IList<Product>>
    {
        protected AppDbContext _ctx { get; }
        public ProductRequestByExpressionHandler(AppDbContext context)
        {
            _ctx = context;
        }

        public async Task<IList<Product>> Handle(ProductRequestByExpression request, CancellationToken cancellationToken)
        {

            var productos = await _ctx.Products.Where(request.Expression).Select(x => x).ToListAsync();
            return productos;
        }
    }
}

