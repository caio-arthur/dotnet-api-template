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
                var provider = configuration.GetValue<string>("Database:Provider");
                var connectionString = configuration.GetConnectionString("DefaultConnection");

                if (string.Equals(provider, "Sqlite", StringComparison.OrdinalIgnoreCase))
                {
                    options.UseSqlite(connectionString);
                }
                else if (string.Equals(provider, "Postgres", StringComparison.OrdinalIgnoreCase))
                {
                    options.UseNpgsql(connectionString);
                }
                else
                {
                    throw new InvalidOperationException($"Provedor de banco de dados não suportado: {provider}");
                }

                options.AddInterceptors(sp.GetRequiredService<AtualizarEntidadesAuditaveisInterceptor>());
            });

            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

            return services;
        }
    }
}
