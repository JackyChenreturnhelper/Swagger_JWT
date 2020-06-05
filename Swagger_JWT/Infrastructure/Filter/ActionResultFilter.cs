using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Swagger_JWT.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Swagger_JWT.Infrastructure.Filter
{
    public class ActionResultFilter : IAsyncActionFilter
    {
        /// <summary>
        /// Called asynchronously before the action, after model binding is complete.
        /// </summary>
        /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext"/>.</param>
        /// <param name="next">
        /// The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.ActionExecutionDelegate"/>. Invoked to
        /// execute the next action filter or the action itself.
        /// </param>
        /// <returns>
        /// A <see cref="T:System.Threading.Tasks.Task"/> that on completion indicates the filter
        /// has executed.
        /// </returns>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var executedContext = await next();

            if (executedContext.Result is ObjectResult result)
            {
                switch (result.Value)
                {
                    case HttpResponseMessage _:
                        return;

                    case SuccessResultViewModel<object> _:
                        return;

                    case FailViewModel<object> _:
                        return;
                }

                var httpMethod = context.HttpContext.Request.Method;

                var responseModel = new SuccessResultViewModel<object>
                {
                    ApiVersion = "1.0.0",
                    Method = $"{context.HttpContext.Request.Path}.{httpMethod}",
                    Status = "Success",
                    Id = Guid.NewGuid(),
                    Data = result.Value
                };

                executedContext.Result = new ObjectResult(responseModel)
                {
                    StatusCode = result.StatusCode
                };
            }
        }
    }
}
