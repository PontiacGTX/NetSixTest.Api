using MediatR;
using NetSixTest.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSixTest.DataAccess.Request
{
    public class ExistProductCategoryById:IRequest<bool>
    {
        public int CategoryId { get; set; }
        public int ProductId { get; set; }
        public class ExistProductCategoryByIdHandler(AppDbContext appDbContext) : IRequestHandler<ExistProductCategoryById, bool>
        {
            public async Task<bool> Handle(ExistProductCategoryById request, CancellationToken cancellationToken)
            {
                return appDbContext.ProductsCategories.Any(x=>x.ProductId == request.ProductId && x.CategoryId == request.CategoryId);
            }
        }
    }
}
