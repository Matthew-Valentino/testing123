﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Bank4Us.BusinessLayer.Core;
using Bank4Us.BusinessLayer.Managers.AccountManagement;
using Bank4Us.BusinessLayer.Managers.CustomerManagement;
using Bank4Us.BusinessLayer.Rules;
using Bank4Us.Common.Core;
using Bank4Us.DataAccess.Core;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
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
using Bank4Us.ServiceApp.Services;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Bank4Us.Common.CanonicalSchema;

namespace Bank4Us.ServiceApp
{
    /// <summary>
    ///   Course Name: COSC 6360 Enterprise Architecture
    ///   Year: Fall 2023
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
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<DataContext>()
                .AddDefaultTokenProviders();

            // Enable Cross-Origin Requests (CORS) in ASP.NET Core
            //https://learn.microsoft.com/en-us/aspnet/core/security/?view=aspnetcore-6.0
            services.AddCors(options =>
            {
                options.AddPolicy(name: "CorsPolicy",
                    builder => builder.AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .SetIsOriginAllowed(origin => true)
                        );
            });

            //INFO: IdentityServer4 is an OpenID Connect and OAuth 2.0 framework for ASP.NET Core.
            //      http://docs.identityserver.io/en/latest/ 
            //Helpful Post:  https://fullstackmark.com/post/21/user-authentication-and-identity-with-angular-aspnet-core-and-identityserver
            services.AddIdentityServer()
                    .AddDeveloperSigningCredential()
                    .AddInMemoryPersistedGrants()
                    .AddInMemoryIdentityResources(IdSvrConfig.GetIdentityResources())
                    .AddInMemoryApiResources(IdSvrConfig.GetApiResources())
                    .AddInMemoryClients(IdSvrConfig.GetClients())
                    .AddAspNetIdentity<ApplicationUser>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                 .AddJwtBearer(jwt =>
                 {
                     jwt.Authority = "https://localhost:44346";
                     jwt.RequireHttpsMetadata = false;
                     jwt.Audience = "Bank4Us.ServiceApp";
                 });

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                                            .RequireAuthenticatedUser()
                                            .RequireClaim("EmployeeNumber")
                                            .Build();
            });

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
            services.AddSingleton<IEmailSender, EmailSender>();
            services.AddTransient<IProfileService, IdentityClaimsProfileService>();

            //INFO:Register the Swagger generator, defining 1 or more Swagger documents
            //https://learn.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-6.0&tabs=visual-studio
            services.AddSwaggerGen(c =>
            {
                //c.DescribeAllEnumsAsStrings();
                c.DescribeAllParametersInCamelCase();
                c.SwaggerDoc("v1", new OpenApiInfo{ Title = "Bank4Us API", Version = "V1" });

                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Flows = new OpenApiOAuthFlows()
                    {
                        Implicit = new OpenApiOAuthFlow()
                        {
                            AuthorizationUrl = new System.Uri("https://localhost:44346/connect/authorize"),
                            Scopes = new Dictionary<string, string> { { "Bank4Us.ServiceApp", "Bank4Us API - full access" } }
                        }
                    }
                });

                c.OperationFilter<AuthorizeCheckOperationFilter>();
            }
        );
            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
            }
            )
                .AddRazorPagesOptions(options =>
             {
                 options.Conventions.AuthorizeFolder("/Account/Manage");
                 options.Conventions.AuthorizePage("/Account/Logout");
             }).SetCompatibilityVersion(CompatibilityVersion.Latest);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // Enable Cross-Origin Requests (CORS) in ASP.NET Core
            //https://learn.microsoft.com/en-us/aspnet/core/security/?view=aspnetcore-6.0
            app.UseCors("CorsPolicy");

            //loggerFactory.AddConsole(LogLevel.Information);
            //loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            //INFO: Enable middleware to serve generated Swagger as a JSON endpoint.
            // 
            app.UseSwagger();

            //INFO: Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            //INFO: specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Bank4Us API V1");

                c.OAuthClientId("swaggerui");
                c.OAuthAppName("Bank4Us API - Swagger"); // presentation purposes only

            });

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseIdentityServer();
            app.UseMvc();

        }

        public class AuthorizeCheckOperationFilter : IOperationFilter
        {
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                MethodInfo info;
                context.ApiDescription.TryGetMethodInfo(out info);
                var hasAuthAttributes =  false;
  
                if (info != null) {
                    hasAuthAttributes = info.GetCustomAttributes(true)
                    .Union(info.DeclaringType.GetTypeInfo().GetCustomAttributes(true))
                    .Any(attr => attr.GetType() == typeof(AuthorizeAttribute));
                }             

                if (hasAuthAttributes)
                {
                    operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
                    operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

                    operation.Security = new List<OpenApiSecurityRequirement>
                    {
                    new OpenApiSecurityRequirement
                        {
                            [new OpenApiSecurityScheme {Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id = "oauth2"}}]
                                = new[] { "demo_api" }
                        }
                     };
                }
            }  
        }
    }
}
