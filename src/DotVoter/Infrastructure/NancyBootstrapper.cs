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
using StructureMap;

namespace DotVoter.Infrastructure
{
    public class NancyBootstrapper : StructureMapNancyBootstrapper
        {
            protected override void ConfigureApplicationContainer(StructureMap.IContainer existingContainer)
            {
                StructureMapContainer.Configure(existingContainer);
            }

            protected override void ApplicationStartup(StructureMap.IContainer container, Nancy.Bootstrapper.IPipelines pipelines)
            {
                RegisterMongoMappings(container);

                pipelines.OnError += (context, exception) =>
                {
                    if (exception is EventNotFoundExeption)
                        return new Response
                        {
                            StatusCode = HttpStatusCode.NotFound,
                            ContentType = "text/html",
                            Contents = (stream) =>
                            {
                                var errorMessage =
                                    Encoding.UTF8.GetBytes(
                                        exception.Message);
                                stream.Write(errorMessage, 0,
                                             errorMessage.Length);
                            }
                        };

                    return HttpStatusCode.InternalServerError;
                };
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
                    a.SetIdMember(a.GetMemberMap(x => x.Id));//.SetIdGenerator(new IntIdGenerator()));
                });
            }

        }
        }
}