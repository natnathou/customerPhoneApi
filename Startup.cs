
using AutoMapper;
using customerPhoneApi.Data;
using customerPhoneApi.helpers;
using customerPhoneApi.services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace customerPhoneApi
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
            services.AddDbContext<DataContext>(x => x.UseNpgsql(Configuration.GetConnectionString("MyWebApiConnection")));
            services.AddAutoMapper(typeof(Startup));
            services.AddControllers();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
          .AddCookie();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseWhen(context => context.Request.Path.StartsWithSegments("/user"), appBuilder =>
                           {
                               appBuilder.UseMiddleware<JwtMiddleware>();

                           });
            app.UseEndpoints(endpoints =>
                        {
                            endpoints.MapControllers();
                        });


        }
    }
}
