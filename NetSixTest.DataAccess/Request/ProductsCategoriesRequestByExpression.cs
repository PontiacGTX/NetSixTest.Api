using MediatR;
using Microsoft.EntityFrameworkCore;
using NetSixTest.Data;
using NetSixTest.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NetSixTest.DataAccess.Request
{
    public class ProductsCategoriesRequestByExpression:IRequest<List<ProductsCategories>>
    {
        public Expression<Func<ProductsCategories, bool>> Expression { get; set; }

        public class ProductsCategoriesRequestByExpressionHandler(AppDbContext appDbContext) : IRequestHandler<ProductsCategoriesRequestByExpression, List<ProductsCategories>>
        {
            public async Task<List<ProductsCategories>> Handle(ProductsCategoriesRequestByExpression request, CancellationToken cancellationToken)
            {
                return appDbContext.ProductsCategories
                                    .Where(request.Expression)
                                    .Include(x=>x.Product)
                                    .Include(x=>x.Category)
                                    .ToList();
            }
        }

    }
}
