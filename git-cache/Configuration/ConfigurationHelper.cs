/******************************************************************************
 * 
 */
using Microsoft.Extensions.Configuration;

namespace git_cache.Configuration
{
  /************************** ConfigurationHelper ****************************/
  /// <summary>
  /// 
  /// </summary>
  public static class ConfigurationHelper
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /*----------------------- GetLocalStoragePath ---------------------------*/
    /// <summary>
    /// Gets the local storage path from the configuration item
    /// </summary>
    /// <param name="config">
    /// Configuration to get the path from
    /// </param>
    public static string GetLocalStoragePath(this IConfiguration config)
    {
      return GetLocalStoragePath(config, "/tmp");
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
    public static string GetLocalStoragePath(this IConfiguration config, string defaultPath)
    {
      return config.GetValue<string>("Cache:Directory", defaultPath);
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
    public static bool DisableHTTPS(this IConfiguration config, bool defaultValue = false)
    {
      return config.GetValue<bool>("ConnectionSettings:DisableHTTPS", defaultValue);
    } /* End of Function - DisableHTTPS */
    /************************ Fields *****************************************/
    /************************ Static *****************************************/

    /*======================= PROTECTED =====================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /************************ Fields *****************************************/
    /************************ Static *****************************************/

    /*======================= PRIVATE =======================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /************************ Fields *****************************************/
    /************************ Static *****************************************/

  } /* End of Class - ConfigurationHelper */
}
/* End of document - ConfigurationHelper.cs */