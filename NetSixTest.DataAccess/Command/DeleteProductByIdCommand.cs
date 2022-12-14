using MediatR;
using Microsoft.EntityFrameworkCore;
using NetSixTest.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSixTest.DataAccess.Command
{
   
    public class DeleteProductByIdCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public class DeleteProductByIdCommandHandler : IRequestHandler<DeleteProductByIdCommand, bool>
        {
            private AppDbContext _ctx;

            public DeleteProductByIdCommandHandler(AppDbContext ctx)
            {
                _ctx = ctx;
            }
            public async Task<bool> Handle(DeleteProductByIdCommand request, CancellationToken cancellationToken)
            {
                var product = await _ctx.Products.FirstOrDefaultAsync(x => x.Id == request.Id);
                if (product is null) return true;
                _ctx.Products.Remove(product);
                await _ctx.SaveChangesAsync();
                return !(await _ctx.Products.AnyAsync(x => x.Id == request.Id));
            }
        }
    }
}
