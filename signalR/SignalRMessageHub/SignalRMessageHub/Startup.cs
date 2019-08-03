using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SignalRMessageHub.Hubs;

namespace SignalRMessageHub
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_3_0)
                            .AddMvcOptions(options => {
                                options.EnableEndpointRouting = false;
                            });

            services.AddCors(options => options.AddPolicy("CorsPolicy",
          builder =>
          {
              builder.WithOrigins("", "null")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();

          }));
            services.AddSignalR();
        }

        [Obsolete]
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            //Disabling HttpsRedirection for supporting python web-socket client
            //app.UseHttpsRedirection();

            app.UseStaticFiles();
            
            app.UseCors("CorsPolicy");

            app.UseSignalR(routes => {
                routes.MapHub<MessageHub>("/messagehub");
            });
            app.UseMvc();
        }
    }
}
