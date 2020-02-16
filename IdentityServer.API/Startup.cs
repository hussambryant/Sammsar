using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using IdentityServer.API.Data;
using IdentityServer4.AspNetIdentity;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.API
{
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
            services.AddDbContext<IdentityServerDbContext>(options =>
                options.UseNpgsql(
                    Configuration.GetConnectionString("Postgres"),
                    sqlOptions => { sqlOptions.EnableRetryOnFailure(15, TimeSpan.FromSeconds(30), null); }));

            services.AddEntityFrameworkNpgsql()
                .AddDbContext<IdentityServerDbContext>(option =>
                    option.UseNpgsql(Configuration.GetConnectionString("Postgres"))
                );

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<IdentityServerDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = "https://localhost:5001/";
                options.Audience = "xxx";
                options.RequireHttpsMetadata = false;
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            context.Response.Headers.Add("Token-Expired", "true");
                        return Task.CompletedTask;
                    }
                };
            });

            var connectionString = Configuration.GetConnectionString("Postgres");
            var migrationsAssembly = typeof(IdentityServerDbContext).GetTypeInfo().Assembly.GetName().Name;

            services.AddAuthorization(x =>
            {
                x.AddPolicy("User", builder => builder.RequireClaim("UserType", ((int)UserType.User).ToString()));
                x.AddPolicy("Partner", builder => builder.RequireClaim("UserType", ((int)UserType.Partner).ToString()));
                x.AddPolicy("Operation", builder => builder.RequireClaim("UserType", ((int)UserType.Operation).ToString()));
            });

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                .AddAspNetIdentity<User>()
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder => builder.UseNpgsql(connectionString,
                        sqlOptions =>
                        {
                            sqlOptions.MigrationsAssembly(migrationsAssembly);
                            //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                            sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30),
                                null);
                        });
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder => builder.UseNpgsql(connectionString,
                        sqlOptions =>
                        {
                            sqlOptions.MigrationsAssembly(migrationsAssembly);
                            //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                            sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30),
                                null);
                        });
                })
                .AddProfileService<ProfileService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            MigrateIdentityServerData(app);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
        }

        private void MigrateIdentityServerData(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();

                if (!context.Clients.Any())
                {
                    context.Clients.AddRange(InMemoryConfigurations.Clients()
                        .Select(x => x.ToEntity()));
                }

                if (!context.ApiResources.Any())
                {
                    context.ApiResources.AddRange(InMemoryConfigurations.ApiResources()
                        .Select(x => x.ToEntity()));
                }

                context.SaveChanges();
            }

        }
    }
}
