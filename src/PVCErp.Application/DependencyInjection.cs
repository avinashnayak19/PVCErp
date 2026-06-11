using Microsoft.Extensions.DependencyInjection;
using PVCErp.Application.Abstractions;
using PVCErp.Application.Services;

namespace PVCErp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IInventoryService, InventoryService>();
        services.AddScoped<IProductionService, ProductionService>();
        services.AddScoped<IDispatchService, DispatchService>();
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<ISocketingService, SocketingService>();
        return services;
    }
}
