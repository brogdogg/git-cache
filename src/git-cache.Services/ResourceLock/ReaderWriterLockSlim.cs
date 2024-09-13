/******************************************************************************
 * File...: ReaderWriterLockSlim.cs
 * Remarks: 
 */
using System;

namespace git_cache.Services.ResourceLock
{
  /************************** ReaderWriterLockSlim ***************************/
  /// <summary>
  /// Implementation of the <see cref="IReaderWriterLock"/> interface using
  /// the <see cref="System.Threading.ReaderWriterLockSlim"/> implementation
  /// </summary>
  public class ReaderWriterLockSlim : IReaderWriterLock
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /*----------------------- IsReaderLockHeld ------------------------------*/
    /// <summary>
    /// Gets a flag indicating if the current thread has a reader lock heald
    /// or if the upgradeable state is held.
    /// </summary>
    /// <remarks>
    /// This could return true even when <see cref="IsWriterLockHeld"/> is
    /// true because the upgradeable reader lock will be held to upgrade to
    /// writer lock.
    /// </remarks>
    public bool IsReaderLockHeld
    {
      get
      {
        return m_slim.IsReadLockHeld
          || m_slim.IsUpgradeableReadLockHeld;
      } // end of get - state
    } // end of property - IsReaderLockHeld

    /*----------------------- IsWriterLockHeld ------------------------------*/
    /// <inheritdoc/>
    public bool IsWriterLockHeld
    {
      get { return m_slim.IsWriteLockHeld; }
    } // end of property - IsWriterLockHeld

    /************************ Construction ***********************************/
    /// <summary>
    /// Constructor using a
    /// <see cref="System.Threading.ReaderWriterLockSlim"/> object as the
    /// underlying implementation of the reader/writer lock implementation.
    /// </summary>
    public ReaderWriterLockSlim()
    {
      m_slim = new System.Threading.ReaderWriterLockSlim();
    } // end of function - ReaderWriterLockSlim

    /************************ Methods ****************************************/
    /*----------------------- AcquireReaderLock -----------------------------*/
    /// <inheritdoc/>
    public void AcquireReaderLock(
                  int msTimeout)
    {
      if (!m_slim.TryEnterReadLock(msTimeout))
        throw new TimeoutException($"Failed to acquire read lock, within {msTimeout} milliseconds");
    } // end of function - AcquireReaderLock

    /*----------------------- AcquireReaderLock -----------------------------*/
    /// <inheritdoc/>
    public void AcquireReaderLock(
                  TimeSpan timeout)
    {
      AcquireReaderLock((int)timeout.TotalMilliseconds);
    } // end of function - AcquireReaderLock

    /*----------------------- DowngradeFromWriterLock -----------------------*/
    /// <inheritdoc/>
    public void DowngradeFromWriterLock()
    {
      m_slim.ExitWriteLock();
    } // end of function - DowngradeFromWriterLock

    /*----------------------- ReleaseReaderLock -----------------------------*/
    /// <inheritdoc/>
    public void ReleaseReaderLock()
    {
      if (m_slim.IsReadLockHeld)
        m_slim.ExitReadLock();
      else if (m_slim.IsUpgradeableReadLockHeld)
        m_slim.ExitUpgradeableReadLock();
    } // end of function - ReleaseReaderLock

    /*----------------------- ReleaseWriterLock -----------------------------*/
    /// <inheritdoc/>
    public void ReleaseWriterLock()
    {
      m_slim.ExitWriteLock();
    } // end of function - ReleaseWriterLock

    /*----------------------- UpgradeToWriterLock ---------------------------*/
    /// <inheritdoc/>
    public void UpgradeToWriterLock(
                  int msTimeout)
    {
      if (!IsReaderLockHeld)
        throw new InvalidOperationException(
          "Trying to upgrade without holding the reader lock");
      // At this point, we will release our normal reader lock
      m_slim.ExitReadLock();
      // And attempt to get an upgradeable reader lock instead
      if (!m_slim.TryEnterUpgradeableReadLock(msTimeout))
        throw new TimeoutException($"Failed to obtain upgradeable read lock in {msTimeout} milliseconds");
      // Because once we have the upgradeable reader lock, then we can
      // go to a writer lock
      if (!m_slim.TryEnterWriteLock(msTimeout))
        throw new TimeoutException($"Failed to upgrade to writer in {msTimeout} milliseconds");
    } // end of function - UpgradeToWriterLock

    /*----------------------- UpgradeToWriterLock ---------------------------*/
    /// <inheritdoc/>
    public void UpgradeToWriterLock(
                  TimeSpan timeout)
    {
      UpgradeToWriterLock((int)timeout.TotalMilliseconds);
    } // end of function - UpgradeToWriterLock
    /************************ Fields *****************************************/
    /************************ Static *****************************************/

    /*======================= PROTECTED =====================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /************************ Fields *****************************************/
    /************************ Static *****************************************/

    /*======================= PRIVATE =======================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /************************ Fields *****************************************/
    private System.Threading.ReaderWriterLockSlim m_slim = null;
    /************************ Static *****************************************/
  } /* End of Class - ReaderWriterLockSlim */
}
/* End of document - ReaderWriterLockSlim.cs */