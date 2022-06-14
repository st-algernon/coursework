namespace Coursework.Web.Configs;

public static class NSwagConfiguration
{
    public static readonly bool IsOpenApiSchemeGenerationRuntime = Environment.StackTrace.Contains("HostFactoryResolver");
        
    public static IServiceCollection AddNSwag(this IServiceCollection source)
    {
        if (IsOpenApiSchemeGenerationRuntime)
        {
            source.AddSwaggerDocument();
        }

        return source;
    }

    public static IApplicationBuilder UseNSwag(this IApplicationBuilder source) => source.UseOpenApi();
}