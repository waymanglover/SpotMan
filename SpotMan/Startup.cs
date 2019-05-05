using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SpotMan.OptionModels;

//using Swashbuckle.AspNetCore.Swagger;

namespace SpotMan
{
    /// <summary>
    /// Main entry point for Spotify Manager.
    /// </summary>
    /// <remarks>Swashbuckle hasn't been updated to support .NET Core 3.0 preview.</remarks>
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            try
            {
                services
                    .Configure<UserAuthOptions>(Configuration.GetSection("UserAuth"))
                    .AddScoped(sp => sp.GetService<IOptionsSnapshot<UserAuthOptions>>().Value)
                    .Configure<SpotifyOptions>(Configuration.GetSection("Spotify"))
                    .AddScoped(sp => sp.GetService<IOptionsSnapshot<SpotifyOptions>>().Value)
                    .AddMvc()
                    //.AddSwaggerGen(c => { c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" }); })
                    .AddNewtonsoftJson();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // ReSharper disable once UnusedMember.Global
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                // The default HSTS value is 30 days.
                // You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();

            app.UseHttpsRedirection()
                .UseRouting()
                .UseAuthorization()
                .UseEndpoints(endpoints => { endpoints.MapControllers(); })
                // Enable middleware to serve generated Swagger as a JSON endpoint.
                //.UseSwagger()
                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
                // specifying the Swagger JSON endpoint.
                /*.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                    c.RoutePrefix = string.Empty;
                })
                */
                ;
        }
    }
}