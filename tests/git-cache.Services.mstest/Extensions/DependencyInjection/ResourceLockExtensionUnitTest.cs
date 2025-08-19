/******************************************************************************
 * File...: ResourceLockExtensionUnitTest.cs
 * Remarks: Unit tests for the resource lock extension DI extensions.
 */
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
    /// Verifies the correct services are added for async resource reader/writer
    /// </summary>
    [TestMethod]
    public void AddAsyncResourceLocks()
    {
      // SETUP
      // ACT
      Services.AddAsyncResourceLocks();
      var serviceProvider = Services.BuildServiceProvider();

      // VERIFY
      Assert.IsInstanceOfType(
        serviceProvider.GetService<IAsyncReaderWriterLockManager<string>>(),
        typeof(AsyncReaderWriterLockManager<string>));

      // Verify the reader/writer lock adds.
      Assert.IsInstanceOfType(
        serviceProvider.GetService<IAsyncReaderWriterLock>(),
        typeof(AsyncReaderWriterLock));
    } /* End of Function - AddResourceLocks */
    /************************ Fields *****************************************/
    /************************ Static *****************************************/
  } /* End of Class - ResourceLockExtensionUnitTest */
}
/* End of document - ResourceLockExtensionUnitTest.cs */
