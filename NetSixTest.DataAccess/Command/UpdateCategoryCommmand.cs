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
    public class UpdateCategoryCommmand : IPropertyFieldHolder<Category>, IRequest<Category>
    {
        public Category Field { get; set; }

        class UpdateCategoryCommmandHandler : IRequestHandler<UpdateCategoryCommmand, Category>
        {
            private AppDbContext _ctx;

            public UpdateCategoryCommmandHandler(AppDbContext ctx)
            {
                _ctx = ctx;
            }
            public async Task<Category> Handle(UpdateCategoryCommmand command, CancellationToken cancellationToken)
            {
                var category = await _ctx.Categories.FirstOrDefaultAsync(x => x.Id == command.Field.Id);
                if (category is null) return null;
                category.Name = command.Field.Name;
                category.Enabled= command.Field.Enabled;
                await _ctx.SaveChangesAsync();
                return (await _ctx.Categories.FirstOrDefaultAsync(x => x.Id == category.Id))!;
            }
        }
    }
}
