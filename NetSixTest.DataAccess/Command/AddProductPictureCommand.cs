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
    public class AddProductPictureCommand:IRequest<ProductPicture?>
    {
        public ProductPicture Field { get; set; }

        public class AddProductPictureCommandHandler(AppDbContext dbContext) : IRequestHandler<AddProductPictureCommand, ProductPicture?>
        {
            public async Task<ProductPicture?> Handle(AddProductPictureCommand request, CancellationToken cancellationToken)
            {
                if (dbContext.Pictures.Any(x => x.Hash == request.Field.Hash && x.ProductId == request.Field.ProductId))
                    return dbContext.Pictures.FirstOrDefault(x => x.Hash == request.Field.Hash);

                request.Field.ProductPictureId = Guid.NewGuid();
                var result = dbContext.Pictures.Add(request.Field);
                await dbContext.SaveChangesAsync();
                return result.Entity;
            }
        }

    }
}
