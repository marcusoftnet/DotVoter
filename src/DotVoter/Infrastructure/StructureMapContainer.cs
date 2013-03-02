﻿using StructureMap;

namespace DotVoter.Infrastructure
{
    public static class StructureMapContainer
    {
        public static void Configure(IContainer container)
        {
            container.Configure(config => config.Scan(c =>
                {
                    c.TheCallingAssembly();
                    c.WithDefaultConventions();
                }));
        }
    }
}