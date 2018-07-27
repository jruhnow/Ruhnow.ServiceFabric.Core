using Microsoft.Extensions.Logging;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Communication.Wcf.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Ruhnow.ServiceFabric.Core;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Ruhnow.ServiceFabric.Server.Wcf
{
    public static class WcfListenerExtensions
    {
        public static ServiceInstanceListener CreateWcfListener<TStatelessService, TStatelessServiceImpl>(this StatelessService serviceImplementation, string endpointName, ILogger logger) where TStatelessServiceImpl : TStatelessService

        {
            return new ServiceInstanceListener(context => CreateWcfTcpCommunicationListener<TStatelessService, TStatelessServiceImpl>(context, endpointName, Activator.CreateInstance<TStatelessServiceImpl>(), logger));
        }
        public static ServiceInstanceListener CreateWcfListener<TStatelessService, TStatelessServiceImpl>(this StatelessService serviceImplementation, string endpointName) where TStatelessServiceImpl : TStatelessService

        {
            return new ServiceInstanceListener(context => CreateWcfTcpCommunicationListener<TStatelessService, TStatelessServiceImpl>(context, endpointName, Activator.CreateInstance<TStatelessServiceImpl>(), null));
        }
        public static ServiceInstanceListener CreateWcfListener<TStatelessService>(this TStatelessService serviceImplementation, string endpointName, ILogger logger)
        {
            return new ServiceInstanceListener(context => CreateWcfTcpCommunicationListener<TStatelessService, TStatelessService>(context, endpointName, serviceImplementation, logger));
        }

        public static ServiceInstanceListener CreateWcfListener<TStatelessService>(this TStatelessService serviceImplementation, string endpointName)
        {
            return new ServiceInstanceListener(context => CreateWcfTcpCommunicationListener<TStatelessService, TStatelessService>(context, endpointName, serviceImplementation, null));
        }
        private static ICommunicationListener CreateWcfTcpCommunicationListener<TStatelessService, TStatelessServiceImpl>(StatelessServiceContext context, string endpointName, TStatelessServiceImpl serviceImplementation, ILogger logger) where TStatelessServiceImpl : TStatelessService
        {
            ApplicationContext.Initialize(context);
            // TODO: Further parameterize
            string host = context.NodeContext.IPAddressOrFQDN;
            var endpointConfig = context.CodePackageActivationContext.GetEndpoint(endpointName);
            int port = endpointConfig.Port;
            var scheme = endpointConfig.UriScheme;
            var protocol = endpointConfig.Protocol;

            if (protocol != System.Fabric.Description.EndpointProtocol.Tcp)
            {
                // TODO: consider supporting http and https protocols along with certificates. 
                throw new NotSupportedException("Only WCF over TCP via NetTcpBinding is currently supported.");
            }

            NetTcpBinding binding;
            try
            {
                binding = new NetTcpBinding("EnterprisePlatform");

            }
            catch (KeyNotFoundException)
            {
                // no override entry was detected in the config - use defaults
                // Setting the security mode to Transport and credentialType to Basic'
                binding = new NetTcpBinding();
                binding.Security.Mode = SecurityMode.None;
                //   binding.Security.Mode = SecurityMode.Message;
                //  binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Windows;
            }

            binding.Namespace = $"{context.CodePackageActivationContext.ApplicationTypeName}";

            string componentName = Naming.Component<TStatelessService>();
            string uri = string.Format(CultureInfo.InvariantCulture, "{0}://{1}:{2}/{3}", scheme, host, port, componentName);

            var listener = new WcfCommunicationListener<TStatelessService>(
                serviceContext: context,
                wcfServiceObject: serviceImplementation,
                listenerBinding: binding,
                address: new EndpointAddress(uri)
            );

            // TODO: Add when servicemodel.core is available
          //  listener.ServiceHost.RunAsMicroservice();


            if (logger != null)
            {
             //   listener.ServiceHost.AddErrorHandler(ex => logger.LogError(ex, ex.Message));
            }

            return listener;
        }
    }
}
