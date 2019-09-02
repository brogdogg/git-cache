/******************************************************************************
 * File...: ResourceLockFactoryUnitTest.cs
 * Remarks: 
 */
using git_cache.Services.ResourceLock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace git_cache.Services.mstest.ResourceLock
{
  /************************** ResourceLockFactoryUnitTest ********************/
  /// <summary>
  /// Verifies the behavior of the <see cref="ResourceLockFactory{TLock}"/>
  /// object.
  /// </summary>
  [TestClass]
  public class ResourceLockFactoryUnitTest
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /*----------------------- CanCreate -------------------------------------*/
    /// <summary>
    /// Verifies the resource lock factory can create a lock object
    /// </summary>
    [TestMethod]
    public void CanCreate()
    {
      var factory = new ResourceLockFactory<TestResourceLock>();
      var lockObj = factory.Create();
      Assert.IsNotNull(lockObj);
      Assert.IsInstanceOfType(lockObj, typeof(TestResourceLock));
    } /* End of Function - CanCreate */
    /************************ Fields *****************************************/
    /************************ Static *****************************************/

    /************************ TestResourceLock *******************************/
    /// <summary>
    /// Test class for testing the factory
    /// </summary>
    private class TestResourceLock : IResourceLock
    {
      /*===================== PUBLIC ========================================*/
      /********************** Events *****************************************/
      /********************** Properties *************************************/
      /********************** Construction ***********************************/
      /********************** Methods ****************************************/
      public void Dispose() => throw new NotImplementedException();
      public void Release() => throw new NotImplementedException();
      public bool WaitOne() => throw new NotImplementedException();
      public bool WaitOne(int milliseconds) => throw new NotImplementedException();
      public bool WaitOne(TimeSpan timeout) => throw new NotImplementedException();
      /********************** Fields *****************************************/
      /********************** Static *****************************************/
    } /* End of Class - TestResourceLock */
  } /* End of Class - ResourceLockFactoryUnitTest */
}
/* End of document - ResourceLockFactoryUnitTest.cs */