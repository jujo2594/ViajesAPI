using Asp.Versioning;
using AspNetCoreRateLimit;
using Core.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.UnitOfWork;

namespace ViajesAPI.Extensions
{
    public static class ApplicationServiceExtension
    {
        public static void ConfigureApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = new HeaderApiVersionReader("X-Version");
            });
        }
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options => {
                options.AddPolicy(
                    "CorsPolicy", builder =>
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                );
            });
        }

        public static void AddAplicationService(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
        public static void ConfigureRateLimiting(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddInMemoryRateLimiting();
            services.Configure<IpRateLimitOptions>(options => {
                options.EnableEndpointRateLimiting = true;
                options.StackBlockedRequests = false;
                options.HttpStatusCode = 429;
                options.RealIpHeader = "X-Real-Ip";
                options.GeneralRules = new List<RateLimitRule>
            {
                new RateLimitRule
                {
                    Endpoint = "*",
                    Period = "10s",
                    Limit = 2
                }
            };
            });
        }
    }
}
