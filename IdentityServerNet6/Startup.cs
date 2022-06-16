using IdentityServer4.Services;
using IdentityServerApi.Services;
using IdentityServerNet6.DataAccess.Contexts;
using IdentityServerNet6.Entities;
using IdentityServerNet6.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace IdentityServerNet6
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
            services.AddDbContext<IdentityDbContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("IdentityDbConnection"));
            });

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // User Settings
                options.User.RequireUniqueEmail = true;

                // Password Settings
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;

                // Default Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
            });

            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            services.AddIdentityServer()
                .AddDeveloperSigningCredential()
                //.AddInMemoryApiResources(Config.ResourceManager.ApiResources)
                //.AddInMemoryApiScopes(Config.ScopeManager.ApiScopes)
                //.AddInMemoryClients(Config.ClientManager.Clients)
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseNpgsql(Configuration.GetConnectionString("IdentityDbConnection"),
                            sqlOptions =>
                            {
                                sqlOptions.MigrationsAssembly(migrationsAssembly);
                                sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                            });
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                        builder.UseNpgsql(Configuration.GetConnectionString("IdentityDbConnection"),
                            sqlOptions => 
                            {
                                sqlOptions.MigrationsAssembly(migrationsAssembly);
                                sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                            });
                    options.EnableTokenCleanup = true;
                    options.TokenCleanupInterval = 3600;
                })
                .AddProfileService<ProfileService>()
                .AddAspNetIdentity<ApplicationUser>()
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>();

            services.AddTransient<IProfileService, ProfileService>();

            

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddIdentityServerAuthentication(options =>
                    {
                        options.Authority = Configuration["IdentityServerURL"];
                        options.RequireHttpsMetadata = false;
                        options.ApiName = "identiy_service_api";
                    });

            services.AddControllers();
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "IdentityServerDemo", Version = "v1" });
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseSwagger();
                //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "IdentityServerDemo v1"));
            }

            app.UseIdentityServer();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
