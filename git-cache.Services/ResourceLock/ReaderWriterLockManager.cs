/******************************************************************************
 * File...: ReaderWriterLockManager.cs
 * Remarks: 
 */
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace git_cache.Services.ResourceLock
{
  /************************** ReaderWriterLockManager ************************/
  /// <summary>
  /// 
  /// </summary>
  public class ReaderWriterLockManager<TKey>
    : DisposableBase, IReaderWriterLockManager<TKey>
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /*----------------------- ReaderWriterLockManager -----------------------*/
    /// <summary>
    /// 
    /// </summary>
    /// <param name="factory"></param>
    public ReaderWriterLockManager(IReaderWriterLockFactory factory, ILogger<ReaderWriterLockManager<TKey>> logger)
    {
      if (null == (m_logger = logger))
        throw new ArgumentNullException(
          nameof(logger), "Must provide a valid logger class");
      if (null == (m_factory = factory))
        throw new ArgumentNullException(
          nameof(factory), "Must provide a valid factory class");
    } /* End of Function - ReaderWriterLockManager */

    /************************ Methods ****************************************/
    /*----------------------- GetFor ----------------------------------------*/
    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    public IReaderWriterLock GetFor(TKey key)
    {
      m_logger.LogInformation($"Getting resource lock for key: {key}");
      IReaderWriterLock retval = null;
      lock (m_lock)
      {
        // Check to see if we have an item already for this
        // key
        if (m_readerWriters.ContainsKey(key))
        {
          m_logger.LogInformation("Already have an object for the given key, retrieving...");
          retval = m_readerWriters[key];
        }
        // Otherwise, create a new one and keep track of it
        // for next time
        else
        {
          m_logger.LogInformation("Creating a new reader/writer lock resource for given key");
          retval = m_factory.Create();
          m_readerWriters.Add(key, retval);
        } // end of else - new item
      } // end of lock - on resource
      m_logger.LogInformation($"Returning resource lock for key: {key}");
      return retval;
    } /* End of Function - GetFor */
    /************************ Fields *****************************************/
    /************************ Static *****************************************/

    /*======================= PROTECTED =====================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /*----------------------- Dispose ---------------------------------------*/
    /// <summary>
    /// 
    /// </summary>
    /// <param name="disposing"></param>
    protected override void Dispose(bool disposing)
    {
      lock (m_lock)
      {
        if(null != m_readerWriters)
        {
          m_readerWriters.Clear();
          m_readerWriters = null;
        } // end of if - valid resource dictionary
      } // end of lock - on resource
    } /* End of Function - Dispose */
    /************************ Fields *****************************************/
    /************************ Static *****************************************/

    /*======================= PRIVATE =======================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /************************ Fields *****************************************/
    private readonly object m_lock = new object();
    private readonly ILogger<ReaderWriterLockManager<TKey>> m_logger = null;
    private Dictionary<TKey, IReaderWriterLock> m_readerWriters
      = new Dictionary<TKey, IReaderWriterLock>();
    private readonly IReaderWriterLockFactory m_factory = null;
    /************************ Static *****************************************/
  } /* End of Class - ReaderWriterLockManager */
}
/* End of document - ReaderWriterLockManager.cs */