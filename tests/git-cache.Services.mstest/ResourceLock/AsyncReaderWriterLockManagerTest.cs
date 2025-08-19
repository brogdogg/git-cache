/******************************************************************************
 * File...: AsyncReaderWriterLockManagerUnitTest.cs
 * Remarks: Unit tests for the async reader/writer lock object.
 */
using System;

using git_cache.Services.ResourceLock;

using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using NSubstitute;

namespace git_cache.Services.mstest.ResourceLock
{
  /// <summary>
  /// Unit tests for the <see cref="AsyncReaderWriterLockManager"/> class.
  /// </summary>
  [TestClass]
  public class AsyncReaderWriterLockManagerTest
  {
    /// <summary>
    /// Verifies the manager can return a resource lock for a given key
    /// </summary>
    [TestMethod]
    public void GetResourceLockForKey()
    {
      var logger = Substitute.For<ILogger<AsyncReaderWriterLockManager<string>>>();
      var serviceProvider = Substitute.For<IServiceProvider>();
      var myLock = Substitute.For<IAsyncReaderWriterLock>();
      serviceProvider.GetService(typeof(IAsyncReaderWriterLock)).Returns(myLock);

      var mgr = new AsyncReaderWriterLockManager<string>(serviceProvider, logger);

      var lockObj = mgr.GetFor("test1");
      Assert.IsNotNull(lockObj);
      var lockObj1 = mgr.GetFor("test1");
      Assert.AreEqual(lockObj, lockObj1);
      serviceProvider.Received(1).GetService(typeof(IAsyncReaderWriterLock));
    }

    /// <summary>
    /// Verifies the <see cref="AsyncReaderWriterLockManager{TKey}"/> constructor
    /// throws when the logger argument is null.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ThrowsForNullLogger()
    {
      _ = new AsyncReaderWriterLockManager<string>(Substitute.For<IServiceProvider>(), null);
    }

    /// <summary>
    /// Verifies the <see cref="AsyncReaderWriterLockManager{TKey}"/> constructor
    /// throws when the service provider argument is null.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ThrowsForNullServiceProvider()
    {
      _ = new AsyncReaderWriterLockManager<string>(
        null, Substitute.For<ILogger<AsyncReaderWriterLockManager<string>>>());
    }

  }

}
