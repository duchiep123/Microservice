using GarageManagementModels;
using GaragesService.API.Repository;
using GaragesService.API.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Hangfire;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire.Dashboard;
using GaragesService.API.Tasks;
using AWS.Service.SQS.SQS.Helper;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon;
using Amazon.SimpleNotificationService;
using Amazon.SQS;
using System.Threading;

namespace GaragesService.API
{
    public class Startup
    {
        public static StackExchange.Redis.ConnectionMultiplexer Redis;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            Redis = StackExchange.Redis.ConnectionMultiplexer.Connect("redis:6379");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration["ConnectionString"];
            var builder = new NpgsqlConnectionStringBuilder(connectionString);
            //var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            // folder migrations is important
            services.AddDbContext<GarageManagementContext>(options =>
                options.UseNpgsql(
                    // connectionString, x => x.MigrationsAssembly("GarageManagementModels")
                    connectionString
                )
            );
            
            services.AddHangfire(configuration =>
            {
                configuration.UseRedisStorage(Redis);

            });
            services.AddStackExchangeRedisCache(o => o.Configuration = "redis:6379"); // config DI redis, add hangfire.aspcore package
            services.AddHangfireServer(action => action.SchedulePollingInterval = TimeSpan.FromMilliseconds(1000));//  set thoi gian nho nhat
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GaragesService.API", Version = "v1" });
            });
            services.AddScoped<IGarageService, GarageService>();
            services.AddScoped<IGarageRepository, GarageRepository>();
            var awsSqsOption = new AWSOptions();
            awsSqsOption.Credentials = new BasicAWSCredentials("hiep", "hiep");
            awsSqsOption.Region = RegionEndpoint.USEast1;
            awsSqsOption.DefaultClientConfig.ServiceURL = "http://localstack:4566";
            services.AddAWSService<IAmazonSimpleNotificationService>(awsSqsOption);
            services.AddAWSService<IAmazonSQS>(awsSqsOption);
            services.AddSingleton<ISQSHelper, SQSHelper>();
            services.AddScoped<TasksService>();
            //services.AddHostedService<QueueReaderService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GaragesService.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                IsReadOnlyFunc = (DashboardContext context) => true
            });
            //BackgroundJob.Enqueue<TasksService>(t => t.RunProcessMessages("Hiep"));
             //RecurringJob.AddOrUpdate<TasksService>(t =>t.ReadQueue("http://localhost:4566/000000000000/garage-queue"), "*/20 * * * * * "); // */20 * * * * * 
        }
    }
}
