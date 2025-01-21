namespace Multiple_Layered.API
{
    public static class LifecycleOptions
    {
        public static void AddLifecycle(this IServiceCollection services) 
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderProductService, OrderProductService>();
            services.AddScoped<ITokenService>(provider =>
            {
                var tokenOptions = provider.GetRequiredService<IOptions<CustomTokenOptions>>().Value;
                var userManager = provider.GetRequiredService<UserManager<User>>();
                return new TokenService(tokenOptions, userManager);
            });
           
        }
    }
}
