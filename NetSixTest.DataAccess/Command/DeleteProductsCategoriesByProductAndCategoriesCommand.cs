using MediatR;
using NetSixTest.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NetSixTest.DataAccess.Command
{
    public class DeleteProductsCategoriesByProductAndCategoriesCommand : IRequest<Unit>
    {
        public int ProductId { get; set; }
        public IEnumerable<int> CategoryIds { get; set; }

        public class DeleteProductsCategoriesByProductAndCategoriesCommandHandler(AppDbContext appDbContext) : IRequestHandler<DeleteProductsCategoriesByProductAndCategoriesCommand, Unit>
        {
            public async Task<Unit> Handle(DeleteProductsCategoriesByProductAndCategoriesCommand request, CancellationToken cancellationToken)
            {
                if (request.CategoryIds == null)
                {
                    throw new System.ArgumentNullException(nameof(request.CategoryIds), "CategoryIds cannot be null.");
                }

                await appDbContext.ProductsCategories
                    .Where(pc => pc.ProductId == request.ProductId && request.CategoryIds.Contains(pc.CategoryId))
                    .ExecuteDeleteAsync(cancellationToken);

                return Unit.Value;
            }
        }
    }
}
