using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeSjoerd.ApplicationInsightsProxy.DebugModes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProxyKit;

namespace DeSjoerd.ApplicationInsightsProxy
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddProxy();
            services.AddApplicationInsightsTelemetry();
            services.AddMvc();

            services.AddSingleton<ExceptionModeSetting>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            app.Use((context, next) =>
            {
                var exceptionMode = context.RequestServices.GetRequiredService<ExceptionModeSetting>();

                if(exceptionMode.Enabled)
                {
                    throw new Exception("ExceptionMode!");
                }

                return next();
            });

            app.RunProxy(context => context
                .ForwardTo(Configuration.GetValue<string>("ProxyUrl"))
                .AddXForwardedHeaders()
                .Send());
        }
    }
}
