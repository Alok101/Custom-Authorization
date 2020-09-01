using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Custom.Auth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Custom.Auth
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
            services.AddControllers();
            services.AddSingleton<ICustomAuthenticationManager, CustomAuthenticationManager>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminAndPowerUser", policy =>
                policy.RequireRole("Administrator","Poweruser"));

                options.AddPolicy("EmployeeWithMoreThan20Years", policy =>
                 policy.Requirements.Add(new EmployeeWithMoreYearsRequirement(20)));
            });
            services.AddSingleton<IAuthorizationHandler, EmployeeWithMoreYearsHandler>();
            services.AddSingleton<IEmployeeNumberOfYears, EmployeeNumberOfYears>();
            services.AddAuthentication("Basic").AddScheme<BasicAuthenticationOptions, CustomAuthenticationHandler>("Basic",null);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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
        }
    }
}
