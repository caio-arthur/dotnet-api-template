using Application.Common.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Auditoria;
using Infrastructure.Persistence.Interceptors;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DepedendencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            services.Configure<AuditoriaOptions>(configuration.GetSection(AuditoriaOptions.SectionName));
            services.AddScoped<AtualizarEntidadesAuditaveisInterceptor>();

            services.AddDbContext<ApplicationDbContext>((sp, options) =>
            {
                options.UseSqlite(configuration.GetConnectionString("SqliteConnection"));
                options.AddInterceptors(sp.GetRequiredService<AtualizarEntidadesAuditaveisInterceptor>());
            });

            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

            return services;
        }
    }
}
