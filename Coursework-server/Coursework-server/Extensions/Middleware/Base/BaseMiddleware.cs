namespace Coursework_server.Extensions.Middleware.Base;

public abstract class BaseMiddleware
{
    public readonly RequestDelegate Next;

    protected BaseMiddleware(RequestDelegate next)
    {
        Next = next;
    }

    public abstract Task Invoke(HttpContext context);
}