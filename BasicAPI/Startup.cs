using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BasicAPI.Entities;
using BasicAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace BasicAPI
{
    public class Startup
    {
        public static IConfigurationRoot Configuration;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appSetting.json", optional: false, reloadOnChange:true)
                .AddJsonFile($"appSettings.{env.EnvironmentName}.json", optional:true, reloadOnChange:true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                    .AddMvcOptions(o => o.OutputFormatters.Add(
                        new XmlDataContractSerializerOutputFormatter()));
            //.AddJsonOptions(o => {
            //    if(o.SerializerSettings.ContractResolver != null)
            //    {
            //        var castedResolver = o.SerializerSettings.ContractResolver 
            //                              as DefaultContractResolver;

            //        castedResolver.NamingStrategy = null;
            //    }
            //});
#if DEBUG

            services.AddTransient<IMailService, LocalMailService>();
#else
            services.AddTransient<IMailService, CloudMailService>();
#endif
            var connectionString = Startup.Configuration["connectionStrings:cityInfoDBConnectionString"];
            services.AddDbContext<CityInfoContext>(o => o.UseSqlServer(connectionString));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, CityInfoContext cityInfoContext)
        {
            loggerFactory.AddConsole();

            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler();
            }

            cityInfoContext.EnsureSeedDataForContext();

            app.UseStatusCodePages();

            app.UseMvc();

            //app.Run((context) =>
            //{
            //    throw new Exception("Example exception");
            //});

            /*app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });*/
        }
    }
}
