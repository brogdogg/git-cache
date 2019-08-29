/******************************************************************************
 * File...: IResourceLockManager.cs
 * Remarks: 
 */
using System;

namespace git_cache.Services.ResourceLock
{
  /************************** IResourceLockManager ***************************/
  /// <summary>
  /// Resource lock manager
  /// </summary>
  public interface IResourceLockManager : IDisposable
  {
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Methods ****************************************/
  } /* End of Interface - IResourceLockManager */

  /************************** IResourceLockManager ***************************/
  /// <summary>
  /// Resource lock manager, with the key type specified
  /// </summary>
  public interface IResourceLockManager<TKey> : IResourceLockManager
  {
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Methods ****************************************/
    /// <summary>
    /// Should block indefinitely for the resource lock associated with the
    /// key.
    /// </summary>
    /// <param name="key">
    /// Key associated with the resource lock.
    /// </param>
    /// <returns>
    /// Resource lock associated with the key
    /// </returns>
    IResourceLock BlockFor(TKey key);
    /// <summary>
    /// Should block for the resource associated with the key for the set
    /// amount of time, before timing out
    /// </summary>
    /// <param name="key">
    /// Key associated with the resource lock.
    /// </param>
    /// <param name="timeout">
    /// Amount of time to wait before timing out
    /// </param>
    /// <returns>
    /// Resource lock associated with the key
    /// </returns>
    /// <exception cref="TimeoutException">
    /// Should be thrown if the lock is not obtained within the
    /// specified amount of time.
    /// </exception>
    IResourceLock BlockFor(TKey key, TimeSpan timeout);
    /// <summary>
    /// Gets the resource lock associated with the key
    /// </summary>
    /// <param name="key">
    /// Key associated with the resource lock
    /// </param>
    /// <returns>
    /// Should always return a resource lock for the key, creating
    /// a new one if it did not already exists.
    /// </returns>
    IResourceLock GetFor(TKey key);
  } /* End of Interface - IResourceLockManager */
}
/* End of document - IResourceLockManager.cs */