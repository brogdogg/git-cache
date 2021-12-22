/******************************************************************************
 * File...: IResourceLock.cs
 * Remarks: 
 */
using System;

namespace git_cache.Services.ResourceLock
{
  /************************** IResourceLock **********************************/
  /// <summary>
  /// Describes a basic resource lock type object
  /// </summary>
  public interface IResourceLock : IDisposable
  {
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Methods ****************************************/
    /// <summary>
    /// Should wait indefinitely to obtain lock
    /// </summary>
    /// <returns></returns>
    bool WaitOne();
    /// <summary>
    /// Should wait for specified amount of milliseconds to obtain lock
    /// </summary>
    /// <param name="milliseconds">Amount of ms to wait</param>
    /// <exception cref="TimeoutException">
    /// Should throw timeout exception of lock not obtained
    /// </exception>
    /// <returns></returns>
    bool WaitOne(int milliseconds);
    /// <summary>
    /// Should wait for specified time span to obtain lock
    /// </summary>
    /// <param name="timeout">
    /// Time to wait
    /// </param>
    /// <exception cref="TimeoutException">
    /// Should throw timeout exception of lock not obtained
    /// </exception>
    /// <returns></returns>
    bool WaitOne(TimeSpan timeout);
    /// <summary>
    /// Should release the obtained lock
    /// </summary>
    void Release();
  } /* End of Interface - IResourceLock */
}
/* End of document - IResourceLock.cs */