using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Bank4Us.BusinessLayer.Core;
using Bank4Us.BusinessLayer.Managers.AccountManagement;
using Bank4Us.BusinessLayer.Managers.CustomerManagement;
using Bank4Us.BusinessLayer.Rules;
using Bank4Us.DataAccess.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NRules;
using NRules.Fluent;
using NRules.RuleModel;
using Swashbuckle.AspNetCore.Swagger;

namespace Bank4Us.ServiceApp
{
    /// <summary>
    ///   Course Name: MSCS 6360 Enterprise Architecture
    ///   Year: Fall 2018
    ///   Name: William J Leannah
    ///   Description: Example implementation of a Service App with MVC           
    /// </summary>
    /// 
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
            //INFO: BRE example implementation.  
            // https://github.com/NRules/NRules/wiki/Getting-Started
            var ruleset = new RuleRepository();
            ruleset.Load(x => x.From(typeof(AdultOwnershipRule).Assembly));

            //Compile rules
            var brefactory = ruleset.Compile();

            //Create a working session
            ISession businessRulesEngine = brefactory.CreateSession();


            //INFO: Dependency injection is a technique that follows the Dependency Inversion
            //      Principle, allowing for applications to be composed of loosely coupled modules.
            //     ASP.NET Core has built-in support for dependency injection, which makes applications
            //    easier to test and maintain.
            //  https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/dependency-injection?view=aspnetcore-2.1

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IDbFactory, DbFactory>();
            services.AddScoped<DataContext>();
            services.AddScoped<IRepository, Repository>();
            services.AddSingleton<ISession>(businessRulesEngine);
            services.AddScoped<ICustomerManager, CustomerManager>();
            services.AddScoped<IAccountManager, AccountManager>();
            services.AddScoped<BusinessManagerFactory>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //INFO:Register the Swagger generator, defining 1 or more Swagger documents
            //https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-2.1&tabs=visual-studio%2Cvisual-studio-xml
            services.AddSwaggerGen(c =>
            {
                c.DescribeAllEnumsAsStrings();
                c.DescribeAllParametersInCamelCase();
                c.SwaggerDoc("v1", new Info { Title = "Bank4Us API", Version = "V1" });

            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(LogLevel.Information);
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            //INFO: Enable middleware to serve generated Swagger as a JSON endpoint.
            // https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-2.1&tabs=visual-studio%2Cvisual-studio-xml
            app.UseSwagger();

            //INFO: Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            //INFO: specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Bank4Us API V1");
               
            });
            
            app.UseHttpsRedirection();
            app.UseMvc();

        }
    }
}
