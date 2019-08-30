/******************************************************************************
 * File...: ResourceLockFactory.cs
 * Remarks: 
 */

namespace git_cache.Services.ResourceLock
{
  /************************** ResourceLockFactory ****************************/
  /// <summary>
  /// Factory class for resource locks
  /// </summary>
  public class ResourceLockFactory<TLock> : IResourceLockFactory<TLock>
    where TLock : IResourceLock, new()
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /************************ Fields *****************************************/
    /************************ Static *****************************************/
    /// <summary>
    /// Should create a resource lock
    /// </summary>
    /// <returns></returns>
    public TLock Create() => new TLock();

    /*----------------------- IResourceLockFactory.Create -------------------*/
    /// <summary>
    /// Explicit impl of <see cref="IResourceLockFactory.Create"/>
    /// </summary>
    IResourceLock IResourceLockFactory.Create()
    {
      return Create();
    } /* End of Function - IResourceLockFactory.Create */
  } /* End of Class - ResourceLockFactory */
}
/* End of document - ResourceLockFactory.cs */