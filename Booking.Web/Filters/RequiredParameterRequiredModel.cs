using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Server.IIS;

namespace Booking.Web.Filters
{
    public class RequiredParameterRequiredModel : ActionFilterAttribute
    {
        private string parameterName;
        public RequiredParameterRequiredModel(string parameterName)
        {
            if (string.IsNullOrWhiteSpace(parameterName))
            { 
                throw new ArgumentException($"'{nameof(parameterName)}' cannot be null or whitespace.", nameof(parameterName));
            }

            this.parameterName = parameterName;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.RouteData.Values[parameterName] == null)
            {
                context.Result = new NotFoundResult();
            }
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (context.Result is ViewResult viewResult)
            {
                if (viewResult.Model is null)
                {
                    context.Result = new NotFoundResult();
                }
            }
        }
    }
}
