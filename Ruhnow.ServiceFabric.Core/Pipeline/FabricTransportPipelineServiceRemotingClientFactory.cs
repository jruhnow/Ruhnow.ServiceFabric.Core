using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Client;
using Microsoft.ServiceFabric.Services.Remoting.V2;
using Microsoft.ServiceFabric.Services.Remoting.V2.Client;
using System;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;

namespace Ruhnow.ServiceFabric.Core.Pipeline
{
    public class FabricTransportPipelineServiceRemotingClientFactory : IServiceRemotingClientFactory
    {
        public event EventHandler<CommunicationClientEventArgs<IServiceRemotingClient>> ClientConnected;
        public event EventHandler<CommunicationClientEventArgs<IServiceRemotingClient>> ClientDisconnected;

        private IServiceRemotingClientFactory innerFactory;
        public FabricTransportPipelineServiceRemotingClientFactory(IServiceRemotingClientFactory innerClientFactory)
        {
            this.innerFactory = innerClientFactory ?? throw new ArgumentNullException(nameof(innerClientFactory));
            this.innerFactory.ClientConnected += this.OnClientConnected;
            this.innerFactory.ClientDisconnected += this.OnClientDisconnected;
        }
        private void OnClientConnected(
           object sender,
           CommunicationClientEventArgs<IServiceRemotingClient> e)
        {
            ClientConnected?.Invoke(this, new CommunicationClientEventArgs<IServiceRemotingClient>()
            {
                Client = e.Client
            });
        }

        private void OnClientDisconnected(
            object sender,
            CommunicationClientEventArgs<IServiceRemotingClient> e)
        {
            ClientDisconnected?.Invoke(this, new CommunicationClientEventArgs<IServiceRemotingClient>()
            {
                Client = e.Client
            });
        }
        public async Task<IServiceRemotingClient> GetClientAsync(Uri serviceUri, ServicePartitionKey partitionKey, TargetReplicaSelector targetReplicaSelector, string listenerName, OperationRetrySettings retrySettings, CancellationToken cancellationToken)
        {
            IServiceRemotingClient client = await this.innerFactory.GetClientAsync(
                                                    serviceUri,
                                                    partitionKey,
                                                    targetReplicaSelector,
                                                    listenerName,
                                                    retrySettings,
                                                    cancellationToken);
            return new FabricTransportPipelineServiceRemotingClient(client);
        }

        public async Task<IServiceRemotingClient> GetClientAsync(ResolvedServicePartition previousRsp, TargetReplicaSelector targetReplicaSelector, string listenerName, OperationRetrySettings retrySettings, CancellationToken cancellationToken)
        {
            IServiceRemotingClient client = await this.innerFactory.GetClientAsync(
                                                    previousRsp,
                                                    targetReplicaSelector,
                                                    listenerName,
                                                    retrySettings,
                                                    cancellationToken);
            return new FabricTransportPipelineServiceRemotingClient(client);
        }

        public IServiceRemotingMessageBodyFactory GetRemotingMessageBodyFactory()
        {
            return this.innerFactory.GetRemotingMessageBodyFactory();
        }

        public Task<OperationRetryControl> ReportOperationExceptionAsync(IServiceRemotingClient client, ExceptionInformation exceptionInformation, OperationRetrySettings retrySettings, CancellationToken cancellationToken)
        {
            return this.innerFactory.ReportOperationExceptionAsync(
          // HACK: requirement that the inner client is the original remoting client for the inner factory - there seems to be some bleed out in their design here
          ((FabricTransportPipelineServiceRemotingClient)client).InnerClient,
          exceptionInformation,
          retrySettings,
          cancellationToken);
        }

      
    }
}
