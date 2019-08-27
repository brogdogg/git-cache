/******************************************************************************
 * File...: GitCacheConfigurationExtension.cs
 * Remarks: 
 */
using git_cache.Services.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace git_cache.Services.Extensions.DependencyInjection
{
  /************************** GitCacheConfigurationExtension *****************/
  /// <summary>
  /// Configuration extension for <see cref="GitCacheConfiguration"/> POCOs
  /// </summary>
  public static class GitCacheConfigurationExtension
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /************************ Fields *****************************************/
    /************************ Static *****************************************/
    /*----------------------- ConfigureGitCache -----------------------------*/
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static IServiceCollection ConfigureGitCache(
      this IServiceCollection services,
      IConfiguration configuration)
    {
      services.Configure<GitCacheConfiguration>(configuration.GetSection("GitCache"));
      return services;
    } /* End of Function - ConfigureGitCache */

  } /* End of Class - GitCacheConfigurationExtension */
}
/* End of document - GitCacheConfigurationExtension.cs */