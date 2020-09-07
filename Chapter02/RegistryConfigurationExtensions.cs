using Microsoft.Extensions.Configuration;
using Microsoft.Win32;

namespace chapter02
{
    public static class RegistryConfigurationExtensions
    {
        public static IConfigurationBuilder AddRegistry(this IConfigurationBuilder builder, RegistryHive hive = RegistryHive.CurrentUser) => builder.Add(new RegistryConfigurationSource { Hive = hive });
    }
}
