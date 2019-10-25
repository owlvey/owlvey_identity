using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Polly.Registry;
using System;
using System.Net.Http;
using System.Reflection;
using Microsoft.IdentityModel.Logging;
using Owlvey.Falcon.Authority.Infra.CrossCutting.Identity;
using Owvley.Falcon.Authority.Domain.Models;


namespace Owlvey.Falcon.Authority.Infra.CrossCutting.IoC
{
    public class DependencyInjectorBootStrapper
    { 
        public static void RegisterServices(IServiceCollection services, IHostingEnvironment environment, IConfiguration configuration)
        {
            IdentityModelEventSource.ShowPII = true;

            //Application            
            services.AddScoped<IIdentityService, IdentityService>();


            var connectionString = configuration.GetConnectionString("DefaultConnection");

            //if (environment.IsDevelopment())
            //{
            //    services.AddDbContext<Data.Sqlite.Contexts.FalconAuthDbContext>(options =>
            //        options.UseSqlite(connectionString)
            //    );


            //    services.AddIdentity<User, IdentityRole>(o =>
            //    {
            //        // SignIn settings
            //        // o.SignIn.RequireConfirmedEmail = true;

            //        // User settings
            //        o.User.RequireUniqueEmail = true;

            //        // Password settings
            //        o.Password.RequireDigit = true;
            //        o.Password.RequireLowercase = true;
            //        o.Password.RequireUppercase = true;
            //        o.Password.RequireNonAlphanumeric = true;
            //        o.Password.RequiredLength = 8;
            //    })
            //    .AddEntityFrameworkStores<Data.Sqlite.Contexts.FalconAuthDbContext>()
            //    .AddDefaultTokenProviders();
            //}
            //else
            //{
            //    services.AddDbContext<Data.SqlServer.Contexts.FalconAuthDbContext>(options =>
            //     options.UseLazyLoadingProxies()
            //            .UseSqlServer(connectionString,
            //            sqlServerOptionsAction: sqlOptions =>
            //            {
            //                sqlOptions.EnableRetryOnFailure(
            //                maxRetryCount: 10,
            //                maxRetryDelay: TimeSpan.FromSeconds(30),
            //                errorNumbersToAdd: null);
            //            })
            // );


            //    services.AddIdentity<User, IdentityRole>(o =>
            //    {
            //        // SignIn settings
            //        // o.SignIn.RequireConfirmedEmail = true;

            //        // User settings
            //        o.User.RequireUniqueEmail = true;

            //        // Password settings
            //        o.Password.RequireDigit = true;
            //        o.Password.RequireLowercase = true;
            //        o.Password.RequireUppercase = true;
            //        o.Password.RequireNonAlphanumeric = true;
            //        o.Password.RequiredLength = 8;
            //    })
            //    .AddEntityFrameworkStores<Data.SqlServer.Contexts.FalconAuthDbContext>()
            //    .AddDefaultTokenProviders();
            //}

            services.AddDbContext<Data.Sqlite.Contexts.FalconAuthDbContext>(options =>
                   options.UseInMemoryDatabase(connectionString)
               );


            services.AddIdentity<User, IdentityRole>(o =>
            {
                // SignIn settings
                // o.SignIn.RequireConfirmedEmail = true;

                // User settings
                o.User.RequireUniqueEmail = true;

                // Password settings
                o.Password.RequireDigit = true;
                o.Password.RequireLowercase = true;
                o.Password.RequireUppercase = true;
                o.Password.RequireNonAlphanumeric = true;
                o.Password.RequiredLength = 8;
            })
            .AddEntityFrameworkStores<Data.Sqlite.Contexts.FalconAuthDbContext>()
            .AddDefaultTokenProviders();


            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });

            
            services.AddIdentityServer(options =>
            {
                options.UserInteraction.LoginUrl = "/Identity/Account/Login";
                options.UserInteraction.LogoutUrl = "/Identity/Account/Logout";

                options.Events.RaiseSuccessEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseErrorEvents = true;
            })
            .AddDeveloperSigningCredential()
            .AddAspNetIdentity<User>()
            .AddConfigurationStore(options =>
            {

                //if (environment.IsDevelopment())
                //{
                //    options.ConfigureDbContext = builder =>
                //builder.UseSqlite(connectionString,
                //    sqliteOptionsAction: sqlOptions =>
                //    {
                //        sqlOptions.MigrationsAssembly(migrationsAssembly);
                //    });
                //}
                //else {
                //    options.ConfigureDbContext = builder =>
                //   builder.UseSqlServer(connectionString,
                //       sqlServerOptionsAction: sqlOptions =>
                //       {
                //           sqlOptions.MigrationsAssembly(migrationsAssembly);
                //           sqlOptions.EnableRetryOnFailure(
                //               maxRetryCount: 10,
                //               maxRetryDelay: TimeSpan.FromSeconds(30),
                //               errorNumbersToAdd: null);
                //       });
                //}

                options.ConfigureDbContext = builder =>
                builder.UseInMemoryDatabase(connectionString);


            })
            .AddOperationalStore(options =>
            {
                //if (environment.IsDevelopment())
                //{
                //    options.ConfigureDbContext = builder =>
                //    builder.UseSqlite(connectionString,
                //        sqliteOptionsAction: sqlOptions =>
                //        {
                //            sqlOptions.MigrationsAssembly(migrationsAssembly);
                //        });
                //}
                //else
                //{
                //    options.ConfigureDbContext = builder =>
                //    builder.UseSqlServer(connectionString,
                //        sqlServerOptionsAction: sqlOptions =>
                //        {
                //            sqlOptions.MigrationsAssembly(migrationsAssembly);
                //            sqlOptions.EnableRetryOnFailure(
                //                maxRetryCount: 10,
                //                maxRetryDelay: TimeSpan.FromSeconds(30),
                //                errorNumbersToAdd: null);
                //        });
                //}

                options.ConfigureDbContext = builder =>
                   builder.UseInMemoryDatabase(connectionString);


                // this enables automatic token cleanup. this is optional.
                options.EnableTokenCleanup = true;
                options.TokenCleanupInterval = 30;
            });
            //.AddProfileService<FalconProfileService>();

