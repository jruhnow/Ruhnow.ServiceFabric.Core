using Microsoft.Extensions.Configuration;
using System.Fabric;

namespace Ruhnow.ServiceFabric.Server.Configuration
{
    public static class ServiceFabricConfigurationExtensions
    {
        public static IConfigurationBuilder AddServiceFabricConfiguration(this IConfigurationBuilder builder, ServiceContext serviceContext)
        {
            builder.Add(new ServiceFabricConfigurationSource(serviceContext));
            return builder;
        }

        public static IConfigurationRoot Configuration(this ServiceContext context)
        {
            if (configuration == null)
            {
                configuration = new ConfigurationBuilder().AddServiceFabricConfiguration(context).Build();
            }
            return configuration;
        }

        private static IConfigurationRoot configuration = null;
    }
}
