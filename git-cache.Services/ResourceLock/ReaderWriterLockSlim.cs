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
    /// <inheritdoc/>
    public bool IsReaderLockHeld
    {
      get { return m_slim.IsUpgradeableReadLockHeld; }
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
      if (!m_slim.TryEnterUpgradeableReadLock(msTimeout))
        throw new TimeoutException("Failed to acquire read lock");
    } // end of function - AcquireReaderLock

    /*----------------------- AcquireReaderLock -----------------------------*/
    /// <inheritdoc/>
    public void AcquireReaderLock(
                  TimeSpan timeout)
    {
      AcquireReaderLock(timeout.Milliseconds);
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
      if (!m_slim.TryEnterWriteLock(msTimeout))
        throw new TimeoutException("Failed to upgrade to writer");
    } // end of function - UpgradeToWriterLock

    /*----------------------- UpgradeToWriterLock ---------------------------*/
    /// <inheritdoc/>
    public void UpgradeToWriterLock(
                  TimeSpan timeout)
    {
      UpgradeToWriterLock(timeout.Milliseconds);
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