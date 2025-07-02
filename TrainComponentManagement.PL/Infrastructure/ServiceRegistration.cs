using TrainComponentManagement.BLL.Services.Interfaces;
using TrainComponentManagement.DAL.Repositories.Interfaces;
using TrainComponentManagement.PL.Extensions;

namespace TrainComponentManagement.PL.Infrastructure;
public static class ServiceRegistration
{
    public static void RegisterBusinessLogicServices(this IServiceCollection services)
    {
        services.RegisterServicesFromAssembly(typeof(IComponentService).Assembly);
        services.RegisterServicesFromAssembly(typeof(IIdempotencyService).Assembly);
    }

    public static void RegisterDataAccessRepositories(this IServiceCollection services)
    {
        services.RegisterServicesFromAssembly(typeof(IComponentRepository).Assembly);
    }
}
