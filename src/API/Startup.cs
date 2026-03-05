using API.ExceptionHandlers;
using API.Workers.Outbox;
using Application;
using Infrastructure;
using Infrastructure.Persistence.Outbox;
using Microsoft.Extensions.Configuration;

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
            if (outboxOptions is not null && outboxOptions.Habilitado)
            {
                services.AddHostedService<ProcessadorOutboxBackgroundService>();
            }

            services.AddControllers();

            services.AddOpenApi();
            services.AddSwaggerGen();

            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
