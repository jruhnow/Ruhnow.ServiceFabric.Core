namespace Ruhnow.ServiceFabric.Core
{
    public class ApplicationContext
    {
        public static void Initialize(System.Fabric.ServiceContext context)
        {
            ApplicationName = context.CodePackageActivationContext.ApplicationName;
            string typeName = context.CodePackageActivationContext.ApplicationTypeName;
            typeName = typeName.Remove(typeName.Length - 4, 4);
            string unqualifiedApplicationName = $"{Constant.FabricScheme}/{typeName}";
            string environmentName = ApplicationName.Replace(unqualifiedApplicationName, "");
            if (environmentName.Length > 1)
            {
                // remove the . character to get the environment name extracted
                EnvironmentName = environmentName.Substring(1, environmentName.Length - 1);
            }
        }

        public static string ApplicationName { get; private set; }
        public static string EnvironmentName { get; private set; }
    }
}
