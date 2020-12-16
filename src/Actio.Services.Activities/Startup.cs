using Actio.Common.RabbitMq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Actio.Services.Activities.Handlers;
using Actio.Common.Commands;
using Actio.Common.Mongo;
using Actio.Services.Activities.Domain.Repositories;
using Actio.Services.Activities.Repositories;
using Actio.Services.Activities.Servises;

namespace Actio.Services.Activities
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
            //services.AddLogging();
            //services.AddMongoDB(Configuration);
            //services.AddScoped<ICategoryRepository, CategoryRepository>();
            //services.AddScoped<IActivityRepository, ActivityRepository>();
            //services.AddScoped<IDatabaseSeeder, CustomMongoSeeder>();
            //services.AddRabbitMq(Configuration);
            //services.AddSingleton<ICommandHandler<CreateActivity>, CreateActivityHandler>();
            services.AddControllers();

            services.AddLogging();

            services.AddMongoDB(Configuration);

            // Our custom configuration of RMQ.
            services.AddRabbitMq(Configuration);

            // Link handlers interfaces with handlers.
            services.AddSingleton<ICommandHandler<CreateActivity>, CreateActivityHandler>();
            services.AddSingleton<ICategoryRepository, CategoryRepository>();
            services.AddSingleton<IActivityRepository, ActivityRepository>();
            services.AddSingleton<IDatabaseSeeder, CustomMongoSeeder>();
            //services.AddSingleton<IActivityService, ActivityService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.ApplicationServices.GetService<IDatabaseInitializer>().InitializeAsync();

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
