using System;

namespace Ruhnow.ServiceFabric.Server
{
    public class PerCallServiceFactory<TService> : ServiceFactory<TService>
    {
        public PerCallServiceFactory(Func<TService> factory) : base(factory) { }

        public override TService GetService()
        {
            return this.CreateService(); ;
        }
    }
}
