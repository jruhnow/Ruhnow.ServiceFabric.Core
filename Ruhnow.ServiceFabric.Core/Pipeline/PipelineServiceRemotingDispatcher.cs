using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.V2;
using Microsoft.ServiceFabric.Services.Remoting.V2.Runtime;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;

namespace Ruhnow.ServiceFabric.Core.Pipeline
{
    public class PipelineServiceRemotingDispatcher
           : ServiceRemotingMessageDispatcher
    {
        public PipelineServiceRemotingDispatcher(ServiceContext serviceContext, IService service) :
            base(serviceContext, service)
        {
        }

        public async override Task<IServiceRemotingResponseMessage> HandleRequestResponseAsync(
            IServiceRemotingRequestContext requestContext,
            IServiceRemotingRequestMessage requestMessage)
        {
            MessagePipeline.Execute(requestMessage);
            IServiceRemotingResponseMessage responseMessage = await base.HandleRequestResponseAsync(requestContext, requestMessage);
            MessagePipeline.Execute(responseMessage);
            return responseMessage;
        }

        public override Task<IServiceRemotingResponseMessageBody> HandleRequestResponseAsync(
            ServiceRemotingDispatchHeaders requestMessageDispatchHeaders,
            IServiceRemotingRequestMessageBody requestMessageBody,
            CancellationToken cancellationToken)
        {
            return base.HandleRequestResponseAsync(requestMessageDispatchHeaders, requestMessageBody, cancellationToken);
        }

        public override void HandleOneWayMessage(IServiceRemotingRequestMessage requestMessage)
        {
            MessagePipeline.Execute(requestMessage);
            base.HandleOneWayMessage(requestMessage);
        }
    }
}
