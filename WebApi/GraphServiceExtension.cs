using WebApi.Services;

namespace WebApi;

public static class GraphServiceExtension
{
    public static IServiceCollection AddGraphService(this IServiceCollection services)
    {
        return services.AddSingleton<NodeGraphProcessingService>();
    }
}
