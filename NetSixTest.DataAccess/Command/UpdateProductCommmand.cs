using MediatR;
using NetSixTest.Data.Entity;
using NetSixTest.Data.Interfaces;
using NetSixTest.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NetSixTest.DataAccess.Command
{
    
    public class UpdateProductCommmand : IPropertyFieldHolder<Product>, IRequest<Product>
    {
        public Product Field { get; set; }

        class UpdateProductCommmandHandler : IRequestHandler<UpdateProductCommmand, Product>
        {
            private AppDbContext _ctx;

            public UpdateProductCommmandHandler(AppDbContext ctx)
            {
                _ctx = ctx;
            }
            public async Task<Product> Handle(UpdateProductCommmand command, CancellationToken cancellationToken)
            {
                var producto = await _ctx.Products.FirstOrDefaultAsync(x => x.Id == command.Field.Id);
                if (producto is null) return null;
                producto.Price = command.Field.Price;
                producto.Name = command.Field.Name;
                producto.Quantity = command.Field.Quantity;
                producto.CategoryId = command.Field.CategoryId;
                await _ctx.SaveChangesAsync();
                return (await _ctx.Products.FirstOrDefaultAsync(x => x.Id == producto.Id))!;
            }
        }
    }
}
