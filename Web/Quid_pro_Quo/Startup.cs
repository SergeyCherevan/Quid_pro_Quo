using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.AspNetCore.SignalR;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;

using System.Threading.Tasks;

using Quartz;

using Quid_pro_Quo.Authentification;
using Quid_pro_Quo.Database.Ralational;
using Quid_pro_Quo.Database.NoSql;
using Quid_pro_Quo.Repositories.Interfaces;
using Quid_pro_Quo.Repositories;
using Quid_pro_Quo.Services;
using Quid_pro_Quo.Services.Interfaces;
using Quid_pro_Quo.SignalRHubs;
using Quid_pro_Quo.AttachingRequests;

namespace Quid_pro_Quo
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Configuration.Bind("AuthOptions", new AuthOptions());

            //System.Console.Write("Please, input secret key: ");
            //AuthOptions.Key = System.Console.ReadLine();
            //System.Console.WriteLine();



            services.AddDbContext<QuidProQuoRelationalDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("SqliteConnection")));

            services.AddScoped<IQuidProQuoMongoDbContext, QuidProQuoMongoDbContext>(
                provider =>
                    new QuidProQuoMongoDbContext(
                        provider.GetRequiredService<IConfiguration>().GetConnectionString("MongoDbConnection"),
                        provider.GetRequiredService<IConfiguration>().GetConnectionString("MongoDbName")
                    )
            );

            services.BuildServiceProvider()
                .GetRequiredService<IQuidProQuoMongoDbContext>()
                .CreateIndexes();



            services.AddControllersWithViews();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });



            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllHeaders",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });

            services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();
            services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
            });



            services.AddSingleton<IAttachingRequestsQueue, AttachingRequestsQueue>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IMessagingService, MessagingService>();
            services.AddScoped<IExchangeOfServicesService, ExchangeOfServicesService>();
            services.AddScoped<IIoTService, IoTService>();



            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AuthOptions.Issuer,
                        ValidateAudience = true,
                        ValidAudience = AuthOptions.Audience,
                        ValidateLifetime = true,
                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            // If the request is for our hub...
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                (path.StartsWithSegments("/messenger") || path.StartsWithSegments("/IoT")))
                            {
                                // Read the token out of the query string
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });



            services.AddTransient<AttachingRequestDeleteJobFactory>();
            services.AddScoped<AttachingRequestDeleterJob>();

            services.AddQuartz(configurator =>
            {
                configurator.UseMicrosoftDependencyInjectionJobFactory();
                // base quartz scheduler, job and trigger configuration
            });

            // ASP.NET Core hosting
            services.AddQuartzServer(options =>
            {
                // when shutting down we want jobs to complete gracefully
                options.WaitForJobsToComplete = true;
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors("AllowAllHeaders");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<MessengerHub>("/messenger");
                endpoints.MapHub<IoTHub>("/IoT");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "api/{controller}/{action}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
