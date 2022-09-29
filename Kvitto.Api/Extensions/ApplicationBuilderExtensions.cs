using Kvitto.Core.Repositories;
using Kvitto.Data.Data;
namespace Kvitto.Api.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task<IApplicationBuilder> SeedDataAsync(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                IDevUoW uow = (IDevUoW)(serviceProvider.GetRequiredService<IUoW>());

                uow.EnsureDeleted();
                uow.Migrate();

                try
                {
                    await SeedData.InitAsync(uow);
                }
                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating the database.");
                }
            }
            return app;
        }
    }
}
