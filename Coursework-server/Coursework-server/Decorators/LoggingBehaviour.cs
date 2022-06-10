using System.Diagnostics;
using System.Globalization;
using MediatR;

namespace Coursework_server.Logs;

public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehaviour<TRequest, TResponse>> _logger;
    
    public LoggingBehaviour(ILogger<LoggingBehaviour<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }
    
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        var requestName = typeof(TRequest).Name;
        var unqiueId = Guid.NewGuid().ToString();
        var utcDate = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
        _logger.LogInformation($"Begin Request Id:{unqiueId}, request name:{requestName}, utc date: {utcDate}");
        var timer = new Stopwatch();
        timer.Start();
        var response = await next();
        timer.Stop();
        _logger.LogInformation($"End Request Id:{unqiueId}, request name:{requestName}, total request time:{timer.ElapsedMilliseconds}");
        
        return response;
    }
}