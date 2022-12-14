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
    public class ExistProductByIdCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public class ExistProductByIdCommandHandler : IRequestHandler<ExistProductByIdCommand, bool>
        {
            private AppDbContext _ctx { get; }

            public ExistProductByIdCommandHandler(AppDbContext ctx)
            {
                _ctx = ctx;
            }
            public async Task<bool> Handle(ExistProductByIdCommand request, CancellationToken cancellationToken)
            {
                return await _ctx.Products.AnyAsync(x => x.Id == request.Id);
            }
        }
    }
}
