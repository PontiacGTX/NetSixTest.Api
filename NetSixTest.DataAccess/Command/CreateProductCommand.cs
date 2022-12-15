using MediatR;
using NetSixTest.Data;
using NetSixTest.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSixTest.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace NetSixTest.DataAccess.Command;


public class CreateProductCommand : IPropertyFieldHolder<Product>, IRequest<Product>
{
    public Product Field { get; set; }
    class CreateProductoCommandHandler : IRequestHandler<CreateProductCommand, Product>
    {
        private AppDbContext _ctx { get; }

        public CreateProductoCommandHandler(AppDbContext ctx)
        {
            _ctx = ctx;
        }
        public async Task<Product> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            Product inventarioItem = new();
            inventarioItem.Id = request.Field.Id;
            inventarioItem.Name = request.Field.Name;
            inventarioItem.CategoryId = request.Field.CategoryId;
            inventarioItem.Price = request.Field.Price;
            inventarioItem.Quantity =request.Field.Quantity;
            var entry = _ctx.Products.Add(inventarioItem);
            await _ctx.SaveChangesAsync();
            return entry.Entity!;

        }
    }
}
