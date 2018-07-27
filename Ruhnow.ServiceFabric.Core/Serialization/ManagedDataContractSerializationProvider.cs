using Microsoft.ServiceFabric.Services.Remoting.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ruhnow.ServiceFabric.Core.Serialization
{
    public class ManagedDataContractSerializationProvider : IServiceRemotingMessageSerializationProvider
    {
        private ServiceRemotingDataContractSerializationProvider provider;
        private static IList<Type> myTypes = new List<Type>();

        public static void RegisterType<TType>()
        {
            myTypes.Add(typeof(TType));
        }

        public static void RegisterType(Type type)
        {
            myTypes.Add(type);
        }

        public static void RegisterTypes(IEnumerable<Type> types)
        {
            foreach (Type type in types)
            {
                RegisterType(type);
                if (!type.IsArray)
                {
                    RegisterType(type.MakeArrayType());
                }
            }
        }
        public ManagedDataContractSerializationProvider()
        {
            this.provider = new ServiceRemotingDataContractSerializationProvider();
        }
        public IServiceRemotingMessageBodyFactory CreateMessageBodyFactory()
        {
            return this.provider.CreateMessageBodyFactory();
        }
        public IServiceRemotingRequestMessageBodySerializer CreateRequestMessageSerializer(Type serviceInterfaceType, IEnumerable<Type> requestWrappedTypes, IEnumerable<Type> requestBodyTypes = null)
        {
            var result = requestBodyTypes.Concat(myTypes);
            return this.provider.CreateRequestMessageSerializer(serviceInterfaceType, result);
        }

        public IServiceRemotingResponseMessageBodySerializer CreateResponseMessageSerializer(Type serviceInterfaceType, IEnumerable<Type> responseWrappedTypes, IEnumerable<Type> responseBodyTypes = null)
        {
            var result = responseBodyTypes.Concat(myTypes);
            return this.provider.CreateResponseMessageSerializer(serviceInterfaceType, result);
        }
    }
}
