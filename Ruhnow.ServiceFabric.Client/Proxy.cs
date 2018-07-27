using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Client;
using Microsoft.ServiceFabric.Services.Runtime;
using Ruhnow.ServiceFabric.Core;
using Ruhnow.ServiceFabric.Core.Pipeline;
using Ruhnow.ServiceFabric.Core.Serialization;
using System;
using System.Diagnostics;

namespace Ruhnow.ServiceFabric.Client
{
    public class Proxy
    {
        private static ServiceProxyFactory defaultProxyFactory = new ServiceProxyFactory(
            handler => new FabricTransportPipelineServiceRemotingClientFactory(
                new FabricTransportServiceRemotingClientFactory(serializationProvider: new ManagedDataContractSerializationProvider())));
        private static I ForService<I>(Uri serviceAddress, ServicePartitionKey partitionKey = null) where I : class, IService
        {
            Debug.Assert(typeof(I).IsInterface);
            I proxy = defaultProxyFactory.CreateNonIServiceProxy<I>(serviceAddress, partitionKey: partitionKey, listenerName: Naming.Listener<I>());
            return proxy;
        }

        internal static I ForService<I>(ServiceProxyFactory serviceProxyFactory, Uri serviceAddress, ServicePartitionKey partitionKey = null) where I : class, IService
        {
            if (serviceProxyFactory == null)
            {
                throw new ArgumentNullException(nameof(serviceProxyFactory), "Invalid node call. Must supply service proxy factory.");
            }
            Debug.Assert(typeof(I).IsInterface);
            return serviceProxyFactory.CreateNonIServiceProxy<I>(serviceAddress, partitionKey: partitionKey, listenerName: Naming.Listener<I>());
        }

        public static I ForMicroservice<I>(ServicePartitionKey partitionKey = null) where I : class, IService
        {
            Debug.Assert(
                typeof(I).Namespace.Contains(Constant.Manager),
                $"Invalid microservice call. Use only the {Constant.Manager} interface to access a microservice.");
            return ForService<I>(defaultProxyFactory, Addressing.Microservice<I>(), partitionKey);
        }

        public static I ForMicroservice<I>(ServiceProxyFactory serviceProxyFactory, ServicePartitionKey partitionKey = null) where I : class, IService
        {
            if (serviceProxyFactory == null)
            {
                throw new ArgumentNullException(nameof(serviceProxyFactory), "Invalid microservice call. Must supply service proxy factory.");
            }
            Debug.Assert(
                typeof(I).Namespace.Contains(Constant.Manager),
                $"Invalid microservice call. Use only the {Constant.Manager} interface to access a microservice.");
            return ForService<I>(serviceProxyFactory, Addressing.Microservice<I>(), partitionKey);
        }

        public static I ForComponent<I>(StatelessService caller, ServicePartitionKey partitionKey = null) where I : class, IService
        {
            Debug.Assert(caller != null, "Invalid component call. Must supply stateless service caller.");
            return ForService<I>(defaultProxyFactory, Addressing.Component<I>(caller), partitionKey);
        }

        public static I ForComponent<I>(ServiceProxyFactory serviceProxyFactory, StatelessService caller, ServicePartitionKey partitionKey = null) where I : class, IService
        {
            if (serviceProxyFactory == null)
            {
                throw new ArgumentNullException(nameof(serviceProxyFactory), "Invalid component call. Must supply service proxy factory.");
            }
            Debug.Assert(caller != null, "Invalid component call. Must supply stateless service caller.");
            return ForService<I>(serviceProxyFactory, Addressing.Component<I>(caller), partitionKey);
        }

        public static I ForComponent<I>(StatefulService caller, ServicePartitionKey partitionKey = null) where I : class, IService
        {
            Debug.Assert(caller != null, "Invalid component call. Must supply stateful service caller.");
            return ForService<I>(defaultProxyFactory, Addressing.Component<I>(caller), partitionKey);
        }

        public static I ForComponent<I>(ServiceProxyFactory serviceProxyFactory, StatefulService caller, ServicePartitionKey partitionKey = null) where I : class, IService
        {
            if (serviceProxyFactory == null)
            {
                throw new ArgumentNullException(nameof(serviceProxyFactory), "Invalid component call. Must supply service proxy factory.");
            }
            Debug.Assert(caller != null, "Invalid component call. Must supply stateful service caller.");
            return ForService<I>(serviceProxyFactory, Addressing.Component<I>(caller), partitionKey);
        }
    }
}
