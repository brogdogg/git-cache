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
  /// 
  /// </summary>
  public interface IReaderWriterLockManager : IDisposable
  {

    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Methods ****************************************/
  } /* End of Interface - IReaderWriterLockManager */


  /************************** IReaderWriterLockManager ***********************/
  /// <summary>
  /// 
  /// </summary>
  public interface IReaderWriterLockManager<in TKey> : IReaderWriterLockManager
  {
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Methods ****************************************/
    IReaderWriterLock GetFor(TKey key);
  } /* End of Interface - IReaderWriterLockManager */
}
/* End of document - IReaderWriterLockManager.cs */