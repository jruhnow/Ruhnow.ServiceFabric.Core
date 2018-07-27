using Microsoft.ServiceFabric.Services.Remoting.V2;

namespace Ruhnow.ServiceFabric.Core.Pipeline
{
    public interface IPipelineRequestEventHandler
    {
        void Process(IServiceRemotingRequestMessage requestMessage);
    }
}
