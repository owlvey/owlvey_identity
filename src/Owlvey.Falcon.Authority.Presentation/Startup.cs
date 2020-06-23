using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Owlvey.Falcon.Authority.Presentation.Constants;
using Owlvey.Falcon.Authority.Infra.Settings;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Authorization;
using Owlvey.Falcon.Authority.Infra.CrossCutting.IoC;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using IdentityServer4.AccessTokenValidation;
using Owlvey.Flacon.Authority.Infra.CrossCutting.Logging.Middlewares;
using Owlvey.Falcon.Authority.Infra.Data.Sqlite.Seed;
using System.Text;
using System;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Owlvey.Falcon.Authority.Presentation
{
    public class Startup
    {
        public IHostingEnvironment Environment { get; }
        public IConfiguration Configuration { get; }
        public static void LogRequestHeaders(IApplicationBuilder app, 
            ILoggerFactory loggerFactory)
        {
            var logger = loggerFactory.CreateLogger("app.headers");
            app.Use(async (context, next) =>
            {
                
                var builder = new StringBuilder("\n");
                foreach (var header in context.Request.Headers)
                {
                    builder.AppendLine($"{header.Key}:{header.Value}");
                }
                logger.LogWarning(builder.ToString());                
                await next.Invoke();
            });
        }
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            var builder = new ConfigurationBuilder()
              .SetBasePath(environment.ContentRootPath)
              .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
              .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true)
              .AddEnvironmentVariables();

            if (environment.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.AddInMemoryCollection(configuration.AsEnumerable()).Build();
            Environment = environment;            
        }

        

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddAuthorization(o =>
            {
                var authorizationPolicy = new AuthorizationPolicyBuilder(SchemeContants.DefaultSchema)
                    .RequireAuthenticatedUser()
                    .Build();
                o.AddPolicy("RequireLoggedOnUsers", authorizationPolicy);
            });

            services.Configure<ApiSettings>(Configuration.GetSection("Api"));
            services.Configure<WebSettings>(Configuration.GetSection("Web"));
            
            services.AddResponseCaching();

            services.AddHealthChecks();

            //services.Configure<ForwardedHeadersOptions>(options =>
            //{
            //    options.KnownNetworks.Clear();
            //    options.KnownProxies.Clear();
            //    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            //});
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.All;                
            });

            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
                options.CacheProfiles.Add("Default60", new CacheProfile()
                {
                    Duration = 60
                });

                //Authorize Filter
                options.Filters.Add(new AuthorizeFilter("RequireLoggedOnUsers"));
                //Https Filter
                AddMvcOptions(options);

            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
            .AddRazorPagesOptions(options =>
            {                
                options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
                options.Conventions.AuthorizeAreaPage("Identity", "/Account/Logout");
            });
            /*
             * .AddJsonOptions(
                //options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            )
            */
            AddAuthentication(services);

            DependencyInjectorBootStrapper.RegisterServices(services, Environment, Configuration);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/proxy-load-balancer?view=aspnetcore-3.1
            //app.UsePathBase("/authority");
            //if (env.EnvironmentName == "k8s")
            //{
            //    app.Use((context, next) =>
            //    {
            //        context.Request.PathBase = new PathString("/authority");
            //        return next();
            //    });
            //}

            //app.UseForwardedHeaders();
            //app.Use((context, next) =>
            //{
            //    if (context.Request.Path.StartsWithSegments("/authority", out var remainder))
            //    {
            //        context.Request.Path = remainder;
            //    }
            //    return next();
            //});

            LogRequestHeaders(app, app.ApplicationServices.GetService<ILoggerFactory>());
            app.UseSerilogRequestLogging(options =>
            {                
                // Attach additional properties to the request completion event
                options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                {
                    diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                    diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
                };
            });

            app.UseHealthChecks("/hc");
                        
            app.UseFalconLogging("/Home/Error");

            app.UseCors(o => o.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseStaticFiles();

            app.UseIdentityServer();

            app.UseResponseCaching();

            app.InitializeDbTestData(Configuration);

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        /// <summary>
        /// Auth section (will be override when testing)
        /// </summary>
        /// <param name="services"></param>
        public virtual void AddAuthentication(IServiceCollection services)
        {
            services.Configure<AuthenticationSettings>(Configuration.GetSection("Authentication"));

            var sp = services.BuildServiceProvider();

            var authenticationSettings = sp.GetService<IOptions<AuthenticationSettings>>();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = SchemeContants.ApplicationSchema;
            })
            .AddPolicyScheme(SchemeContants.DefaultSchema, SchemeContants.DefaultSchema, options =>
            {
                options.ForwardDefaultSelector = c =>
                {
                    if (c.Request.Path.StartsWithSegments("/api"))
                    {
                        return JwtBearerDefaults.AuthenticationScheme;
                    }
                    return SchemeContants.ApplicationSchema;
                };
            })
            .AddIdentityServerAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.Authority = authenticationSettings.Value.Authority;
                options.RequireHttpsMetadata = false;
                options.ApiName = authenticationSettings.Value.ApiName;
                options.NameClaimType = authenticationSettings.Value.NameClaimType;
                options.RoleClaimType = authenticationSettings.Value.RoleClaimType;
            });
        }

        public virtual void AddMvcOptions(MvcOptions options)
        {
            //options.Filters.Add(new RequireHttpsAttribute());
        }

        public virtual void AddMiddleware(IApplicationBuilder app)
        {
            //app.UseForwardedHeaders();
            //app.UseHttpsRedirection();
        }
    }
}
