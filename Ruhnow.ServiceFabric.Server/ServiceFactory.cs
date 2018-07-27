using System;

namespace Ruhnow.ServiceFabric.Server
{
    public abstract class ServiceFactory<TService> : IServiceFactory<TService>
    {
        protected ServiceFactory(Func<TService> factory)
        {
            this.factory = factory;
        }

        private Func<TService> factory;
        protected TService CreateService()
        {
            return factory.Invoke();
        }
        public abstract TService GetService();

        public void DisposeService(TService service)
        {
            if (service is IDisposable)
            {
                ((IDisposable)service).Dispose();
            }
        }
    }
}
