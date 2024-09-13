/******************************************************************************
 * File...: IReaderWriterLockManager.cs
 * Remarks: 
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace git_cache.Services.ResourceLock
{
  /************************** IReaderWriterLockManager ***********************/
  /// <summary>
  /// Reader/Writer resource lock manager
  /// </summary>
  public interface IReaderWriterLockManager : IDisposable
  {
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Methods ****************************************/
  } /* End of Interface - IReaderWriterLockManager */


  /************************** IReaderWriterLockManager ***********************/
  /// <summary>
  /// Reader/writer resource lock manager, with the key type specified
  /// </summary>
  public interface IReaderWriterLockManager<in TKey> : IReaderWriterLockManager
  {
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Methods ****************************************/
    /// <summary>
    /// Gets the <see cref="IReaderWriterLock"/> object associated with
    /// the key
    /// </summary>
    /// <param name="key">
    /// Key for the reader/writer lock
    /// </param>
    /// <returns>Instance tied to the key</returns>
    IReaderWriterLock GetFor(TKey key);
  } /* End of Interface - IReaderWriterLockManager */
}
/* End of document - IReaderWriterLockManager.cs */