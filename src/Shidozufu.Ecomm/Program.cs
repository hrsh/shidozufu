using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Shidozufu.Ecomm;

await Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(builder =>
    {
        builder.UseStartup<Startup>();
    }).Build().StartAsync();
