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

namespace NetSixTest.DataAccess.Command
{
    public class ExistCategoryByExpressionCommand: IRequest<bool>
    {
        public Expression<Func<Category, bool>> Expression { get; set; }
        public class ExistCategoryByExpressionCommandHandler : IRequestHandler<ExistCategoryByExpressionCommand, bool>
        {
            private AppDbContext _ctx { get; }

            public ExistCategoryByExpressionCommandHandler(AppDbContext ctx)
            {
                _ctx = ctx;
            }
            public async Task<bool> Handle(ExistCategoryByExpressionCommand request, CancellationToken cancellationToken)
            {
                return await _ctx.Categories.AnyAsync(request.Expression);
            }
        }
    }
}
