namespace escafandra.services.API.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Logica para registrar la solicitud
            await _next(context);
            // Logica para registrar la respuesta
        }
    }
}
