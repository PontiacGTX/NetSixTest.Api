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
    public class GetCategoryByIdQuery : IRequest<Category>
    {
        public int Id { get; set; }
        public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, Category>
        {
            protected AppDbContext _ctx { get; }
            public GetCategoryByIdQueryHandler(AppDbContext context)
            {
                _ctx = context;
            }
            public async Task<Category> Handle(GetCategoryByIdQuery query, CancellationToken cancellationToken)
            {
                return (await _ctx.Categories.FirstOrDefaultAsync(a => a.Id == query.Id))!;
            }
        }
    }
}
