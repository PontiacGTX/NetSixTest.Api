using MediatR;
using NetSixTest.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSixTest.DataAccess.Command
{
    public class DeleteProductCategoryByIdCommand:IRequest<bool>
    {
        public int? categoryId { get; set; }
        public Guid Id { get; set; }
        public int? productId { get; set; }

        public class DeleteProductCategoryByIdCommandHandler(AppDbContext appDbContext) : IRequestHandler<DeleteProductCategoryByIdCommand, bool>
        {
            public async Task<bool> Handle(DeleteProductCategoryByIdCommand request, CancellationToken cancellationToken)
            {
                var productCategory =
                    request.categoryId == null && request.productId == null? 
                    appDbContext.ProductsCategories.FirstOrDefault(x=>x.Id == request.Id)
                    : appDbContext.ProductsCategories.FirstOrDefault(x => x.ProductId == request.productId && x.CategoryId == request.categoryId);
                if (productCategory == null)
                {
                    return true;
                }
                appDbContext.ProductsCategories.Remove(productCategory);
                await appDbContext.SaveChangesAsync();
                return true;

            }
        }
    }
}
