using System;
using System.Diagnostics;

namespace Ruhnow.ServiceFabric.Core
{
    public static class Naming
    {
        public static string Microservice<I>()
        {
            Debug.Assert(
                typeof(I).Namespace.Contains(Constant.Manager),
                $"Invalid microservice interface. Use only the {Constant.Manager} interface to access a microservice.");
            string[] namespaceSegments = typeof(I).Namespace.Split('.');
            if (namespaceSegments.Length < Constant.NamespaceMinimumSize)
            {
                throw new FormatException(@"Microservice namespace is an invalid format.");
            }
            string microserviceName = $"{namespaceSegments[0]}.{Constant.Microservice}.{namespaceSegments[2]}";
            if (!string.IsNullOrEmpty(ApplicationContext.EnvironmentName))
            {
                microserviceName += $".{ApplicationContext.EnvironmentName}";
            }

            return microserviceName;
        }

        public static string Component<I>()
        {
            string[] namespaceSegments = typeof(I).Namespace.Split('.');
            if (namespaceSegments.Length < Constant.NamespaceMinimumSize)
            {
                throw new FormatException(@"Component namespace is an invalid format.");
            }

            return $"{namespaceSegments[0]}.{namespaceSegments[1]}.{namespaceSegments[2]}.Host";
        }

        public static string Listener<I>()
        {
            return typeof(I).Name;
        }

        public static string ServiceType<T>()
        {
            return typeof(T).FullName.Replace($".{Constant.Service}", string.Empty);
        }
    }
}
