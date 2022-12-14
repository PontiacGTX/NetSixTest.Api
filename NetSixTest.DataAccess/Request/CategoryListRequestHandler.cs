using MediatR;
using NetSixTest.Data.Entity;
using NetSixTest.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NetSixTest.DataAccess.Request
{
    public class CategoryListRequest : IRequest<IList<Category>>
    {
        public class CategoryosListRequestHandler : IRequestHandler<CategoryListRequest, IList<Category>>
        {
            protected AppDbContext _ctx { get; }
            public CategoryosListRequestHandler(AppDbContext context)
            {
                _ctx = context;
            }

            public async Task<IList<Category>> Handle(CategoryListRequest request, CancellationToken cancellationToken)
            {

                var productos = await _ctx.Categories.Select(x => x).ToListAsync();
                return productos;
            }
        }
    }
}
