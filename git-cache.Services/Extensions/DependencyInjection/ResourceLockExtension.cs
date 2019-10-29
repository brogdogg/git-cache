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
    /*----------------------- AddResourceLocks ------------------------------*/
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    public static IServiceCollection AddResourceLocks(this IServiceCollection services)
    {
      // Add the generic resource lock factory to have a specific one used
      // if desired
      services.AddSingleton(typeof(IResourceLockFactory<>), typeof(ResourceLockFactory<>));
      // But then for the non-specific factory, will default to our
      // ResourceLock instead
      services.AddSingleton<IResourceLockFactory, ResourceLockFactory<Services.ResourceLock.ResourceLock>>();
      // And we will allow for a generic use of the lock manager, so a
      // consumer can use the type of key desired
      services.AddSingleton(typeof(IResourceLockManager<>), typeof(ResourceLockManager<>));

      // Add the reader/writer lock classes to the services
      services.AddSingleton(typeof(IReaderWriterLockFactory<>), typeof(ReaderWriterLockFactory<>))
              .AddSingleton<IReaderWriterLockFactory, ReaderWriterLockFactory<Services.ResourceLock.ReaderWriterLockSlim>>()
              .AddSingleton(typeof(IReaderWriterLockManager<>), typeof(ReaderWriterLockManager<>));

      return services;
    } /* End of Function - AddResourceLocks */

  } /* End of Class - ResourceLockExtension */
}
/* End of document - ResourceLockExtension.cs */