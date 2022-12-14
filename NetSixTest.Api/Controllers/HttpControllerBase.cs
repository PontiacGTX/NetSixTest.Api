using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NetSixTest.Shared.Helpers;
using NetSixTest.Shared.Responses;

namespace NetSixTest.Api.Controllers
{
    public class HttpControllerBase:ControllerBase
    {
        protected ObjectResult OkResponse(object o)
          => Ok(new Response(o));
        protected ObjectResult ErrorResponse()
        => StatusCode(StatusCodes.Status500InternalServerError, Factory.GetResponse<ServerErrorResponse>(null,
                    500,
                    "Various internal unexpected errors happened",
                    false));
        protected ObjectResult BadRequestResponse(ModelStateDictionary ModelState)
            => BadRequest(Factory.GetResponse<ServerErrorResponse>(null, 400, "Bad Request", false,
                ModelState.SelectMany(x => x.Value.Errors.Select(e => e.ErrorMessage))));
    }
}
