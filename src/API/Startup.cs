using API.ExceptionHandlers;
using API.Extensions;
using API.Middlewares;
using API.Workers.Outbox;
using Application;
using Infrastructure;
using Infrastructure.Persistence.Outbox;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication();
            services.AddInfrastructure(Configuration);

            services.Configure<OutboxOptions>(Configuration.GetSection(OutboxOptions.SectionName));
            var outboxOptions = Configuration.GetSection(OutboxOptions.SectionName).Get<OutboxOptions>();
            if (outboxOptions is not null &&
                outboxOptions.Habilitado)
            {
                services.AddHostedService<ProcessadorOutboxBackgroundService>();
            }

            services.AddControllers();

            services.AddOpenApi();
            services.AddSwaggerGen();

            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();

            services.AddConfiguracaoRateLimit(Configuration);

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler();

            //if (env.IsDevelopment())
            //{
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.DocExpansion(DocExpansion.None);
                c.DefaultModelsExpandDepth(-1);
            });
            //}

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("DefaultCorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<VerificacaoBanimentoMiddleware>();
            app.UseRateLimiter();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
