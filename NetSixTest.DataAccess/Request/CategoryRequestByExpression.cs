using MediatR;
using NetSixTest.Data.Entity;
using NetSixTest.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NetSixTest.DataAccess.Request;

public class CategoryRequestByExpression : IRequest<IList<Category>>
{
    public Expression<Func<Category, bool>> Expression { get; set; }
    public class CategoryRequestByExpressionHandler : IRequestHandler<CategoryRequestByExpression, IList<Category>>
    {
        protected AppDbContext _ctx { get; }
        public CategoryRequestByExpressionHandler(AppDbContext context)
        {
            _ctx = context;
        }

        public async Task<IList<Category>> Handle(CategoryRequestByExpression request, CancellationToken cancellationToken)
        {

            var categories = await _ctx.Categories.Where(request.Expression).Select(x => x).ToListAsync();
            return categories;
        }
    }
}

