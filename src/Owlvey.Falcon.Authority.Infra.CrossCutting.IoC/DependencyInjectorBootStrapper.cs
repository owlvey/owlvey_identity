using TheRoostt.Authority.Domain.Interfaces;
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
using Owlvey.Falcon.Authority.Infra.Data.SqlServer.Contexts;
using Owvley.Falcon.Authority.Domain.Models;
using Owvley.Falcon.Authority.Domain.Core.Manager;
using Owlvey.Falcon.Authority.Infra.Data.SqlServer.Repositories;
using Owvley.Flacon.Application.Services.Interfaces;
using Owvley.Flacon.Application.Services;
using Owvley.Falcon.Authority.Domain.Interfaces;
using Owvley.Flacon.Authority.Application.Interfaces;

namespace Owlvey.Falcon.Authority.Infra.CrossCutting.IoC
{
    public class DependencyInjectorBootStrapper
    { 
        public static void RegisterServices(IServiceCollection services, IHostingEnvironment environment, IConfiguration configuration)
        {
            IdentityModelEventSource.ShowPII = true;

            //Application
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserQueryService, UserQueryService>();
            services.AddScoped<IUserPreferenceService, UserPreferenceService>();
            services.AddScoped<IUserActivityService, UserActivityService>();
            services.AddScoped<IUserActivityQueryService, UserActivityQueryService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ICustomerMemberService, CustomerMemberService>();
            services.AddScoped<ICustomerQueryService, CustomerQueryService>();

            services.AddScoped<IIdentityService, IdentityService>();
            
            services.AddDbContext<FalconAuthDbContext>(options =>
                options.UseLazyLoadingProxies()
                       .UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                       sqlServerOptionsAction: sqlOptions =>
                       {
                           sqlOptions.EnableRetryOnFailure(
                           maxRetryCount: 10,
                           maxRetryDelay: TimeSpan.FromSeconds(30),
                           errorNumbersToAdd: null);
                       })
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
            .AddEntityFrameworkStores<FalconAuthDbContext>()
            .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Identity/Account/Login";
                options.LogoutPath = $"/Identity/Account/Logout";
                options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
            });

            var migrationsAssembly = typeof(FalconAuthDbContext).GetTypeInfo().Assembly.GetName().Name;
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
                options.ConfigureDbContext = builder =>
                    builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                        sqlServerOptionsAction: sqlOptions =>
                        {
                            sqlOptions.MigrationsAssembly(migrationsAssembly);
                            sqlOptions.EnableRetryOnFailure(
                                maxRetryCount: 10,
                                maxRetryDelay: TimeSpan.FromSeconds(30),
                                errorNumbersToAdd: null);
                        });
            })
            .AddOperationalStore(options =>
            {
                options.ConfigureDbContext = builder =>
                    builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                        sqlServerOptionsAction: sqlOptions =>
                        {
                            sqlOptions.MigrationsAssembly(migrationsAssembly);
                            sqlOptions.EnableRetryOnFailure(
                                maxRetryCount: 10,
                                maxRetryDelay: TimeSpan.FromSeconds(30),
                                errorNumbersToAdd: null);
                        });
                // this enables automatic token cleanup. this is optional.
                options.EnableTokenCleanup = true;
                options.TokenCleanupInterval = 30;
            });
            //.AddProfileService<FalconProfileService>();

            services.AddHttpContextAccessor();
            services.AddHttpClient();

            // Domain
            services.AddScoped<IDomainManagerService, DomainManagerService>();
            
            // Infra - Data
            services.AddTransient<IUserRepository, UserSqlServerRepository>();
            services.AddTransient<ICustomerRepository, CustomerSqlServerRepository>();
            services.AddTransient<ICustomerMemberRepository, CustomerMemberSqlServerRepository>();
            

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
