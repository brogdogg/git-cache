/******************************************************************************
 * File...: ResourceLockManagerUnitTest.cs
 * Remarks: 
 */
using git_cache.Services.ResourceLock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace git_cache.Services.mstest.ResourceLock
{
  /************************** ResourceLockManagerUnitTest ********************/
  /// <summary>
  /// 
  /// </summary>
  [TestClass]
  public class ResourceLockManagerUnitTest
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /*----------------------- ThrowsWithNullFactory -------------------------*/
    /// <summary>
    /// Verifies constructing a <see cref="ResourceLockManager{TKey}"/> with
    /// a null-valued factory causes an exception to be thrown.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ThrowsWithNullFactory()
    {
      var mgr = new ResourceLockManager<string>(null);
    } /* End of Function - ThrowsWithNullFactory */

    /*----------------------- DisposesResources -----------------------------*/
    /// <summary>
    /// Verifies the behavior of dispose, which should handle multiple calls
    /// to dispose.
    /// </summary>
    [TestMethod]
    public void DisposesResources()
    {
      var mgr = new ResourceLockManager<int>(Substitute.For<IResourceLockFactory>());
      mgr.Dispose();
      mgr.Dispose();
    } /* End of Function - DisposesResources */

    /*----------------------- GetForNewItem ---------------------------------*/
    /// <summary>
    /// 
    /// </summary>
    [TestMethod]
    public void GetForNewItem()
    {
      // SETUP
      // We will need a mock factory
      var factory = Substitute.For<IResourceLockFactory>();
      // And a controlled WaitHandle type to return
      var lockObj = Substitute.For<IResourceLock>();
      factory.Create().Returns(lockObj);
      // Create the resource lock manager
      var mgr = new ResourceLockManager<int>(factory);

      // ACT
      var item = mgr.GetFor(0);

      // ASSERT
      // We should have gotten the item we stated to return
      Assert.AreEqual(item, lockObj);
      // And the factory should have been called once
      factory.Received(1).Create();
    } /* End of Function - GetForNewItem */

    /*----------------------- GetForNewItemWithMultiThread ------------------*/
    /// <summary>
    /// 
    /// </summary>
    [TestMethod]
    public void GetForNewItemWithMultiThread()
    {
      // SETUP
      // We will need a mock factory
      var factory = Substitute.For<IResourceLockFactory>();
      // And a controlled WaitHandle type to return
      var lockObj = Substitute.For<IResourceLock>();
      ManualResetEvent continueEvent = new ManualResetEvent(false);
      // Our factory create will wait for a control signal to return
      // while things get setup
      factory.Create().Returns((callInfo) =>
      {
        if (!continueEvent.WaitOne(TimeSpan.FromSeconds(20)))
          throw new TimeoutException("Failed to get event, not expected!");
        return lockObj;
      });
      // Create the resource lock manager
      var mgr = new ResourceLockManager<int>(factory);

      // ACT
      var t1 = Task<IResourceLock>.Factory.StartNew(() => mgr.GetFor(0));
      var t2 = Task<IResourceLock>.Factory.StartNew(() => mgr.GetFor(0));
      var t3 = Task<IResourceLock>.Factory.StartNew(() => mgr.GetFor(0));
      var t4 = Task<bool>.Factory.StartNew(() =>
      {
        Thread.Sleep(500);
        continueEvent.Set();
        return true;
      });

      Task.WaitAll(t1, t2, t3, t4);

      // ASSERT
      // We should have gotten the item we stated to return
      Assert.AreEqual(t1.Result, lockObj);
      Assert.AreEqual(t2.Result, lockObj);
      Assert.AreEqual(t3.Result, lockObj);
      // And the factory should have been called once
      factory.Received(1).Create();

    } /* End of Function - GetForNewItemWithMultiThread */

    /*----------------------- BlockFor --------------------------------------*/
    /// <summary>
    /// Verifies the basic operation of BlockFor
    /// </summary>
    [TestMethod]
    public void BlockFor()
    {
      // SETUP
      // Specify the time to wait
      var waitTime = TimeSpan.FromMilliseconds(0);
      // Create our fakes
      var lockObj = Substitute.For<IResourceLock>();
      lockObj.WaitOne(waitTime).Returns(true);
      var factory = Substitute.For<IResourceLockFactory>();
      factory.Create().Returns(lockObj);
      // Create the resource lock manager to work with
      var mgr = new ResourceLockManager<int>(factory);

      // ACT
      // Block for item 0, which should create a new one
      var item = mgr.BlockFor(0, waitTime);

      // ASSERT
      Assert.AreEqual(item, lockObj);
      factory.Received(1).Create();
    } /* End of Function - BlockFor */

    /*----------------------- BlockForTimeout -------------------------------*/
    /// <summary>
    /// Verifies an exception is thrown when failed to get lock in specified
    /// amount of time.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(TimeoutException))]
    public void BlockForTimeout()
    {
      // SETUP
      TimeSpan waitTime = TimeSpan.FromMilliseconds(100);
      // Create a pretend handle, so we can pretend a timeout
      // occurred.
      var mockWaitHandle = Substitute.For<IResourceLock>();
      mockWaitHandle.WaitOne(waitTime).Returns(false);
      // Setup our fake factory to return our fake wait handle
      var factory = Substitute.For<IResourceLockFactory>();
      factory.Create().Returns(mockWaitHandle);
      // Finally we will need our resource lock manager to work with
      var mgr = new ResourceLockManager<int>(factory);

      // ACT
      var item = mgr.BlockFor(0, waitTime);

      // ASSERT
      // Should throw exception
    } /* End of Function - BlockForTimeout */
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
    /************************ Static *****************************************/

  } /* End of Class - ResourceLockManagerUnitTest */
}
/* End of document - ResourceLockManagerUnitTest.cs */