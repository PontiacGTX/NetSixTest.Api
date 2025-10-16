using MediatR;
using NetSixTest.Data.Entity;
using NetSixTest.Data.Models;
using NetSixTest.DataAccess.Command;
using NetSixTest.DataAccess.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NetSixTest.Services.Services
{
    public class ProductCategoriesServices
    {
        public IMediator _mediator { get; set; }
        public ProductCategoriesServices(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<ProductsCategories> GetCategory(Guid id)
        {
            return await _mediator.Send(new GetProductCategoryByIdQuery() { Id = id });
        }
       
        public async Task<IList<ProductsCategories>> Get(Expression<Func<ProductsCategories, bool>> expression)
        {
            return await _mediator.Send(new ProductsCategoriesRequestByExpression() { Expression = expression });
        }
        public async Task<IList<ProductsCategories>> GetAll(Expression<Func<ProductsCategories, bool>> expression)
        {
            return await _mediator.Send(new ProductsCategoriesRequestByExpression() { Expression = expression });
        }

        public async Task<ProductsCategories> CreateCategory(ProductsCategories producto)
        {

            producto = await _mediator.Send(new CreateProductsCategoriesCommand() { Field = producto });
            return producto;
        }
        public async Task<bool> DeleteCategory(int productId,int categoryId)
        {
            return await _mediator.Send(new DeleteProductCategoryByIdCommand() { productId = productId, categoryId= categoryId });
        }
        public async Task<bool> DeleteCategory(Guid id)
        {
            return await _mediator.Send(new DeleteProductCategoryByIdCommand() { Id = id });
        }
        public async Task<bool> ExistCategory(int productId, int categoryId)
        {
            return await _mediator.Send(new ExistProductCategoryById() { ProductId = productId, CategoryId = categoryId });
        }

        public async Task AddProductsCategoriesRange(IEnumerable<ProductsCategories> productsCategories)
        {
            await _mediator.Send(new CreateProductsCategoriesRangeCommand { Fields = productsCategories });
        }

        public async Task DeleteProductsCategoriesRange(int productId, IEnumerable<int> categoryIds)
        {
            await _mediator.Send(new DeleteProductsCategoriesByProductAndCategoriesCommand { ProductId = productId, CategoryIds = categoryIds });
        }
    }
}
