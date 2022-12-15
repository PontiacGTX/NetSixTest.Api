using MediatR;
using NetSixTest.Data.Entity;
using NetSixTest.Data.Interfaces;
using NetSixTest.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSixTest.DataAccess.Command
{
    public class CreateCategoryCommand : IPropertyFieldHolder<Category>, IRequest<Category>
    {
        public Category Field { get; set; }
        class CreateCategoryoCommandHandler : IRequestHandler<CreateCategoryCommand, Category>
        {
            private AppDbContext _ctx { get; }

            public CreateCategoryoCommandHandler(AppDbContext ctx)
            {
                _ctx = ctx;
            }
            public async Task<Category> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
            {
                Category inventarioItem = new();
                inventarioItem.Id = request.Field.Id;
                inventarioItem.Name = request.Field.Name;
                inventarioItem.Enabled = request.Field.Enabled;
                var entry = _ctx.Categories.Add(inventarioItem);
                await _ctx.SaveChangesAsync();
                return entry.Entity!;

            }
        }
    }
}
