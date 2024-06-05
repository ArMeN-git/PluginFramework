using Microsoft.Extensions.DependencyInjection;
using PluginFramework.Core.Images;

namespace PluginFramework.Core.Extensions;

public static class CoreServiceCollectionExtensions
{
    public static void AddCoreServices(this IServiceCollection services)
    {
        services.AddScoped<IImageManager, ImageManager>();
    }
}
