using Microsoft.Azure.Functions.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(SFSServices.Service1.Startup))]

namespace SFSServices.Service1
{
    using Microsoft.Extensions.DependencyInjection;
    using MongoDB.Driver;
    using SFSServices.S1.Repositories;
    using SFSServices.S2;
    using System;
    using System.Net.Http;

    public class Startup : FunctionsStartup
    {
        public static void ConfigureService1(IFunctionsHostBuilder builder)
        {
            //I chose to do the MongoAPI for cosmos becuase I really like the simplicity of the nosql and linq drivers for an app that's so simple.
            builder.Services.AddSingleton<MongoClient>(c =>
            {
                string connString = Environment.GetEnvironmentVariable("dbConnectionString");
                Console.WriteLine("DI: Creating MongoClient with connection string" + connString);
                return new MongoClient(connString);
            });

            builder.Services.AddSingleton<IMongoDatabase>(c =>
            {
                string dbName = Environment.GetEnvironmentVariable("dbName");
                Console.WriteLine("DI: Adding MongoDatabase with name " + dbName);
                return c.GetService<MongoClient>().GetDatabase(dbName);
            });

            builder.Services.AddSingleton<ICreditRepository, CreditRepositoryCosmosDB>();
        }

        private static void ConfigureService2(IFunctionsHostBuilder builder)
        {
            builder.Services.AddSingleton<HttpClient>(c =>
            {
                return new HttpClient
                {
                    BaseAddress = new Uri(Environment.GetEnvironmentVariable("httpBaseAddress"))
                };
            });

            builder.Services.AddScoped<IDTIService, DTIService>();
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            ConfigureService1(builder);
            ConfigureService2(builder);
        }
    }
}
