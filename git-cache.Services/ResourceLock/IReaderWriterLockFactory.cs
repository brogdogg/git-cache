/******************************************************************************
 * File...: IReaderWriterLockFactory.cs
 * Remarks: 
 */

namespace git_cache.Services.ResourceLock
{
  /************************** IReaderWriterLockFactory ***********************/
  /// <summary>
  /// Describes a class responsible for creating a
  /// <see cref="IReaderWriterLock"/> type object
  /// </summary>
  public interface IReaderWriterLockFactory
  {
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Methods ****************************************/
    /// <summary>
    /// Should create an instance of <see cref="IReaderWriterLock"/> type
    /// object.
    /// </summary>
    /// <returns></returns>
    IReaderWriterLock Create();
  } /* End of Interface - IReaderWriterLockFactory */

  /************************** IReaderWriterLockFactory ***********************/
  /// <summary>
  /// 
  /// </summary>
  public interface IReaderWriterLockFactory<out TLock>: IReaderWriterLockFactory
    where TLock: IReaderWriterLock, new()
  {
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Methods ****************************************/
    new TLock Create();
  } /* End of Interface - IReaderWriterLockFactory */
}
/* End of document - IReaderWriterLockFactory.cs */