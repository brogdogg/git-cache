/******************************************************************************
 * File...: IAsyncReaderWriterLock.cs
 * Remarks: Definition of the async reader/writer lock interface.
 */
using System;
using System.Threading;
using System.Threading.Tasks;

namespace git_cache.Services.ResourceLock
{

  /// <summary>
  /// A simple wrapper around a `IDisposable` lock to allow for
  /// asynchronous disposal.
  /// </summary>
  public class AsyncDisposable : IAsyncDisposable
  {
    /*------------------------- AsyncDisposable -------------------------------*/
    /// <summary>
    /// Initializes a new instance of the <see cref="AsyncDisposable"/> class.
    /// </summary>
    /// <param name="lock">The lock to be disposed asynchronously.</param>
    /// <exception cref="ArgumentNullException">Thrown when the lock is null.</exception>
    public AsyncDisposable(IDisposable @lock)
    {
      _lock = @lock ?? throw new ArgumentNullException(nameof(@lock));
    }

    /*------------------------ DisposeAsync ----------------------------------*/
    /// <inheritdoc />
    public ValueTask DisposeAsync()
    {
      _lock.Dispose();
      // Since the lock is synchronous, we can return a completed ValueTask.
      return ValueTask.CompletedTask;
    }

    private readonly IDisposable _lock;
  }

  /************************** IAsyncReaderWriterLock *************************/
  /// <summary>
  /// Describes the basics of a reader/writer lock implementation that works
  /// with asynchronous operations.
  /// </summary>
  public interface IAsyncReaderWriterLock
  {
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Methods ****************************************/
    /*----------------------- ReaderLockAsync -------------------------------*/
    /// <summary>
    /// Acquires a reader lock asynchronously.
    /// </summary>
    /// <param name="timeout">
    /// The maximum time to wait for the lock.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token to observe while waiting for the lock.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result
    /// is an <see cref="IAsyncDisposable"/> that releases the lock when
    /// disposed.
    /// </returns>
    ValueTask<IAsyncDisposable> ReaderLockAsync(
        TimeSpan timeout,
        CancellationToken cancellationToken = default);

    /*----------------------- WriterLockAsync -------------------------------*/
    /// <summary>
    /// Acquires a writer lock asynchronously.
    /// </summary>
    /// <param name="timeout">
    /// The maximum time to wait for the lock.
    /// </param>
    /// <param name="cancellationToken">
    /// A cancellation token to observe while waiting for the lock.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result
    /// is an <see cref="IAsyncDisposable"/> that releases the lock when
    /// disposed.
    /// </returns>
    ValueTask<IAsyncDisposable> WriterLockAsync(
        TimeSpan timeout,
        CancellationToken cancellationToken = default);
  } /* End of Interface - IAsyncReaderWriterLock */
}
/* End of document - IAsyncReaderWriterLock.cs */
