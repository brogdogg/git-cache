/******************************************************************************
 * File...: ReaderWriterLockSlim.cs
 * Remarks: 
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace git_cache.Services.ResourceLock
{
  /************************** ReaderWriterLockSlim ***************************/
  /// <summary>
  /// 
  /// </summary>
  public class ReaderWriterLockSlim : IReaderWriterLock
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    public bool IsReaderLockHeld
    {
      get { return m_slim.IsUpgradeableReadLockHeld; }
    }

    public bool IsWriterLockHeld
    {
      get { return m_slim.IsWriteLockHeld; }
    }

    public int WriterSequenceNumber
    {
      get { return m_slim.WaitingWriteCount; }
    }

    /************************ Construction ***********************************/
    public ReaderWriterLockSlim()
    {
      m_slim = new System.Threading.ReaderWriterLockSlim();
    }
    /************************ Methods ****************************************/
    public void AcquireReaderLock(int msTimeout)
    {
      if (!m_slim.TryEnterUpgradeableReadLock(msTimeout))
        throw new TimeoutException("Failed to acquire read lock");
    }

    public void AcquireReaderLock(TimeSpan timeout)
    {
      if (!m_slim.TryEnterUpgradeableReadLock(timeout))
        throw new TimeoutException("Failed to acquire read lock");
    }

    public void AcquireWriterLock(int msTimeout)
    {
      if (!m_slim.TryEnterWriteLock(msTimeout))
        throw new TimeoutException("Failed to acquire write lock");
    }

    public void AcquireWriterLock(TimeSpan timeout)
    {
      if (!m_slim.TryEnterWriteLock(timeout))
        throw new TimeoutException("Failed to acquire write lock");
    }

    public bool AnyWritersSince(int seqNum)
    {
      throw new NotImplementedException();
    }

    public void DowngradeFromWriterLock(ref LockCookie lockCookie)
    {
      m_slim.ExitWriteLock();
    }

    public LockCookie ReleaseLock()
    {
      if (m_slim.IsWriteLockHeld)
        m_slim.ExitWriteLock();
      if (m_slim.IsUpgradeableReadLockHeld)
        m_slim.ExitUpgradeableReadLock();
      return default(LockCookie);
    }

    public void ReleaseReaderLock()
    {
      m_slim.ExitUpgradeableReadLock();
    }

    public void ReleaseWriterLock()
    {
      m_slim.ExitWriteLock();
    }

    public void RestoreLock(ref LockCookie lockCookie)
    {
      throw new NotImplementedException();
    }

    public LockCookie UpgradeToWriterLock(int msTimeout)
    {
      if (!m_slim.TryEnterWriteLock(msTimeout))
        throw new TimeoutException("Failed to upgrade to writer");
      return default(LockCookie);
    }

    public LockCookie UpgradeToWriterLock(TimeSpan timeout)
    {
      if (!m_slim.TryEnterWriteLock(timeout))
        throw new TimeoutException("Failed to upgrade to writer");
      return default(LockCookie);
    }
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