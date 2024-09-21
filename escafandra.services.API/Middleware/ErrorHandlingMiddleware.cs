using Newtonsoft.Json;
using System.Net;

namespace escafandra.services.API.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var code = HttpStatusCode.InternalServerError;
            var error = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            var result = JsonConvert.SerializeObject(new { error = error.ToString() });

            context.Response.ContentType = "application/json";
            //context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(result); 
        }   
    }
}
