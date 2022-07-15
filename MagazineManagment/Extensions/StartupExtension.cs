namespace MagazineManagment.Web.Extensions
{
    public static class StartupExtension
    {
        public static  IServiceCollection  InjectServices(this IServiceCollection services , IConfiguration configuration)
        {
            return services;
        }
    }
}
