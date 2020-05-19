
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cw10.DAL;
using Cw10.Middlewares;
using Cw10.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Cw10.Tokens;
using Microsoft.EntityFrameworkCore;

namespace CW
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

            services.AddDbContext<StudentDBContext>(options =>
            {
                options.UseSqlServer("Data Source=db-mssql;Initial Catalog=s17064;Integrated Security=True");
            });
            services.AddControllers();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {

                            ValidateIssuer = true,
                            ValidIssuer = MyToken.ISSUER,
                            ValidateAudience = true,
                            ValidAudience = MyToken.AUDIENCE,
                            ValidateLifetime = true,

                            IssuerSigningKey = MyToken.GetSymmetricSecurityKey(),
                            ValidateIssuerSigningKey = true,
                        };
                    });
            services.AddControllersWithViews();
 

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseDeveloperExceptionPage();

            app.UseDefaultFiles();
            app.UseStaticFiles();

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