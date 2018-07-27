namespace Ruhnow.ServiceFabric.Server
{

    public static partial class ServiceInitializationExtensions
    {
        public class SingletonServiceFactory<TService> : ServiceFactory<TService>
        {
            public SingletonServiceFactory(System.Func<TService> factory) : base(factory) { }
            public override TService GetService()
            {
                if (service == null)
                {
                    service = CreateService();
                }

                return service;
            }
            private TService service;

            public void Reset()
            {
                this.service = default(TService);
            }
        }
    }
}
