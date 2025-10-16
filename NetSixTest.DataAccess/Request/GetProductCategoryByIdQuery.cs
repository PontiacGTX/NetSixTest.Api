using MediatR;
using NetSixTest.Data;
using NetSixTest.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSixTest.DataAccess.Request
{
    public class GetProductCategoryByIdQuery:IRequest<ProductsCategories>
    {
        public Guid Id { get; set; }

        public class GetProductCategoryByIdQueryHandler(AppDbContext dbContext) : IRequestHandler<GetProductCategoryByIdQuery, ProductsCategories>
        {
            public async Task<ProductsCategories> Handle(GetProductCategoryByIdQuery request, CancellationToken cancellationToken)
            {
                return dbContext.ProductsCategories.FirstOrDefault(x=>x.Id == request.Id);
            }
        }
    }
}
