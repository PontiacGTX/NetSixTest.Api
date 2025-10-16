using MediatR;
using Microsoft.EntityFrameworkCore;
using NetSixTest.Data;
using NetSixTest.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSixTest.DataAccess.Command
{
    public class CreateProductsCategoriesCommand:IRequest<ProductsCategories>
    {
        public ProductsCategories? Field { get; set; }
        public class CreateProductsCategoriesCommandHandler(AppDbContext appDbContext) : IRequestHandler<CreateProductsCategoriesCommand, ProductsCategories>
        {
            public async Task<ProductsCategories> Handle(CreateProductsCategoriesCommand request, CancellationToken cancellationToken)
            {
                if (request.Field == null)
                {
                    throw new ArgumentNullException(nameof(request.Field), "ProductsCategories Field cannot be null.");
                }
                request.Field.Id = Guid.NewGuid();
                var item = appDbContext.ProductsCategories.Add(request.Field);
                await appDbContext.SaveChangesAsync();
                appDbContext.ChangeTracker.Clear();

                return item.Entity;
            }
        }

    }
}
