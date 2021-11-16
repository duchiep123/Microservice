using Amazon;
using Amazon.DynamoDBv2;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Amazon.SQS;
using Amazon.SQS.Model;
using AWS.Service.SNS;
using AWS.Service.SNS.SNS.Helper;
using AWS.Service.SQS.SQS.Helper;
using CarsService.API.AWS.SQS.Service;
using CarsService.API.Repository;
using CarsService.API.Service;
using GarageManagementModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarsService.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public async void ConfigureServices(IServiceCollection services)
        {

            var connectionString = Configuration["ConnectionString"];
            Console.WriteLine(connectionString);
            var builder = new NpgsqlConnectionStringBuilder(connectionString);
            //var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            // folder migrations is important
            services.AddDbContext<GarageManagementContext>(options =>
                options.UseNpgsql(
                    // connectionString, x => x.MigrationsAssembly("GarageManagementModels")
                    connectionString
                )
            );
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CarsService.API", Version = "v1" });
            });
            var appSettingsSection = Configuration.GetSection("ServiceConfiguration");
            /*var awsCreds = new BasicAWSCredentials("hiep", "hiep");
            var config = new AmazonSQSConfig
            {
                ServiceURL = "http://localhost:4566",
                //RegionEndpoint = RegionEndpoint.USEast1
            };
            var amazonSqsClient = new AmazonSQSClient(awsCreds, config); // AmazonSQSClient implement IAmazonSQS
            var createQueueRequest = new CreateQueueRequest();
            var createQueueResponse = await amazonSqsClient.CreateQueueAsync(createQueueRequest);
            Console.WriteLine("QueueUrl : " + createQueueResponse.QueueUrl);*/

            //services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
            //services.AddAWSService<IAmazonSQS>();
            /*var res = Configuration.GetAWSOptions();
            Console.WriteLine(res.Credentials);
            Console.WriteLine(res.DefaultClientConfig.ServiceURL);
            Console.WriteLine(res.Profile);
            Console.WriteLine(res.Region);*/

            //If your SQS clients live with localstack in the same docker network, then it’s the container’s host name inside the docker network.In my docker - compose file it’s HOSTNAME_EXTERNAL = localstack.

            // If your SQS clients live on the docker host it’s HOSTNAME_EXTERNAL = localhost and you must expose your SQS port to the host.
            var awsOption = new AWSOptions();
            awsOption.Credentials = new BasicAWSCredentials("hiep", "hiep");
            awsOption.Region = RegionEndpoint.USEast1;
            awsOption.DefaultClientConfig.ServiceURL = "http://localstack:4566";

            services.AddAWSService<IAmazonSimpleNotificationService>(awsOption);
            services.AddAWSService<IAmazonSQS>(awsOption);
            services.AddAWSService<IAmazonDynamoDB>(awsOption);
            services.AddSingleton<IAmazonS3>(p => {
                var config = new AmazonS3Config
                {
                    RegionEndpoint = RegionEndpoint.USEast1,
                    ServiceURL= "http://localstack:4566",
                    ForcePathStyle = true
                };
                return new AmazonS3Client("hiep", "hiep", config);
            });
            //services.Configure<ServiceConfiguration>(appSettingsSection);
            services.AddScoped<IAWSSQSService, AWSSQSService>();
            services.AddScoped<ICarService, CarService>();
            services.AddScoped<ICarRepository, CarRepository>();
            services.AddScoped<ISQSHelper, SQSHelper>();
            services.AddScoped<ISNSHelper, SNSHelper>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CarsService.API v1"));
            }

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
