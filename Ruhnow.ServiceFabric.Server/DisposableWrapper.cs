using System;

namespace Ruhnow.ServiceFabric.Server
{
    public class DisposableWrapper<TService> : IDisposable
    {
        public DisposableWrapper(IServiceFactory<TService> factory)
        {
            this.Factory = factory;
        }
        protected IServiceFactory<TService> Factory { get; set; }
        public TService Service { get { return Factory.GetService(); } }
        public void Dispose()
        {
            Factory.DisposeService(this.Service);
        }
    }
}
