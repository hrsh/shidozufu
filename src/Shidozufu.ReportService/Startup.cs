using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Shidozufu.EventBus;

namespace Shidozufu.ReportService
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.UsePlainRabbitMq();
            services.AddSingleton<IMemoryReportStorage, MemoryReportStorage>();
            services.AddHostedService<ReportDataCollector>();
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Report Service",
                    Version = "v1"
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "Report Service");
            });
            app.UseRouting();

            app.UseEndpoints(e =>
            {
                e.MapGet("/", async context => 
                    await context.Response.WriteAsync("Shidozufu.ReportService"));
                e.MapControllers();
            });
        }
    }
}