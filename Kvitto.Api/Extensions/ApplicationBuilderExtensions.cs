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

#pragma warning disable CS0168 // Variable is declared but never used
                try
                {
                    await SeedData.InitAsync(uow);
                }
                catch (Exception e)
                {
                    throw;
                }
#pragma warning restore CS0168 // Variable is declared but never used
            }
            return app;
        }
    }
}
