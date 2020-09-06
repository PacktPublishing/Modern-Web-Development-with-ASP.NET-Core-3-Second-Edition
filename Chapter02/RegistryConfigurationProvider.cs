using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Win32;

namespace chapter02
{
    public sealed class RegistryConfigurationProvider : ConfigurationProvider
    {
        private readonly RegistryConfigurationSource _configurationSource;

        public RegistryConfigurationProvider(RegistryConfigurationSource configurationSource) => _configurationSource = configurationSource;

        private RegistryKey GetRegistryKey(string key)
        {
            RegistryKey regKey;
            switch (_configurationSource.Hive)
            {
                case RegistryHive.ClassesRoot:
                    regKey = Registry.ClassesRoot;
                    break;

                case RegistryHive.CurrentConfig:
                    regKey = Registry.CurrentConfig;
                    break;

                case RegistryHive.CurrentUser:
                    regKey = Registry.CurrentUser;
                    break;

                case RegistryHive.LocalMachine:
                    regKey = Registry.LocalMachine;
                    break;

                case RegistryHive.PerformanceData:
                    regKey = Registry.PerformanceData;
                    break;

                case RegistryHive.Users:
                    regKey = Registry.Users;
                    break;

                default:
                    throw new InvalidOperationException($"Supplied hive {_configurationSource.Hive} is invalid.");
            }

            var parts = key.Split('\\');
            var subKey = string.Join("", parts.Where(
              (x, i) => i < parts.Length - 1));

            return regKey.OpenSubKey(subKey);
        }

        public override bool TryGet(string key, out string value)
        {
            var regKey = GetRegistryKey(key);
            var parts = key.Split('\\');
            var name = parts.Last();
            var regValue = regKey.GetValue(name);

            value = regValue?.ToString();

            return regValue != null;
        }

        public override void Set(string key, string value)
        {
            var regKey = GetRegistryKey(key);
            var parts = key.Split('\\');
            var name = parts.Last();

            regKey.SetValue(name, value);
        }
    }
}
