namespace Ruhnow.ServiceFabric.Server
{
    public interface IServiceFactory<TService>
    {
        TService GetService();

        void DisposeService(TService service);
    }
}
