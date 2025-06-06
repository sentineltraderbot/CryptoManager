using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using CryptoManager.Business;
using CryptoManager.Domain.Mapper;
using CryptoManager.Integration;
using CryptoManager.Repository;
using CryptoManager.Repository.DatabaseContext;
using CryptoManager.WebApi.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;

namespace CryptoManager.WebApi.Test.Mocks
{
    public class MockStartup<TStartup> : IDisposable
    {
        private const string SOLUTION_NAME = "CryptoManager.Server.sln";
        private readonly TestServer _server;
        public static MockStartup<TStartup> Instance { get; private set; }

        static MockStartup()
        {
            Instance = new MockStartup<TStartup>();
        }

        public IConfiguration _configuration { get; }

        private MockStartup()
        {
            var startupAssembly = typeof(TStartup).GetTypeInfo().Assembly;
            var contentRoot = GetProjectPath(string.Empty, startupAssembly);

            var configbuilder = new ConfigurationBuilder()
                .SetBasePath(contentRoot)
                .AddEnvironmentVariables();
            configbuilder.AddUserSecrets<MockStartup<Startup>>();
            _configuration = configbuilder.Build();

            var builder = new WebHostBuilder()
                .UseContentRoot(contentRoot)
                .ConfigureServices(ConfigureServices)
                .UseEnvironment("Development")
                .UseStartup(typeof(TStartup))
                .Configure(Configure);

            _server = new TestServer(builder);
        }

        public void Dispose()
        {
            _server.Dispose();
        }

        public HttpClient GetCliente()
        {
            return _server.CreateClient();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            WebUtil.JwtKeyName = TestWebUtil.JwtKeyName = _configuration["JwtKeyName"];
            WebUtil.FacebookAppId = TestWebUtil.FacebookAppId = _configuration["Authentication:Facebook:AppId"];
            WebUtil.FacebookAppSecret = TestWebUtil.FacebookAppSecret = _configuration["Authentication:Facebook:AppSecret"];
            TestWebUtil.FacebookAccessToken = _configuration["Authentication:Facebook:AccessToken"];
            WebUtil.SuperUserEmail = TestWebUtil.SuperUserEmail = _configuration["Authentication:SuperUserEmail"];

            services.AddDbContextPool<ApplicationIdentityDbContext>(options =>
            {
                options.UseInMemoryDatabase("DBINTEGRATIONTEST")
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });

            services.AddDbContextPool<ApplicationIdentityDbContext>(options =>
            {
                options.UseInMemoryDatabase("DBINTEGRATIONTEST")
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });

            services.AddORM();
            services.AddRepositories();
            services.AddBusiness();
            services.AddIntegrations();

            services.AddAuthentication().AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = WebUtil.FacebookAppId;
                facebookOptions.AppSecret = WebUtil.FacebookAppSecret;
            });

            services.AddSingleton<JwtFactory>();

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
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    //ValidAudience = "the audience you want to validate",
                    ValidateIssuer = false,
                    //ValidIssuer = "the isser you want to validate",

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(WebUtil.JwtKeyName)),

                    ValidateLifetime = true, //validate the expiration and not before values in the token

                    ClockSkew = TimeSpan.FromMinutes(5) //5 minute tolerance for the expiration date
                };
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

            services.AddDistributedMemoryCache();
        }


        public void Configure(IApplicationBuilder app)
        {
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
            });

            app.EnsureCreateDatabase();
            app.AddRole(WebUtil.ADMINISTRATOR_ROLE_NAME).Wait();
        }
        /// <summary>
        /// Gets the full path to the target project path that we wish to test
        /// </summary>
        /// <param name="solutionRelativePath">
        /// The parent directory of the target project.
        /// e.g. src, samples, test, or test/Websites
        /// </param>
        /// <param name="startupAssembly">The target project's assembly.</param>
        /// <returns>The full path to the target project.</returns>
        private static string GetProjectPath(string solutionRelativePath, Assembly startupAssembly)
        {
            // Get name of the target project which we want to test
            var projectName = startupAssembly.GetName().Name;

            // Get currently executing test project path
            var applicationBasePath = PlatformServices.Default.Application.ApplicationBasePath;

            // Find the folder which contains the solution file. We then use this information to find the target
            // project which we want to test.
            var directoryInfo = new DirectoryInfo(applicationBasePath);
            do
            {
                var solutionFileInfo = new FileInfo(Path.Combine(directoryInfo.FullName, SOLUTION_NAME));
                if (solutionFileInfo.Exists)
                {
                    return Path.GetFullPath(Path.Combine(directoryInfo.FullName, solutionRelativePath, projectName));
                }

                directoryInfo = directoryInfo.Parent;
            }
            while (directoryInfo.Parent != null);

            throw new Exception($"Solution root could not be located using application root {applicationBasePath}.");
        }
    }
}
