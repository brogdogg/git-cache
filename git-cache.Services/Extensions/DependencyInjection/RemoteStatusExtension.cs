/******************************************************************************
 * File...: RemoteStatusExtension.cs
 * Remarks: 
 */
using git_cache.Services.Git.Status;
using Microsoft.Extensions.DependencyInjection;

namespace git_cache.Services.Extensions.DependencyInjection
{
  /************************** RemoteStatusExtension **************************/
  /// <summary>
  /// Extension for adding the remote status service to the collection
  /// </summary>
  public static class RemoteStatusExtension
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /************************ Fields *****************************************/
    /************************ Static *****************************************/
    /*----------------------- AddRemoteStatusService ------------------------*/
    /// <summary>
    /// Adds needed classes for the <see cref="IRemoteStatus"/> service.
    /// </summary>
    /// <param name="services">
    /// Service collection
    /// </param>
    public static IServiceCollection AddRemoteStatusService(
      this IServiceCollection services)
    {
      // Add in our remote status service
      services.AddSingleton(typeof(IRemoteStatus), typeof(RemoteStatus));
      return services;
    } /* End of Function - AddRemoteStatusService */

  } /* End of Class - RemoteStatusExtension */
}
/* End of document - RemoteStatusExtension.cs */