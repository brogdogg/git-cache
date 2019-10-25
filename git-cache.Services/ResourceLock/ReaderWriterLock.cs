using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace git_cache.Services.ResourceLock
{
  /************************** ReaderWriterLock *******************************/
  /// <summary>
  /// 
  /// </summary>
  public class ReaderWriterLock : IReaderWriterLock 
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    public bool IsReaderLockHeld
    {
      get { return m_obj.IsReaderLockHeld; }
    }

    public bool IsWriterLockHeld
    {
      get { return m_obj.IsWriterLockHeld; }
    }

    public int WriterSequenceNumber
    {
      get { return m_obj.WriterSeqNum; }
    }
    /************************ Construction ***********************************/
    public ReaderWriterLock()
    {
      m_obj = new System.Threading.ReaderWriterLock();
    }
    /************************ Methods ****************************************/

    public void AcquireReaderLock(int msTimeout)
    {
      m_obj.AcquireReaderLock(msTimeout);
    }

    public void AcquireReaderLock(TimeSpan timeout)
    {
      m_obj.AcquireReaderLock(timeout);
    }

    public void AcquireWriterLock(int msTimeout)
    {
      m_obj.AcquireWriterLock(msTimeout);
    }

    public void AcquireWriterLock(TimeSpan timeout)
    {
      m_obj.AcquireWriterLock(timeout);
    }

    public bool AnyWritersSince(int seqNum)
    {
      return m_obj.AnyWritersSince(seqNum);
    }

    public void DowngradeFromWriterLock(ref LockCookie lockCookie)
    {
      m_obj.DowngradeFromWriterLock(ref lockCookie);
    }

    public LockCookie ReleaseLock()
    {
      return m_obj.ReleaseLock();
    }

    public void ReleaseReaderLock()
    {
      m_obj.ReleaseReaderLock();
    }

    public void ReleaseWriterLock()
    {
      m_obj.ReleaseWriterLock();
    }

    public void RestoreLock(ref LockCookie lockCookie)
    {
      m_obj.RestoreLock(ref lockCookie);
    }

    public LockCookie UpgradeToWriterLock(int msTimeout)
    {
      return m_obj.UpgradeToWriterLock(msTimeout);
    }

    public LockCookie UpgradeToWriterLock(TimeSpan timeout)
    {
      return m_obj.UpgradeToWriterLock(timeout);
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
    private System.Threading.ReaderWriterLock m_obj = null;
    /************************ Static *****************************************/
  } /* End of Class - ReaderWriterLock */
}
