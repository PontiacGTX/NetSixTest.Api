using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NetSixTest.Data.Entity;
using NetSixTest.Data.Models;
using NetSixTest.Services.Services;
using NetSixTest.Shared.Helpers;
using NetSixTest.Shared.Responses;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace NetSixTest.Api.Controllers
{
    [Route("[controller]")]
    [Controller]
    public class ProductController : HttpControllerBase
    {
        private ProductServices _productoServices;
        private readonly ILogger<ProductController> _logger;

        public ProductController(ProductServices productoServices, ILogger<ProductController> logger)
        {
            _productoServices = productoServices;
            _logger = logger;
        }

        private void LogException( Exception ex, [CallerMemberName] string methodName="")
        { _logger.LogError("An error happened at " + methodName + " due to " + ex.Message + " exception details: " + 
            ex.InnerException + " Stack trace: " + ex.StackTrace); 
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                 
                var productos = await _productoServices.GetAll();
                return OkResponse(productos);

            }
            catch (Exception ex)
            {
                
                LogException(ex);
                return ErrorResponse();
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute]int id)
        {
            try
            {

                var product = await _productoServices.GetProduct(id);
                return OkResponse(product);

            }
            catch (Exception ex)
            {

                LogException(ex);
                return ErrorResponse();
            }
        }
        


        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ProductModel producto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestResponse(ModelState);
            }

            try
            {

                var res = await _productoServices.UpdateProduct(new(producto));
                return OkResponse(res);
            }
            catch (Exception ex)
            {
                LogException(ex);
                return ErrorResponse();
            }
        }

        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductModel producto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequestResponse(ModelState);
            }

            try
            {
               
                if (await _productoServices.ExistProduct(producto))
                {
                    var product =_productoServices.GetProduct(producto);
                    return OkResponse(producto);
                }
                 var result =  await _productoServices.CreateProduct(new(producto));
               

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
                var res = await _productoServices.DeleteProduct((int)id!);
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
