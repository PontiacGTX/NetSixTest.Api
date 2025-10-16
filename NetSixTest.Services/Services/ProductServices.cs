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
        public ProductCategoriesServices _productCategoriesServices { get; set; }

        private CategoryService _categoryService;
        private ProductPictureServices _productPictureServices;


        public ProductServices(IMediator mediator, ProductCategoriesServices productCategoriesServices,CategoryService categoryService,
            ProductPictureServices productPictureServices)
        {
            _mediator = mediator;
            _productCategoriesServices = productCategoriesServices;
            _categoryService = categoryService;
            _productPictureServices= productPictureServices;
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
        public async Task<Product> CreateProduct(ProductInsertModel model)
        {
            var producto = new Product(model);
            var result = await _mediator.Send(new CreateProductCommand() { Field = producto });
            if (model.ProductsCategories != null)
            {
                model.ProductsCategories = model.ProductsCategories.Where(x => !_productCategoriesServices.ExistCategory(x.ProductId, x.CategoryId).Result).ToList();
                foreach (var category in model.ProductsCategories)
                {
                    try
                    {
                        if (!await _categoryService.ExistCategory(category.CategoryId))
                            continue;

                        await _productCategoriesServices.CreateCategory(new ProductsCategories
                        {
                            CategoryId = category.CategoryId,
                            ProductId = result.Id
                        });
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

            if (result != null && model.ProductPictures != null)
            {
                model.ProductPictures = model.ProductPictures.Select(x =>
                {
                    return new InsertProductPictureModel
                    {
                        PictureData = x.PictureData,
                        ProductId = result.Id,
                        FileName = x.FileName
                    };
                }).ToList();
                result.Pictures = await _productPictureServices.AddProductPicture(model.ProductPictures);
            }

            return result;
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
                 x.Name.ToLower() == product.Name.ToLower());
        }
        public async Task<bool> ExistProduct(ProductInsertModel product)
        {
            return await this.ExistProduct(x =>
                 x.Name.ToLower() == product.Name.ToLower());
        }
        public async Task<Product> GetProduct(ProductModel product)
        {
            return (await this.GetAll(x => x.Name.ToLower() == product.Name.ToLower())).FirstOrDefault()!;
        }
        public async Task<Product> GetProduct(ProductInsertModel product)
        {
            return (await this.GetAll(x => x.Name.ToLower() == product.Name.ToLower())).FirstOrDefault()!;
        }
        public async Task<Product> UpdateProduct(Product producto)
        {
            var currentCats = await _productCategoriesServices.Get(x => x.ProductId == producto.Id);

            var toDelete = currentCats
                .Where(x => producto.ProductsCategories == null || !producto.ProductsCategories.Any(y => y.CategoryId == x.CategoryId))
                .Select(x => x.CategoryId)
                .ToList();

            var toAdd = producto.ProductsCategories
                .Where(x => currentCats == null || !currentCats.Any(y => y.CategoryId == x.CategoryId))
                .Select(x => new ProductsCategories { ProductId = producto.Id, CategoryId = x.CategoryId })
                .ToList();

            if (toDelete.Any())
            {
                await _productCategoriesServices.DeleteProductsCategoriesRange(producto.Id, toDelete);
            }

            if (toAdd.Any())
            {
                await _productCategoriesServices.AddProductsCategoriesRange(toAdd);
            }
            return await _mediator.Send(new UpdateProductCommmand() { Field = producto });
        }
    }
}
