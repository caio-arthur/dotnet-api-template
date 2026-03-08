namespace API.Extensions
{
    public static class CorsExtensions
    {
        public static IServiceCollection AddCustomCors(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment ambiente)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("DefaultCorsPolicy", politica =>
                {
                    if (ambiente.IsDevelopment())
                    {
                        // Ambiente de Desenvolvimento: Permite tudo
                        politica.AllowAnyOrigin()
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    }
                    else
                    {
                        // Ambiente de Produção: Restrito aos domínios do appsettings
                        var origensPermitidas = configuration.GetSection("Cors:OrigensPermitidas").Get<string[]>() ?? [];

                        if (origensPermitidas.Length > 0)
                        {
                            politica.WithOrigins(origensPermitidas)
                                  .AllowAnyHeader() 
                                  .AllowAnyMethod() 
                                  .AllowCredentials();
                        }
                        else
                        {
                            politica.WithOrigins("https://dominio-seguro-padrao.com.br");
                        }
                    }
                });
            });

            return services;
        }
    }
}
