/******************************************************************************
 * File...: AsyncReaderWriterLockUnitTest.cs
 * Remarks: Unit tests for the async reader/writer lock object.
 */
using System;
using System.Threading.Tasks;

using git_cache.Services.ResourceLock;

using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using NSubstitute;

namespace git_cache.Services.mstest.ResourceLock
{
  [TestClass]
  public class AsyncReaderWriterLockUnitTest
  {

    /// <summary>
    /// Tests the <see cref="AsyncDisposable"/> object.
    /// </summary>
    /// <returns></returns>
    [TestMethod]
    public async Task AsyncDisposableWrapper()
    {
      var disposable = Substitute.For<IDisposable>();
      var asyncDisposable = new AsyncDisposable(disposable);
      await asyncDisposable.DisposeAsync();
      disposable.Received().Dispose();
    }

    /// <summary>
    /// Verifies the constructor throws when the logger is null.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AsyncDisposableThrowsForInvalidLock()
    {
      _ = new AsyncDisposable(null);
    }

    /// <summary>
    /// Verifies the constructor throws when the logger is null.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ThrowsForInvalidLogger()
    {
      _ = new AsyncReaderWriterLock(null);
    }

    /// <summary>
    /// Verifies the simple creation of the <see cref="AsyncReaderWriterLock"/>
    /// and obtaining a read lock on the resource.
    /// </summary>
    [TestMethod]
    public async Task AsyncReaderWriterLockReadLockTest()
    {
      var logger = Substitute.For<ILogger<AsyncReaderWriterLock>>();
      var lockObject = new AsyncReaderWriterLock(logger);
      var readLock = await lockObject.ReaderLockAsync(TimeSpan.Zero, default);
    }

    /// <summary>
    /// Verifies the simple creation of the <see cref="AsyncReaderWriterLock"/>
    /// and obtaining a write lock on the resource.
    /// </summary>
    [TestMethod]
    public async Task AsyncReaderWriterLockWriteLockTest()
    {
      var logger = Substitute.For<ILogger<AsyncReaderWriterLock>>();
      var lockObject = new AsyncReaderWriterLock(logger);
      var readLock = await lockObject.WriterLockAsync(TimeSpan.Zero, default);
    }

  }
}
