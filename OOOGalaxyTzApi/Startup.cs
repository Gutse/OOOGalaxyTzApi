using System;
using System.Globalization;
using System.Reflection;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OOOGalaxyTzApi.Auth;
using OOOGalaxyTzApi.Config;
using OOOGalaxyTzApi.DAL;
using OOOGalaxyTzApi.Helpers;
using OOOGalaxyTzApi.Interfaces;
using OOOGalaxyTzApi.Middlewares;
using OOOGalaxyTzApi.Swagger;
using OOOGalaxyTzApiRepository;

namespace OOOGalaxyTzApi
{
    public class Startup
    {
        private IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddAutoMapper(Assembly.GetEntryAssembly());
            services.AddHttpContextAccessor();
            services.AddSingleton<IContentTypeProvider, FileExtensionContentTypeProvider>();
            services.AddSingleton<AppSessionFactory>();
            services.AddScoped(x => x.GetRequiredService<AppSessionFactory>()
                                     .OpenSession());
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IPasswordHelper), typeof(PasswordHelper));

            services.AddAutoMapper(Assembly.GetEntryAssembly());

            services.AddResponseCompression();

            services.AddCors();

            services.AddRouting();

            services.AddControllers()
                    .AddControllersAsServices()
                    .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                        options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                        options.SerializerSettings.DateFormatString = "dd.MM.yyyy HH:mm:ss";
                        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                        options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
                        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                        options.SerializerSettings.Culture = CultureInfo.CurrentCulture;
                        options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                        options.SerializerSettings.Formatting = Formatting.Indented;
                     });



            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                     {
                         options.RequireHttpsMetadata = false;
                         options.TokenValidationParameters = new TokenValidationParameters
                         {
                             ValidateIssuer = true,
                             ValidIssuer = AuthOptions.Issuer,
                             ValidateAudience = false,
                             ValidAudience = AuthOptions.Audience,
                             ValidateLifetime = false,
                             IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                             ValidateIssuerSigningKey = true,
                             ClockSkew = TimeSpan.Zero
                         };
                     });

            services.AddSwaggerDocumentation();

            services.Configure<DatabaseConfig>(Configuration.GetSection("SQLConfig"));

            services.AddMemoryCache();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<CloseSessionMiddleware>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwaggerDocumentation();
        }
    }
}
