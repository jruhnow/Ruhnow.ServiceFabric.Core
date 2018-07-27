using Microsoft.ServiceFabric.Services.Remoting.V2;

namespace Ruhnow.ServiceFabric.Core.Pipeline
{
    public interface IPipelineResponseEventHandler
    {
        void Process(IServiceRemotingResponseMessage responseMessage);
    }
}
