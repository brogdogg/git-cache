/******************************************************************************
 * File...: ReaderWriterLockUnitTest.cs
 * Remarks: 
 */
using git_cache.Services.ResourceLock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;

namespace git_cache.Services.mstest.ResourceLock
{
  /*========================= ReaderWriterLockUnitTest ======================*/
  /// <summary>
  /// Verifies the behavior of the <see cref="ReaderWriterLock"/> wrapper
  /// class
  /// </summary>
  [TestClass]
  public class ReaderWriterLockUnitTest
  {
    /************************ PUBLIC *****************************************/
    /************************ Types ******************************************/
    /************************ Static Values **********************************/
    /************************ Fields *****************************************/
    /************************ Properties *************************************/
    /************************ Construction/Destruction ***********************/
    /************************ Methods ****************************************/
    /*----------------------- CanAcquireReadLock ----------------------------*/
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [TestMethod]
    public void CanAcquireReadLock()
    {
      // Setup
      var l = new ReaderWriterLockSlim();
      // Act
      l.AcquireReaderLock(100);
      Assert.IsTrue(l.IsReaderLockHeld);
      l.ReleaseReaderLock();
      Assert.IsFalse(l.IsReaderLockHeld);
    } // end of function - CanAcquireReadLock

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ThrowsOnUpgradeWithoutRead()
    {
      // Setup
      var l = new ReaderWriterLockSlim();
      // Act/Verify
      l.UpgradeToWriterLock(TimeSpan.MinValue);
    }
    /*----------------------- ThrowsOnTimeoutAttemptingUpgrade --------------*/
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [TestMethod]
    public void ThrowsOnTimeoutAttemptingUpgrade()
    {
      // Setup
      var l = new ReaderWriterLockSlim();
      var ready = new System.Threading.ManualResetEvent(false);
      var meToo = new System.Threading.ManualResetEvent(false);
      // Go ahead and grab the reader lock
      Debug.WriteLine($"Thread ID: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
      System.Threading.Thread t = new System.Threading.Thread((flObj) =>
      {
        var fl = (ReaderWriterLockSlim)flObj;
        Debug.WriteLine($"(t)Thread ID: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
        try
        {
          fl.AcquireReaderLock(0);
          ready.Set();
          meToo.WaitOne();
          fl.UpgradeToWriterLock(0);
          System.Threading.Thread.Sleep(1000);
          fl.DowngradeFromWriterLock();
          fl.ReleaseReaderLock();
        }
        catch { ready.Set(); }
      });
      bool didThrow = false;
      System.Threading.Thread t1 = new System.Threading.Thread((flObj) =>
      {
        var fl = (ReaderWriterLockSlim)flObj;
        Debug.WriteLine($"(t1)Thread ID: {System.Threading.Thread.CurrentThread.ManagedThreadId}");
        ready.WaitOne();
        fl.AcquireReaderLock(0);
        meToo.Set();
        if(ready.WaitOne(0))
        {
          try { fl.UpgradeToWriterLock(0); }
          catch (TimeoutException) { didThrow = true; }
        }
      });
      t.Start(l);
      t1.Start(l);
      // Act
      t.Join();
      t1.Join();
      // Verify
      Assert.IsTrue(didThrow);
    } // end of function - ThrowsOnTimeoutAttemptingUpgrade

    /*----------------------- StillInReadLockAfterDowngrading ---------------*/
    /// <summary>
    /// Verifies the state of the reader/writer when upgrading to a writer
    /// and back down to a reader.
    /// </summary>
    [TestMethod]
    public void StillInReadLockAfterDowngrading()
    {
      // Setup
      var l = new ReaderWriterLockSlim();
      // Act/Verify
      l.AcquireReaderLock(0);
      Assert.IsTrue(l.IsReaderLockHeld);
      l.UpgradeToWriterLock(0);
      Assert.IsTrue(l.IsWriterLockHeld);
      Assert.IsTrue(l.IsReaderLockHeld);
      l.DowngradeFromWriterLock();
      Assert.IsFalse(l.IsWriterLockHeld);
      Assert.IsTrue(l.IsReaderLockHeld);
    } // end of function - StillInReadLockAfterDowngrading
    /************************ Static Functions *******************************/

    /************************ PROTECTED **************************************/
    /************************ Types ******************************************/
    /************************ Static Values **********************************/
    /************************ Fields *****************************************/
    /************************ Properties *************************************/
    /************************ Methods ****************************************/
    /************************ Static Functions *******************************/

    /************************ PRIVATE ****************************************/
    /************************ Types ******************************************/
    /************************ Static Values **********************************/
    /************************ Fields *****************************************/
    /************************ Properties *************************************/
    /************************ Methods ****************************************/
    /************************ Static Functions *******************************/

  } // end of class - ReaderWriterLockUnitTest

}
/*............................. End of ReaderWriterLockUnitTest.cs ..........*/
