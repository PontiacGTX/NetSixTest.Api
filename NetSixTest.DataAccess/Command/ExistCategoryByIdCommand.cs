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
    public class ExistCategoryByIdCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public class ExistCategoryByIdCommandHandler : IRequestHandler<ExistCategoryByIdCommand, bool>
        {
            private AppDbContext _ctx { get; }

            public ExistCategoryByIdCommandHandler(AppDbContext ctx)
            {
                _ctx = ctx;
            }
            public async Task<bool> Handle(ExistCategoryByIdCommand request, CancellationToken cancellationToken)
            {
                return await _ctx.Categories.AnyAsync(x => x.Id == request.Id);
            }
        }
    }
}
