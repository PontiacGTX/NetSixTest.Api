using global::NetSixTest.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace NetSixTest.DataAccess.Command
{
    namespace NetSixTest.DataAccess.Command
    {
        public class DeleteCategoryByIdCommand : IRequest<bool>
        {
            public int Id { get; set; }
            public class DeleteCategoryByIdCommandHandler : IRequestHandler<DeleteCategoryByIdCommand, bool>
            {
                private AppDbContext _ctx;

                public DeleteCategoryByIdCommandHandler(AppDbContext ctx)
                {
                    _ctx = ctx;
                }
                public async Task<bool> Handle(DeleteCategoryByIdCommand request, CancellationToken cancellationToken)
                {
                    var category = await _ctx.Categories.FirstOrDefaultAsync(x => x.Id == request.Id );
                    if (category is null) return true;
                    category.Enabled = false;
                    await _ctx.SaveChangesAsync();
                    return !(await _ctx.Categories.AnyAsync(x => x.Id == request.Id));
                }
            }
        }
    }

}
