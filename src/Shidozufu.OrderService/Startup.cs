using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Shidozufu.EventBus;
using Shidozufu.Shared;

namespace Shidozufu.OrderService
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration) =>
            _configuration = configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<DatabaseOptions>(
                _configuration.GetSection(nameof(DatabaseOptions)),
                opt => _configuration.Bind(opt));
            services.AddControllers();
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Order Service",
                    Version = "v1"
                });
            });
            services.AddSingleton<IOrderDetailsProvider, OrderDetailsProvider>();
            services.AddSingleton<ICreateOrder, CreateOrder>();
            services.AddSingleton<IDeleteOrder, DeleteOrder>();

            //services.UsePlainRabbitMq();
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
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "Order Service");
            });
            app.UseRouting();

            app.UseEndpoints(e =>
            {
                e.MapGet("/", async context => await context.Response.WriteAsync("Shidozufu.OrderService"));
                e.MapControllers();
            });
        }
    }
}
