using FluentValidation;
using NetSixTest.Data.Entity;
using NetSixTest.Data.Models;
using NetSixTest.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSixTest.Services.Validation
{
    public class ProductValidator : AbstractValidator<ProductModel>
    {
        CategoryService _categoryService { get; }
        public ProductValidator(CategoryService categoryService)
        {
            _categoryService = categoryService;
            // Check name is not null, empty and is between 1 and 250 characters
            RuleFor(product => product.Name).NotNull().NotEmpty().Length(1, 250).InjectValidator();

            
            RuleFor(product => product.Price).GreaterThan(-1).WithMessage("Price should be higher than or equal to 0");

            
            RuleFor(product => product.Quantity).InclusiveBetween(0, int.MaxValue).WithErrorCode("Quantity must be higher or equal to 0");

            
            RuleFor(product => product.CategoryId).Must(ExistCategory).WithMessage("Category Id does not exist");
        }

        bool ExistCategory(int categoryId) =>
            _categoryService.ExistCategory(x => x.Id == categoryId).Result;
    }
}
