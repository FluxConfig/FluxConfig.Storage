using FluxConfig.Storage.Infrastructure.Configuration.Models;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace FluxConfig.Storage.Infrastructure.Dal.Infrastructure;

public static class Mongo
{
    public static void AddMongoDbClient(IServiceCollection services, MongoDbOptions mongoOptions)
    {
        services.AddSingleton<IMongoClient>(new MongoClient(mongoOptions.ConnectionString));
    }

    //TODO: Add migration runner
    public static void AddMigrations(IServiceCollection services)
    {
        
    }
}