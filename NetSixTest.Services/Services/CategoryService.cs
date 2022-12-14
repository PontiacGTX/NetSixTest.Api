using MediatR;
using NetSixTest.Data.Entity;
using NetSixTest.Data.Models;
using NetSixTest.DataAccess.Command;
using NetSixTest.DataAccess.Command.NetSixTest.DataAccess.Command;
using NetSixTest.DataAccess.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NetSixTest.Services.Services
{
    public class CategoryService
    {
        public IMediator _mediator { get; set; }
        public CategoryService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Category> GetCategory(int id)
        {
            return await _mediator.Send(new GetCategoryByIdQuery() { Id = id });
        }
        public async Task<Category> GetCategory(CategoryModel Model)
        {
            return (await this.Get(x => x.Name.ToUpper().Trim() == Model.Name.Trim())).FirstOrDefault();
        }

        public async Task<IList<Category>> GetAll()
        {
            return await _mediator.Send(new CategoryListRequest());
        }

        public async Task<IList<Category>> GetAll(Expression<Func<Category, bool>> expression)
        {
            return await _mediator.Send(new CategoryRequestByExpression() { Expression = expression });
        }
        public async Task<Category> CreateCategory(Category producto)
        {

            producto = await _mediator.Send(new CreateCategoryCommand() { Field = producto });
            return producto;
        }
        public async Task<bool> DeleteCategory(int id)
        {
            return await _mediator.Send(new DeleteCategoryByIdCommand() { Id = id });
        }
        public async Task<bool> ExistCategory(int id)
        {
            return await _mediator.Send(new ExistCategoryByIdCommand() { Id = id });
        }  
        public async Task<bool> ExistCategory(CategoryModel model)
        {
            return (await ExistCategory(x=>x.Name.ToUpper().Trim() == model.Name.ToUpper().Trim()));
        }
        public async Task<bool> ExistCategory(Expression<Func<Category, bool>> expression)
        {
            return await _mediator.Send(new ExistCategoryByExpressionCommand() { Expression = expression });
        }
        public async Task<IList<Category>> Get(Expression<Func<Category, bool>> expression)
        {
            return await _mediator.Send(new CategoryRequestByExpression() { Expression = expression });
        }
        public async Task<Category> UpdateCategory(Category producto)
        {
            return await _mediator.Send(new UpdateCategoryCommmand() { Field = producto });
        }
    }
}
