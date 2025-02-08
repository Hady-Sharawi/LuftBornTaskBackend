using Service.Contracts;
using Service.Services;

namespace WebApi.Extentions;

public static class ServicesInjections
{
    public static void InjectServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IProductService, ProductService>();
    }
}
