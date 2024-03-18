using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Common.DbEventStore.Settings.MongoDb;
using Common.DbEventStore.Settings.Service;
using Microsoft.Extensions.Logging;

namespace Common.DbEventStore.MongoDB;

public static class Extensions
{
    public static IServiceCollection AddMongo(this IServiceCollection services)
    {
        BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
        BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

        services.AddSingleton(serviceProvider =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var serviceSettings = configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>();
            var mongoDbSettings = configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
            if (mongoDbSettings is null)
            {
                throw new ArgumentNullException(nameof(mongoDbSettings), "MongoDbSettings is not found in appsettings.json");
            }
            if (mongoDbSettings.ConnectionString is null)
            {
                throw new ArgumentNullException(nameof(mongoDbSettings.ConnectionString), "ConnectionString is not found in appsettings.json");
            }
            if (serviceSettings is null)
            {
                throw new ArgumentNullException(nameof(serviceSettings), "ServiceSettings is not found in appsettings.json");
            }
            if (serviceSettings.ServiceName is null)
            {
                throw new ArgumentNullException(nameof(serviceSettings.ServiceName), "ServiceName is not found in appsettings.json");
            }
            if (serviceSettings.ItemsCollectionName is null)
            {
                throw new ArgumentNullException(nameof(serviceSettings.ItemsCollectionName), "ItemsCollectionName is not found in appsettings.json");
            }
            var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);

            return mongoClient.GetDatabase(serviceSettings.ServiceName);
        });

        return services;
    }

    public static IServiceCollection AddMongoRepository<T>(this IServiceCollection services, string collectionName) where T : IEntity
    {
        services.AddSingleton<IRepository<T>>(serviceProvider =>
        {
            var database = serviceProvider.GetRequiredService<IMongoDatabase>();
            var logger = serviceProvider.GetRequiredService<ILogger<MongoRepository<T>>>();
            return new MongoRepository<T>(database, collectionName, logger);
        });

        return services;
    }
}
