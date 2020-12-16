using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Actio.Api.Handlers;
using Actio.Common.Events;
using Actio.Common.Mongo;
using Actio.Common.RabbitMq;
using Actio.Services.Activities.Domain.Repositories;
using Actio.Services.Activities.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Actio.Api
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
            //services.AddControllers();
            //services.AddRabbitMq(Configuration);
            //services.AddSingleton<IEventHandler<ActivityCreated>, ActivityCreatedHandler>();

            services.AddControllers();
            services.AddLogging();
            //services.AddJwt(Configuration);
            services.AddRabbitMq(Configuration);
            services.AddMongoDB(Configuration);
            services.AddSingleton<IEventHandler<ActivityCreated>, ActivityCreatedHandler>();
            //services.AddSingleton<IEventHandler<UserAuthenticated>, UserAuthenticatedHandler>();
            //services.AddSingleton<IEventHandler<UserCreated>, UserCreatedHandler>();
            services.AddSingleton<IActivityRepository, ActivityRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.ApplicationServices.GetService<IDatabaseInitializer>().InitializeAsync();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
