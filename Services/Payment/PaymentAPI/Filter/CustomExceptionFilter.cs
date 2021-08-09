using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Payment.Core.Application.Exceptions;
using PaymentAPI.Models;
using Serilog;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PaymentAPI.Filter
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is ValidationExceptions)
            {
                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                var resp = new BaseResponseModel();
                resp.Message = string.Join("/", ((ValidationExceptions)context.Exception).Failures.Values.Select(k => k[0]).ToArray());
                resp.Success = (int)HttpStatusCode.BadRequest;

                context.Result = new JsonResult(resp);
                Log.Error("Validation Exception {0}, {1}, context : {3}", resp.Message, context.ModelState, context);

                return;
            }

            if (context.Exception is NotValidDataException)
            {
                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                var resp = new BaseResponseModel();
                resp.Message = context.Exception.Message;
                resp.Success = (int)HttpStatusCode.BadRequest;

                context.Result = new JsonResult(resp);
                Log.Error("Validation Exception {0}, {1}, context : {3}", resp.Message, context.ModelState, context);

                return;
            }

            var code = HttpStatusCode.InternalServerError;

            if (context.Exception is NotFoundException)
            {
                code = HttpStatusCode.NotFound;
            }

            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = (int)code;
            context.Result = new JsonResult(new BaseResponseModel()
            {
                Success = (int)code,
                Message = context.Exception.Message,
                ExtraFields = context.Exception.StackTrace
            });

            Log.Error(context.Exception, "Custom Exception Filter {0} , {1}", context.Exception.Message, context.Exception.StackTrace);
        }
    }
}
