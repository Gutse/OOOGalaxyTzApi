using System;
using System.Globalization;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NHibernate;
using NHibernate.Caches.CoreMemoryCache;
using NHibernate.Context;
using NHibernate.Dialect;
using NHibernate.Tool.hbm2ddl;
using OOOGalaxyTzApi.Config;
using OOOGalaxyTzApi.Models;
using Environment = NHibernate.Cfg.Environment;
using ISession = NHibernate.ISession;

namespace OOOGalaxyTzApi.DAL
{
    public class AppSessionFactory
    {
        private readonly ISessionFactory _factory;
        private readonly DatabaseConfig _config;
        private readonly IHttpContextAccessor _accessor;

        public AppSessionFactory(IOptions<DatabaseConfig> options, IHttpContextAccessor accessor, IHostEnvironment environment)
        {
            _config = options.Value;
            _accessor = accessor;
            _factory = CreateApiSessionFactory();
        }

        private ISessionFactory CreateApiSessionFactory()
        {
            var config = GetConfiguration(_config.SqlServer);
            return BuildSessionFactory(config);
        }

        private ISessionFactory BuildSessionFactory(FluentConfiguration config)
        {
            var cfg = config
                     .Mappings(m => m.FluentMappings.AddFromAssemblyOf<BaseModel>())
                     .ExposeConfiguration(c => new SchemaUpdate(c).Execute(false, true))
                     .BuildConfiguration();
            return cfg.BuildSessionFactory();
        }

        private FluentConfiguration GetConfiguration(string connectionKey)
        {
            var cfg = Fluently.Configure()
                              .Database(MsSqlConfiguration
                                       .MsSql2012
                                       .ConnectionString(e => e.Is(connectionKey))
                                       .Dialect<MsSql2012Dialect>()
                                       .UseReflectionOptimizer()
                                       .Raw(Environment.CommandTimeout, TimeSpan.FromMinutes(5)
                                                                                .TotalSeconds.ToString(CultureInfo.InvariantCulture))
                                       .AdoNetBatchSize(100))
                              .Cache(c => c
                                         .UseQueryCache()
                                         .UseSecondLevelCache()
                                         .ProviderClass<CoreMemoryCacheProvider>())
                              .CurrentSessionContext<AsyncLocalSessionContext>();

            if (_config.ShowSQL)
            {
                cfg.Database(MySQLConfiguration
                            .Standard
                            .FormatSql()
                            .ShowSql()
                            .Raw(Environment.GenerateStatistics, "true"));
            }

            return cfg;
        }

        public ISession OpenSession()
        {
            var session = _factory.WithOptions().OpenSession();
            session.BeginTransaction();
            return session;
        }
    }
}