            services.AddHttpContextAccessor();
            services.AddHttpClient();

            // Domain
            //services.AddScoped<IDomainManagerService, DomainManagerService>();
            
            

            //Polly Policies
            IPolicyRegistry<string> registry = services.AddPolicyRegistry();

            IAsyncPolicy<HttpResponseMessage> httpRetryPolicy =
                Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode && r.StatusCode != System.Net.HttpStatusCode.Conflict)
                    .RetryAsync(3);

            registry.Add("SimpleHttpRetryPolicy", httpRetryPolicy);

            Random jitterer = new Random();

            IAsyncPolicy<HttpResponseMessage> httWaitAndpRetryPolicy =
                Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode && r.StatusCode != System.Net.HttpStatusCode.Conflict)
                    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
                                                        + TimeSpan.FromMilliseconds(jitterer.Next(0, 100)));

            registry.Add("SimpleWaitAndRetryPolicy", httWaitAndpRetryPolicy);

            IAsyncPolicy<HttpResponseMessage> noOpPolicy = Policy.NoOpAsync()
                .AsAsyncPolicy<HttpResponseMessage>();

            registry.Add("NoOpPolicy", noOpPolicy);

            services.AddHttpClient("RemoteServerFromService", client =>
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            })
            .AddPolicyHandlerFromRegistry((policyRegistry, httpRequestMessage) =>
            {
                if (httpRequestMessage.Method == HttpMethod.Get)
                {
                    return policyRegistry.Get<IAsyncPolicy<HttpResponseMessage>>("SimpleHttpRetryPolicy");
                }
                else if (httpRequestMessage.Method == HttpMethod.Post)
                {
                    return policyRegistry.Get<IAsyncPolicy<HttpResponseMessage>>("NoOpPolicy");
                }
                else
                {
                    return policyRegistry.Get<IAsyncPolicy<HttpResponseMessage>>("SimpleWaitAndRetryPolicy");
                }
            })
            .AddPolicyHandler((httpRequestMessage) => {
                return HttpPolicyExtensions.HandleTransientHttpError().CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
            });

            services.AddHttpClient("RemoteServerFromWorker", client =>
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            })
            .AddPolicyHandlerFromRegistry((policyRegistry, httpRequestMessage) =>
            {
                if (httpRequestMessage.Method == HttpMethod.Get)
                {
                    return policyRegistry.Get<IAsyncPolicy<HttpResponseMessage>>("SimpleHttpRetryPolicy");
                }
                else if (httpRequestMessage.Method == HttpMethod.Post)
                {
                    return policyRegistry.Get<IAsyncPolicy<HttpResponseMessage>>("NoOpPolicy");
                }
                else
                {
                    return policyRegistry.Get<IAsyncPolicy<HttpResponseMessage>>("SimpleWaitAndRetryPolicy");
                }
            })
            .AddPolicyHandler((httpRequestMessage) => {
                return HttpPolicyExtensions.HandleTransientHttpError().CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
            });
        }
    }
}
