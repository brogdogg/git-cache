/******************************************************************************
 * File...: ResourceLockExtensionUnitTest.cs
 * Remarks: 
 */
using System;
using git_cache.Services.Extensions.DependencyInjection;
using git_cache.Services.ResourceLock;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace git_cache.Services.mstest.Extensions.DependencyInjection
{
  /************************** ResourceLockExtensionUnitTest ******************/
  /// <summary>
  /// Verifies the service extension method for adding services
  /// for resource locks works as expected.
  /// </summary>
  [TestClass]
  public class ResourceLockExtensionUnitTest : BaseExtensionUnitTest
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /*----------------------- AddResourceLocks ------------------------------*/
    /// <summary>
    /// Verifies the correct services are added for resource locks
    /// </summary>
    [TestMethod]
    public void AddResourceLocks()
    {
      // SETUP
      // ACT
      Services.AddResourceLocks();
      var serviceProvider = Services.BuildServiceProvider();

      // VERIFY
      Assert.IsInstanceOfType(
        serviceProvider.GetService<IResourceLockFactory>(),
        typeof(ResourceLockFactory<Services.ResourceLock.ResourceLock>));

      Assert.IsInstanceOfType(
        serviceProvider.GetService<IResourceLockManager<string>>(),
        typeof(ResourceLockManager<string>));

      Assert.IsInstanceOfType(
        serviceProvider.GetService<IResourceLockFactory<FakeResourceLock>>(),
        typeof(ResourceLockFactory<FakeResourceLock>));
    } /* End of Function - AddResourceLocks */
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
    /************************ Types ******************************************/

    /************************ FakeResourceLock *******************************/
    /// <summary>
    /// Fake class for testing
    /// </summary>
    public class FakeResourceLock : IResourceLock
    {
      public void Dispose() => throw new NotImplementedException();
      public void Release() => throw new NotImplementedException();
      public bool WaitOne() => throw new NotImplementedException();
      public bool WaitOne(int milliseconds) => throw new NotImplementedException();
      public bool WaitOne(TimeSpan timeout) => throw new NotImplementedException();
    } /* End of Class - FakeResourceLock */

  } /* End of Class - ResourceLockExtensionUnitTest */
}
/* End of document - ResourceLockExtensionUnitTest.cs */