/******************************************************************************
 * File...: IResourceLockFactory.cs
 * Remarks: 
 */

namespace git_cache.Services.ResourceLock
{
  /************************** IResourceLockFactory ***************************/
  /// <summary>
  /// Describes a factory class responsible for creating
  /// <see cref="IResourceLock"/> objects.
  /// </summary>
  public interface IResourceLockFactory
  {
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Methods ****************************************/
    /// <summary>
    /// Creates a <see cref="IResourceLock"/> object
    /// </summary>
    /// <returns></returns>
    IResourceLock Create();
  } /* End of Interface - IResourceLockFactory */

  /************************** IResourceLockFactory ***************************/
  /// <summary>
  /// Describes a factory class responsible for creating a specific types
  /// <see cref="IResourceLock"/> objects.
  /// </summary>
  public interface IResourceLockFactory<TLock> : IResourceLockFactory
    where TLock : IResourceLock, new()
  {

    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Methods ****************************************/
    /// <summary>
    /// Creates a <see cref="TLock"/> type of <see cref="IResourceLock"/>
    /// obect.
    /// </summary>
    /// <returns></returns>
    TLock Create();
  } /* End of Interface - IResourceLockFactory */
}
/* End of document - IResourceLockFactory.cs */