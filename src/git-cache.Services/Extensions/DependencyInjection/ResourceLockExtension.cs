/******************************************************************************
 * File...: ResourceLockExtension.cs
 * Remarks:
 */
using git_cache.Services.ResourceLock;

using Microsoft.Extensions.DependencyInjection;

namespace git_cache.Services.Extensions.DependencyInjection
{
  /************************** ResourceLockExtension **************************/
  /// <summary>
  /// Adds the appropriate classes to the service collection for dealing
  /// with resource locks
  /// </summary>
  public static class ResourceLockExtension
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /************************ Fields *****************************************/
    /************************ Static *****************************************/
    /*----------------------- AddAsyncResourceLocks -------------------------*/
    /// <summary>
    /// Adds the asynchronous resource lock classes to the service collection.
    /// </summary>
    /// <param name="services">The service collection to add the locks to.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddAsyncResourceLocks(this IServiceCollection services)
    {
      // Add the async reader/writer lock classes to the services
      services.AddTransient<IAsyncReaderWriterLock, AsyncReaderWriterLock>()
              .AddSingleton(typeof(IAsyncReaderWriterLockManager<>), typeof(AsyncReaderWriterLockManager<>));

      return services;
    } /* End of Function - AddAsyncResourceLocks */
  } /* End of Class - ResourceLockExtension */
}
/* End of document - ResourceLockExtension.cs */
