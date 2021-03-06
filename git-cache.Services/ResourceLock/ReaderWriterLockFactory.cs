﻿/******************************************************************************
 * File...: ReaderWriterLockFactory.cs
 * Remarks: 
 */

namespace git_cache.Services.ResourceLock
{
  /************************** ReaderWriterLockFactory ************************/
  /// <summary>
  /// Implementation of the <see cref="IReaderWriterLockFactory{TLock}"/>
  /// interface.
  /// </summary>
  public class ReaderWriterLockFactory<TLock> : IReaderWriterLockFactory<TLock>
    where TLock : IReaderWriterLock, new()
  {

    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /// <summary>
    /// Should create a reader/writer resource lock
    /// </summary>
    /// <returns></returns>
    public TLock Create() => new TLock();

    /*----------------------- IReaderWriterLockFactory.Create ---------------*/
    /// <summary>
    /// Explicit implementation of
    /// <see cref="IReaderWriterLockFactory.Create"/>.
    /// </summary>
    IReaderWriterLock IReaderWriterLockFactory.Create()
    {
      return Create();
    } /* End of Function - IReaderWriterLockFactory.Create */
    /************************ Fields *****************************************/
    /************************ Static *****************************************/
  } /* End of Class - ReaderWriterLockFactory */
}
/* End of document - ReaderWriterLockFactory.cs */