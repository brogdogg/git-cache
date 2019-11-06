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
  /// Descibes the basics of a reader/writer lock implementation
  /// </summary>
  public interface IReaderWriterLock
  {
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /// <summary>
    /// Get a flag indicating if the reader lock is considered held by the
    /// current thread.
    /// </summary>
    bool IsReaderLockHeld { get; }

    /// <summary>
    /// Gets a flag indicating if the writer lock is considered held by the
    /// current thread.
    /// </summary>
    bool IsWriterLockHeld { get; }
    /************************ Methods ****************************************/

    /// <summary>
    /// Attempts to acquire the reader lock in the specified amount of time.
    /// </summary>
    /// <param name="msTimeout">
    /// Time in millseconds before timing out.
    /// </param>
    /// <exception cref="TimeoutException">
    /// Thrown if the reader lock is not acquired in the specified
    /// time.
    /// </exception>
    void AcquireReaderLock(int msTimeout);
    /// <summary>
    /// Attempts to acquire the reader lock in a specified time span.
    /// </summary>
    /// <param name="timeout">
    /// Timespan to acquire reader lock
    /// </param>
    /// <exception cref="TimeoutException">
    /// Thrown if the reader lock is not acquired in the specified
    /// time.
    /// </exception>
    void AcquireReaderLock(TimeSpan timeout);
    /// <summary>
    /// Downgrades from a writer lock to a reader lock.
    /// </summary>
    /// <exception cref="SynchronizationLockException">
    /// Thrown if the writer lock was not currently held to downgrade
    /// from or if the reader lock was not previously held.
    /// </exception>
    void DowngradeFromWriterLock();
    /// <summary>
    /// Releases the reader lock, which must have been previously held.
    /// </summary>
    /// <exception cref="SynchronizationLockException">
    /// Thrown if the reader lock is not held.
    /// </exception>
    void ReleaseReaderLock();
    /// <summary>
    /// Releases the writer lock, which must have been previously held.
    /// </summary>
    /// <exception cref="SynchronizationLockException">
    /// Thrown if the writer lock is not held.
    /// </exception>
    void ReleaseWriterLock();
    /// <summary>
    /// Upgrades from a reader lock to a writer lock, within the specified
    /// timeout.
    /// </summary>
    /// <param name="msTimeout">
    /// Time to wait in milliseconds before timing out.
    /// </param>
    /// <exception cref="TimeoutException">
    /// Thrown if the writer lock was not acquired in the timeout period.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the reader lock is not already held and trying to upgrade
    /// </exception>
    void UpgradeToWriterLock(int msTimeout);
    /// <summary>
    /// Upgrades from a reader lock to a writer lock, within the specified
    /// timespan
    /// </summary>
    /// <param name="timeout">
    /// Time to wait before timing out
    /// </param>
    /// <exception cref="TimeoutException">
    /// Thrown after the timespan has passed and the upgrade has not occured
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the reader lock is not already held and trying to upgrade
    /// </exception>
    void UpgradeToWriterLock(TimeSpan timeout);
  } /* End of Interface - IReaderWriterLock */
}
/* End of document - IReaderWriterLock.cs */