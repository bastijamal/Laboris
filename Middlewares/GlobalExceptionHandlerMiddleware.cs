using System;
namespace Laboris.Middlewares
{
	public class GlobalExceptionHandlerMiddleware
	{
        private readonly RequestDelegate _next;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next)
		{
			_next = next;

        }


		//public async Task InvokeAsync(HttpContext context)
		//{
		//	try
		//	{
		//		await _next.Invoke(context);
		//	}

		//	catch (Exception e)
		//	{
		//		context.Response.Redirect($"/Home/Error?error={e.Message}");
		//	}
		//}


	}
}

