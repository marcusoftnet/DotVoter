using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using DotVoter.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Options;
using Nancy;
using Nancy.Bootstrappers.StructureMap;
using Nancy.Session;
using StructureMap;
using Nancy.Diagnostics;

namespace DotVoter.Infrastructure
{
    public class NancyBootstrapper : StructureMapNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(StructureMap.IContainer existingContainer)
        {
            StructureMapContainer.Configure(existingContainer);
        }

        protected override DiagnosticsConfiguration DiagnosticsConfiguration
        {
            get { return new DiagnosticsConfiguration { Password = @"D0tV0t3r" }; }
        }

        protected override void ApplicationStartup(StructureMap.IContainer container, Nancy.Bootstrapper.IPipelines pipelines)
        {
            RegisterMongoMappings(container);

            CookieBasedSessions.Enable(pipelines);

            StaticConfiguration.EnableRequestTracing = true;
        }

        public static void RegisterMongoMappings(IContainer container)
        {
            DateTimeSerializationOptions.Defaults = new DateTimeSerializationOptions(DateTimeKind.Local);


            BsonSerializer.RegisterIdGenerator(typeof(int), container.GetInstance<IIdentityGenerator>() as IIdGenerator);


            if (!BsonClassMap.IsClassMapRegistered(typeof(Vote)))
            {
                BsonClassMap.RegisterClassMap<Vote>(a => a.AutoMap());
            }

            if (!BsonClassMap.IsClassMapRegistered(typeof(Topic)))
            {
                BsonClassMap.RegisterClassMap<Topic>(a =>
                {
                    a.AutoMap();
                    a.MapIdMember(c => c.Id).SetIsRequired(true);
                });
            }
            if (!BsonClassMap.IsClassMapRegistered(typeof(WorkShopEvent)))
            {
                BsonClassMap.RegisterClassMap<WorkShopEvent>(a =>
                {
                    a.AutoMap();
                    a.SetIdMember(a.GetMemberMap(x => x.Id));
                });
            }

        }
    }
}