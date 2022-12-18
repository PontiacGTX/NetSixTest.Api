using Microsoft.AspNetCore.Mvc;
using NetSixTest.Data.Models;
using NetSixTest.Services.Services;
using NetSixTest.Shared.Helpers;
using NetSixTest.Shared.Responses;
using System.Runtime.CompilerServices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NetSixTest.Api.Controllers
{
    [Route("[controller]")]
    [Controller]
    public class CategoryController : HttpControllerBase
    {
        // GET: api/<CategoryController>
        private CategoryService _categoryServices;
        private ProductServices _productServices;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(CategoryService categoryServices,ProductServices productServices, ILogger<CategoryController> logger)
        {
            _categoryServices = categoryServices;
            _productServices = productServices;
            _logger = logger;
        }

        private void LogException(Exception ex, [CallerMemberName] string methodName = "")
        {
            _logger.LogError("An error happened at " + methodName + " due to " + ex.Message + " exception details: " +
            ex.InnerException + " Stack trace " + ex.StackTrace);
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {

                var categorys = await _categoryServices.GetAll();
                return OkResponse(categorys);

            }
            catch (Exception ex)
            {

                LogException(ex);
                return ErrorResponse();
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            try
            {

                var product = await _categoryServices.GetCategory(id);
                return OkResponse(product);

            }
            catch (Exception ex)
            {

                LogException(ex);
                return ErrorResponse();
            }
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute]int id,[FromBody] CategoryModel category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestResponse(ModelState);
            }

            try
            {

                var res = await _categoryServices.UpdateCategory(new(category));
                return OkResponse(res);
            }
            catch (Exception ex)
            {
                LogException(ex);
                return ErrorResponse();
            }
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CategoryModel category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestResponse(ModelState);
            }

            try
            {

                if (await _categoryServices.ExistCategory(category))
                {
                    var product = _categoryServices.GetCategory(category);
                    return OkResponse(category);
                }
                var result = await _categoryServices.CreateCategory(new(category));


                return OkResponse(result);
            }
            catch (Exception ex)
            {
                LogException(ex);
                return ErrorResponse();
            }

        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestResponse(ModelState);
            }

            try
            {
                var res = await _categoryServices.DeleteCategory((int)id!);


                try
                {
                    foreach (var product in await _productServices.GetAll(x => x.CategoryId == id))
                    {
                        try
                        {
                            product.Enabled = false;
                            await _productServices.UpdateProduct(product);
                        }
                        catch (Exception ex)
                        {
                            LogException(ex);
                        }
                    }
                }
                catch (Exception ex)
                { 
                    LogException(ex);
                }
                
                return OkResponse(res);
            }
            catch (Exception ex)
            {
                LogException(ex);
                return ErrorResponse();
            }
        }

    }
}
