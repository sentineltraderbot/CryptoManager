using System;
using System.Text;
using CryptoManager.Business;
using CryptoManager.Domain.Mapper;
using CryptoManager.Integration;
using CryptoManager.Repository;
using CryptoManager.WebApi.HealthCheck;
using CryptoManager.WebApi.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace CryptoManager.WebApi
{
    public class Startup
    {
        public IConfiguration _configuration { get; }
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }
            _configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services, IWebHostEnvironment env)
        {
            IdentityModelEventSource.ShowPII = env.IsDevelopment();
            //using secret manager to develop with real code
            WebUtil.JwtKeyName = _configuration["JwtKeyName"];
            WebUtil.FacebookAppId = _configuration["Authentication:Facebook:AppId"];
            WebUtil.FacebookAppSecret = _configuration["Authentication:Facebook:AppSecret"];
            WebUtil.SuperUserEmail = _configuration["Authentication:SuperUserEmail"];
            WebUtil.GoogleAppId = _configuration["Authentication:Google:AppId"];
            WebUtil.GoogleAppSecret = _configuration["Authentication:Google:AppSecret"];

            if (_configuration["DatabaseProvider"] == "SQLite")
            {
                services.AddSQLiteDbContexts(_configuration.GetConnectionString("DefaultConnection"));
            }
            else
            {
                services.AddSQLServerDbContexts(_configuration.GetConnectionString("DefaultConnection"));
            }

            services.AddORM();
            services.AddRepositories();
            services.AddBusiness();
            services.AddIntegrations();
            services.AddHealthChecks()
                .AddCheck<ExchangeIntegrationHealthCheck>("ExchangeIntegrationPing");

            services.AddSingleton<JwtFactory>();
            services.AddSingleton(_configuration);

            services.AddCors();

            services.AddControllers(options =>
            {
                options.Filters.Add(new AuthorizeFilter("Bearer"));
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = _configuration["IdentityUrl"];
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    //ValidAudience = "the audience you want to validate",
                    ValidateIssuer = false,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(WebUtil.JwtKeyName)),

                    ValidateLifetime = true, //validate the expiration and not before values in the token

                    ClockSkew = TimeSpan.FromMinutes(5), //5 minute tolerance for the expiration date
                };
            }).AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = WebUtil.FacebookAppId;
                facebookOptions.AppSecret = WebUtil.FacebookAppSecret;
            }).AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = WebUtil.GoogleAppId;;
                googleOptions.ClientSecret = WebUtil.GoogleAppSecret;;
            });

            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfileConfiguration());
            });
            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                    .RequireAuthenticatedUser().Build());
            });

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(setup =>
            {
                setup.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "CryptoManager API",
                    Version = "v1",
                    Description = "API to Manage Crypto Currencies using Exchanges APIs",
                    Contact = new OpenApiContact
                    {
                        Name = "Rômulo Nissóla Rocha",
                        Email = "romulonissola@gmail.com",
                        Url = new Uri("https://github.com/romulonissola/CryptoManager")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "GNU General Public License v2.0",
                        Url = new Uri("https://www.gnu.org/licenses/old-licenses/gpl-2.0.html")
                    }
                });
            });

            services.AddDistributedMemoryCache();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            });

            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CryptoManager API V1");
            });

            //app.EnsureCreateDatabase();
            app.AddRole(WebUtil.ADMINISTRATOR_ROLE_NAME).Wait();
        }
    }
}
