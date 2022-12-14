using MediatR;
using Microsoft.EntityFrameworkCore;
using NetSixTest.Data;
using NetSixTest.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NetSixTest.DataAccess.Command
{
    public class ExistProductByExpressionCommand:IRequest<bool>
    {
        public Expression<Func<Product,bool>> Expression { get; set; }
        public class ExistProductByExpressionCommandHandler : IRequestHandler<ExistProductByExpressionCommand, bool>
        {
            private AppDbContext _ctx { get; }

            public ExistProductByExpressionCommandHandler(AppDbContext ctx)
            {
                _ctx = ctx;
            }
            public async Task<bool> Handle(ExistProductByExpressionCommand request, CancellationToken cancellationToken)
            {
                return await _ctx.Products.AnyAsync(request.Expression);
            }
        }
    }
}
