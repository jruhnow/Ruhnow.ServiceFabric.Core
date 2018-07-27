using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Text;

namespace Ruhnow.ServiceFabric.Server.Configuration
{
    public static class ConfigurationExtensions
    {
        public static string GetConnectionString(this IConfigurationRoot config, string connectionStringName)
        {
            return config.GetSection($"ConnectionStrings:{connectionStringName}").Value;
        }
        public static ConfigurationPackage GetDefaultConfigurationPackage(this ServiceContext context)
        {
            return GetConfigurationPackage(context, (@"Config"));
        }
        public static ConfigurationPackage GetConfigurationPackage(this ServiceContext context, string packageName)
        {
            return context.CodePackageActivationContext.GetConfigurationPackageObject(packageName);
        }
        public static System.Fabric.Description.ConfigurationSection GetConfigurationSection(this ServiceContext context, string sectionName)
        {
            return GetDefaultConfigurationPackage(context).Settings.Sections[sectionName];
        }
        public static System.Fabric.Description.ConfigurationSection GetConfigurationSection(this ServiceContext context, string packageName, string sectionName)
        {
            return GetConfigurationPackage(context, packageName).Settings.Sections[sectionName];
        }
        public static string GetConfigurationSetting(this ServiceContext context, string packageName, string sectionName, string settingName)
        {
            return GetConfigurationSection(context, packageName, sectionName).Parameters[settingName].Value;
        }
        public static string GetConfigurationSetting(this ServiceContext context, string sectionName, string settingName)
        {
            return GetConfigurationSection(context, sectionName).Parameters[settingName].Value;
        }
        public static string GetConfigurationConnectionString(this ServiceContext context, string connectionStringName)
        {
            return GetConfigurationSetting(context, "ConnectionStrings", connectionStringName);
        }
    }
}
