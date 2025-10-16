using MediatR;
using NetSixTest.Data;
using NetSixTest.Data.Entity;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NetSixTest.DataAccess.Command
{
    public class CreateProductsCategoriesRangeCommand : IRequest<Unit>
    {
        public IEnumerable<ProductsCategories> Fields { get; set; }

        public class CreateProductsCategoriesRangeCommandHandler(AppDbContext appDbContext) : IRequestHandler<CreateProductsCategoriesRangeCommand, Unit>
        {
            public async Task<Unit> Handle(CreateProductsCategoriesRangeCommand request, CancellationToken cancellationToken)
            {
                if (request.Fields == null)
                {
                    throw new System.ArgumentNullException(nameof(request.Fields), "ProductsCategories Fields cannot be null.");
                }

                foreach (var item in request.Fields)
                {
                    item.Id = System.Guid.NewGuid();
                    // Adjuntar la categoría si no está ya adjunta y tiene un ID.
                    // Esto es importante si las categorías vienen con solo el ID y no el objeto completo.
                    if (item.CategoryId != 0 && item.Category == null)
                    {
                        var existingCategory = await appDbContext.Categories.FindAsync(new object[] { item.CategoryId }, cancellationToken);
                        if (existingCategory != null)
                        {
                            item.Category = existingCategory;
                            appDbContext.Entry(existingCategory).State = Microsoft.EntityFrameworkCore.EntityState.Unchanged;
                        }
                    }
                    appDbContext.ProductsCategories.Add(item);
                }

                await appDbContext.SaveChangesAsync(cancellationToken);
                return Unit.Value;
            }
        }
    }
}
