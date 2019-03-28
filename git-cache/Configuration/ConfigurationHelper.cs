/******************************************************************************
 * File...: ConfigurationHelper.cs
 * Remarks: 
 */
using Microsoft.Extensions.Configuration;

namespace git_cache.Configuration
{
  /************************** ConfigurationHelper ****************************/
  /// <summary>
  /// <see cref="IConfiguration"/> extension class, for dealing with git-cache
  /// specific configuration settings.
  /// </summary>
  public static class ConfigurationHelper
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /************************ Fields *****************************************/
    /************************ Static *****************************************/
    /// <summary>
    /// The default storage path used if not specified
    /// </summary>
    public static string DEFAULT_STORAGE_PATH = "/tmp";
    /// <summary>
    /// Key for the cache directory configuration setting
    /// </summary>
    public static string CACHE_DIRECTORY_KEY = "Cache:Directory";
    /// <summary>
    /// Key for the DisableHTTPS configuration setting
    /// </summary>
    public static string DISABLE_HTTPS_KEY = "ConnectionSettings:DisableHTTPS";
    /*----------------------- GetLocalStoragePath ---------------------------*/
    /// <summary>
    /// Gets the local storage path from the configuration item
    /// </summary>
    /// <param name="config">
    /// Configuration to get the path from
    /// </param>
    public static string GetLocalStoragePath(IConfiguration config)
    {
      return GetLocalStoragePath(config, DEFAULT_STORAGE_PATH);
    } /* End of Function - GetLocalStoragePath */

    /*----------------------- GetLocalStoragePath ---------------------------*/
    /// <summary>
    /// Gets the local storage path, with a default path if it does not exists
    /// </summary>
    /// <param name="config">
    /// Configuration to get values from
    /// </param>
    /// <param name="defaultPath">
    /// The default path to use, if the item does not exists in the
    /// configuration
    /// </param>
    public static string GetLocalStoragePath(IConfiguration config, string defaultPath)
    {
      return config.GetValue<string>(CACHE_DIRECTORY_KEY, defaultPath);
    } /* End of Function - GetLocalStoragePath */

    /*----------------------- DisableHTTPS ----------------------------------*/
    /// <summary>
    /// Gets the `DisableHTTPS` value from configuration, defaulting to false
    /// </summary>
    /// <param name="config">
    /// Configuration to read from
    /// </param>
    /// <param name="defaultValue">
    /// Able to provide a default value, if not specified false is given
    /// </param>
    public static bool DisableHTTPS(IConfiguration config, bool defaultValue = false)
    {
      return config.GetValue<bool>(DISABLE_HTTPS_KEY, defaultValue);
    } /* End of Function - DisableHTTPS */

  } /* End of Class - ConfigurationHelper */
} /* end of namespace - git_cache.Configuration */
/* End of document - ConfigurationHelper.cs */