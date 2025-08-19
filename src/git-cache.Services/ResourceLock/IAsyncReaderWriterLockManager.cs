/******************************************************************************
 * File...: IAsyncReaderWriterLockManager.cs
 * Remarks:
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace git_cache.Services.ResourceLock
{
  /************************** IAsyncReaderWriterLockManager ******************/
  /// <summary>
  /// Async reader/writer resource lock manager, with the key type specified
  /// </summary>
  public interface IAsyncReaderWriterLockManager<in TKey>
  {
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Methods ****************************************/
    /// <summary>
    /// Gets the <see cref="IAsyncReaderWriterLock"/> object associated with
    /// the key
    /// </summary>
    /// <param name="key">
    /// Key for the async reader/writer lock
    /// </param>
    /// <returns>Instance tied to the key</returns>
    IAsyncReaderWriterLock GetFor(TKey key);
  } /* End of Interface - IAsyncReaderWriterLockManager */
}
/* End of document - IAsyncReaderWriterLockManager.cs */
