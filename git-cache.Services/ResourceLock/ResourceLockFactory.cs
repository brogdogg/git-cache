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
  public class ResourceLockFactory<TLock> : IResourceLockFactory
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
    public IResourceLock Create() => new TLock();
  } /* End of Class - ResourceLockFactory */
}
/* End of document - ResourceLockFactory.cs */