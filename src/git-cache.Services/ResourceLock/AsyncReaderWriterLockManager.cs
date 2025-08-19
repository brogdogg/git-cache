/******************************************************************************
 * File...: AsyncReaderWriterLockManager.cs
 * Remarks: Concrete implementation of the IAsyncReaderWriterLockManager
 *          interface.
 */
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace git_cache.Services.ResourceLock
{
  /************************** AsyncReaderWriterLockManager *******************/
  /// <summary>
  /// Implementation of the <see cref="IAsyncReaderWriterLockManager{TKey}"/>
  /// interface
  /// </summary>
  public class AsyncReaderWriterLockManager<TKey>
    : DisposableBase, IAsyncReaderWriterLockManager<TKey>
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /*----------------------- AsyncReaderWriterLockManager ------------------*/
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="serviceProvider">Service provider to resolve dependencies</param>
    /// <param name="logger">Logger object tied to this instance</param>
    public AsyncReaderWriterLockManager(
      IServiceProvider serviceProvider,
      ILogger<AsyncReaderWriterLockManager<TKey>> logger)
    {
      m_logger = logger ?? throw new ArgumentNullException(nameof(logger));
      m_serviceProvider = serviceProvider
        ?? throw new ArgumentNullException(nameof(serviceProvider));
    } /* End of Function - AsyncReaderWriterLockManager */

    /************************ Methods ****************************************/
    /*----------------------- GetFor ----------------------------------------*/
    /// <inheritdoc />
    public IAsyncReaderWriterLock GetFor(TKey key)
    {
      m_logger.LogInformation($"Getting resource lock for key: {key}");
      IAsyncReaderWriterLock retval = null;
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
          retval = m_serviceProvider.GetRequiredService<IAsyncReaderWriterLock>();
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
    /// <inheritdoc />
    protected override void Dispose(bool disposing)
    {
      lock (m_lock)
      {
        if (null != m_readerWriters)
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
    private readonly ILogger<AsyncReaderWriterLockManager<TKey>> m_logger = null;
    private Dictionary<TKey, IAsyncReaderWriterLock> m_readerWriters
      = new Dictionary<TKey, IAsyncReaderWriterLock>();
    private readonly IServiceProvider m_serviceProvider;
    /************************ Static *****************************************/
  } /* End of Class - AsyncReaderWriterLockManager */
}
/* End of document - AsyncReaderWriterLockManager.cs */
