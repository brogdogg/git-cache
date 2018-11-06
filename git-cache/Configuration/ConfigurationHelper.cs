using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace git_cache.Configuration
{
  public static class ConfigurationHelper
  {
    public static string GetLocalStoragePath(this IConfiguration config)
    {
      return GetLocalStoragePath(config, "/tmp");
    }
    public static string GetLocalStoragePath(this IConfiguration config, string defaultPath)
    {
      return config.GetValue<string>("Cache:Directory", defaultPath);
    }

    public static bool DisableHTTPS(this IConfiguration config, bool defaultValue = false)
    {
      return config.GetValue<bool>("ConnectionSettings:DisableHTTPS", defaultValue);
    }
  }
}
