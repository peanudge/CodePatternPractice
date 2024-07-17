using WebApi.Services;

namespace WebApi;

public static class GraphServiceExtension
{
    public static void AddGraphService(this IServiceCollection services)
    {
        services.AddSingleton<NodeGraphProcessingService>();
    }
}
