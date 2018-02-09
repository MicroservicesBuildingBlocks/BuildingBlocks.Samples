using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Raven.Client.Documents;
using Raven.Client.Documents.Conventions;
using Raven.Client.Documents.Indexes;

namespace Identity.Application
{
    public static class DocumentStoreHolder
    {
        private static bool _initialized;
        public static string RavenDbServerAddress;
        public static string DatabaseName;
        public static IEnumerable<Assembly> IndexSources;
        public static IEnumerable<CollectionMapping> Mappings;

        public static void Initialize(IConfiguration configuration, params CollectionMapping[] mappings)
        {
            Initialize(configuration, new Assembly[0], mappings);
        }

        public static void Initialize(IConfiguration configuration, Assembly[] indexSources, params CollectionMapping[] mappings)
        {
            Initialize(
                configuration["RavenDbUrl"],
                configuration["RavenDbDatabase"],
                indexSources,
                mappings
            );
        }

        public static void Initialize(
            string ravenDbServerAddress,
            string databaseName,
            params CollectionMapping[] mappings
        )
        {
            Initialize(ravenDbServerAddress, databaseName, new Assembly[0], mappings);
        }

        public static void Initialize(
            string ravenDbServerAddress,
            string databaseName,
            Assembly[] indexSources,
            params CollectionMapping[] mappings
        )
        {
            if (_initialized)
                throw new InvalidOperationException("DocumentStoreHolder already initialized.");

            RavenDbServerAddress = ravenDbServerAddress;
            DatabaseName = databaseName;
            Mappings = mappings;
            IndexSources = indexSources;

            _initialized = true;
        }

        private static readonly Lazy<IDocumentStore> LazyStore =
            new Lazy<IDocumentStore>(() =>
            {
                if (!_initialized)
                    throw new InvalidOperationException("DocumentStoreHolder was not initialized.");

                var store = new DocumentStore
                {
                    Urls = new[] { RavenDbServerAddress },
                    Database = DatabaseName
                };

                var serializer = store.Conventions.CreateSerializer();
                serializer.TypeNameHandling = TypeNameHandling.All;

                store.Conventions.FindCollectionName = (type) =>
                {
                    if (Mappings != null)
                    {
                        foreach (var mapping in Mappings)
                        {
                            if (mapping.Type.IsAssignableFrom(type))
                                return mapping.CollectionName;
                        }
                    }

                    return DocumentConventions.DefaultGetCollectionName(type);
                };

                store.Initialize();

                foreach (var indexSource in IndexSources)
                {
                    IndexCreation.CreateIndexes(indexSource, store);
                }

                return store;
            });

        public static IDocumentStore Store =>
            LazyStore.Value;

        public static IServiceCollection AddDocumentStore(
            this IServiceCollection services,
            IConfiguration configuration,
            params CollectionMapping[] mappings
        )
        {
            return AddDocumentStore(services, configuration, new Assembly[0], mappings);
        }

        public static IServiceCollection AddDocumentStore(
            this IServiceCollection services,
            IConfiguration configuration,
            Assembly[] indexSources,
            params CollectionMapping[] mappings
        )
        {
            Initialize(configuration, indexSources, mappings);
            //Initialize(configuration, mappings);
            services.AddSingleton(Store);
            return services;
        }
    }

    public struct CollectionMapping
    {
        public Type Type { get; }
        public string CollectionName { get; }

        public CollectionMapping(Type type, string collectionName)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));

            if (string.IsNullOrEmpty(collectionName))
            {
                throw new ArgumentException("Collection Name could not be null nor empty", nameof(collectionName));
            }
            CollectionName = collectionName;
        }
    }

}
