/******************************************************************************
 * File...: AsyncReaderWriterLock.cs
 * Remarks: Wrapper class for Nito.AsyncEx.AsyncReaderWriterLock
 *          to provide an async-compatible reader/writer lock.
 */
using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

namespace git_cache.Services.ResourceLock
{
  /************************** AsyncReaderWriterLock **************************/
  /// <summary>
  /// Implementation of the <see cref="IAsyncReaderWriterLock"/> interface
  /// for asynchronous operations.
  /// </summary>
  /// <remarks>
  /// This class uses Nito.AsyncEx.AsyncReaderWriterLock to provide asynchronous
  /// reader/writer locking capabilities.
  /// </remarks>
  /// <seealso cref="IAsyncReaderWriterLock"/>
  public class AsyncReaderWriterLock : IAsyncReaderWriterLock
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /*----------------------- AsyncReaderWriterLock -------------------------*/
    /// <summary>
    /// Initializes a new instance of the <see cref="AsyncReaderWriterLock"/> class.
    /// </summary>
    /// <param name="logger">
    /// The logger to use for logging operations.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the <paramref name="logger"/> is null.
    /// </exception>
    public AsyncReaderWriterLock(ILogger<AsyncReaderWriterLock> logger)
    {
      _logger = logger ?? throw new ArgumentNullException(nameof(logger));
      // Initialize the async lock.
      // Nito.AsyncEx.AsyncReaderWriterLock is a popular library for async locks.
      // Ensure you have the Nito.AsyncEx package installed.
      _asyncLock = new Nito.AsyncEx.AsyncReaderWriterLock();
    } // end of function - AsyncReaderWriterLock

    /************************ Methods ****************************************/
    /*----------------------- ReaderLockAsync -------------------------------*/
    /// <inheritdoc />
    public async ValueTask<IAsyncDisposable> ReaderLockAsync(
      TimeSpan timeout,
      CancellationToken cancellationToken = default)
    {
      using var joinedCancellationToken = CreateLinkedCancellationTokenSource(cancellationToken, timeout);
      var result = await _asyncLock.ReaderLockAsync(joinedCancellationToken.Token);
      return new AsyncDisposable(result);
    }

    /*----------------------- WriterLockAsync -------------------------------*/
    /// <inheritdoc />
    public async ValueTask<IAsyncDisposable> WriterLockAsync(
      TimeSpan timeout,
      CancellationToken cancellationToken = default)
    {
      using var joinedCancellationToken = CreateLinkedCancellationTokenSource(cancellationToken, timeout);
      var result = await _asyncLock.WriterLockAsync(joinedCancellationToken.Token);
      return new AsyncDisposable(result);
    }
    /************************ Fields *****************************************/
    /************************ Static *****************************************/

    /*======================= PROTECTED =====================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    protected virtual CancellationTokenSource CreateLinkedCancellationTokenSource(
        CancellationToken cancellationToken, TimeSpan timeout)
    {
      var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
      linkedCts.CancelAfter(timeout);
      return linkedCts;
    } // end of function - CreateLinkedCancellationTokenSource

    /************************ Fields *****************************************/
    /************************ Static *****************************************/

    /*======================= PRIVATE =======================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /************************ Fields *****************************************/
    private readonly Nito.AsyncEx.AsyncReaderWriterLock _asyncLock;
    private readonly ILogger<AsyncReaderWriterLock> _logger;

    /************************ Static *****************************************/
  } /* End of Class - AsyncReaderWriterLock */
}
/* End of document - AsyncReaderWriterLock.cs */
