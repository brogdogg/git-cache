/******************************************************************************
 * File...: IReaderWriterLock.cs
 * Remarks: 
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace git_cache.Services.ResourceLock
{
  /************************** IReaderWriterLock ******************************/
  /// <summary>
  /// 
  /// </summary>
  public interface IReaderWriterLock
  {
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    bool IsReaderLockHeld { get; }
    bool IsWriterLockHeld { get; }
    int WriterSequenceNumber { get; }
    /************************ Methods ****************************************/
    void AcquireReaderLock(int msTimeout);
    void AcquireReaderLock(TimeSpan timeout);
    void AcquireWriterLock(int msTimeout);
    void AcquireWriterLock(TimeSpan timeout);
    bool AnyWritersSince(int seqNum);
    void DowngradeFromWriterLock(ref LockCookie lockCookie);
    LockCookie ReleaseLock();
    void ReleaseReaderLock();
    void ReleaseWriterLock();
    void RestoreLock(ref LockCookie lockCookie);
    LockCookie UpgradeToWriterLock(int msTimeout);
    LockCookie UpgradeToWriterLock(TimeSpan timeout);
  } /* End of Interface - IReaderWriterLock */
}
/* End of document - IReaderWriterLock.cs */