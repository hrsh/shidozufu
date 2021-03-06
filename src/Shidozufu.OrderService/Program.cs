using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Shidozufu.OrderService;

await Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseStartup<Startup>();
    }).Build()
    .RunAsync();