using FluentValidation;
using NetSixTest.Data.Models;
using NetSixTest.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSixTest.Services.Validation
{
    public class CategoryValidator:AbstractValidator<CategoryModel>
    {
        
        public CategoryValidator()
        {

            RuleFor(x => x.Name).NotEmpty().NotNull();

        }
    }
}
