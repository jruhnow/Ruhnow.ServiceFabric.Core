using Microsoft.ServiceFabric.Services.Remoting.V2;
using Microsoft.ServiceFabric.Services.Remoting.V2.Client;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Text;
using System.Threading.Tasks;

namespace Ruhnow.ServiceFabric.Core.Pipeline
{
    public class FabricTransportPipelineServiceRemotingClient : IServiceRemotingClient
    {
        public FabricTransportPipelineServiceRemotingClient(IServiceRemotingClient innerClient)
        {
            this.InnerClient = innerClient ?? throw new ArgumentNullException(nameof(innerClient));
        }

        ~FabricTransportPipelineServiceRemotingClient()
        {
            var disposable = this.InnerClient as IDisposable;
            disposable?.Dispose();
        }

        internal IServiceRemotingClient InnerClient { get; }

        public string ListenerName
        {
            get
            {
                return this.InnerClient.ListenerName;
            }
            set
            {
                this.InnerClient.ListenerName = value;
            }
        }

        public ResolvedServiceEndpoint Endpoint
        {
            get
            {
                return this.InnerClient.Endpoint;
            }
            set
            {
                this.InnerClient.Endpoint = value;
            }
        }

        public ResolvedServicePartition ResolvedServicePartition
        {
            get
            {
                return this.InnerClient.ResolvedServicePartition;
            }
            set
            {
                this.InnerClient.ResolvedServicePartition = value;
            }
        }

        public async Task<IServiceRemotingResponseMessage> RequestResponseAsync(IServiceRemotingRequestMessage requestMessage)
        {
            MessagePipeline.Execute(requestMessage);
            IServiceRemotingResponseMessage result = await this.InnerClient.RequestResponseAsync(requestMessage);
            MessagePipeline.Execute(result);

            return result;
        }

        public void SendOneWay(IServiceRemotingRequestMessage requestMessage)
        {
            this.InnerClient.SendOneWay(requestMessage);
            MessagePipeline.Execute(requestMessage);
        }
    }
}
