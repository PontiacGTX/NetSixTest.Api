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
    
    public class ProductServices
    {
        public IMediator _mediator { get; set; }
        public ProductServices(IMediator mediator)
        {
            _mediator = mediator;
        }
   
        public async Task<Product> GetProduct(int id)
        {
            return await _mediator.Send(new GetProductByIdQuery() { Id = id });
        }
        public async Task<IList<Product>> GetAll()
        {
            return await _mediator.Send(new ProductListRequest());
        }

        public async Task<IList<Product>> GetAll(Expression<Func<Product,bool>> expression)
        {
            return await _mediator.Send(new ProductRequestByExpression() {  Expression = expression});
        }
        public async Task<Product> CreateProduct(Product producto)
        {

            producto = await _mediator.Send(new CreateProductCommand() { Field = producto });
            return producto;
        }
        public async Task<bool> DeleteProduct(int id)
        {
            return await _mediator.Send(new DeleteProductByIdCommand() { Id = id });
        }
        public async Task<bool> ExistProduct(int id)
        {
            return await _mediator.Send(new ExistProductByIdCommand() { Id = id });
        }
        public async Task<bool> ExistProduct(Expression<Func<Product,bool>> expression)
        {
            return await _mediator.Send(new ExistProductByExpressionCommand() { Expression = expression });
        }
        public async Task<bool> ExistProduct(ProductModel product)
        {
            return await this.ExistProduct(x =>
                 x.Name.ToLower() == product.Name.ToLower() && x.CategoryId == product.CategoryId);
        }
        public async Task<bool> ExistProduct(ProductInsertModel product)
        {
            return await this.ExistProduct(x =>
                 x.Name.ToLower() == product.Name.ToLower() && x.CategoryId == product.CategoryId);
        }
        public async Task<Product> GetProduct(ProductModel product)
        {
            return (await this.GetAll(x => x.Name.ToLower() == product.Name.ToLower() && x.CategoryId == product.CategoryId)).FirstOrDefault()!;
        }
        public async Task<Product> GetProduct(ProductInsertModel product)
        {
            return (await this.GetAll(x => x.Name.ToLower() == product.Name.ToLower() && x.CategoryId == product.CategoryId)).FirstOrDefault()!;
        }
        public async Task<Product> UpdateProduct(Product producto)
        {
            return await _mediator.Send(new UpdateProductCommmand() { Field = producto });
        }
    }
}
