/******************************************************************************
 * File...: ReaderWriterLockFactoryUnitTest.cs
 * Remarks: 
 */
using git_cache.Services.ResourceLock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace git_cache.Services.mstest.ResourceLock
{
  /************************** ReaderWriterLockFactoryUnitTest ****************/
  /// <summary>
  /// Verifies the behavior of the <see cref="ReaderWriterLockFactory"/>
  /// class object.
  /// </summary>
  [TestClass]
  public class ReaderWriterLockFactoryUnitTest
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /*----------------------- Test ------------------------------------------*/
    /// <summary>
    /// Verifies the factory generates the correct object
    /// </summary>
    [TestMethod]
    public void CanCreateCorrectObject()
    {
      var factory = new ReaderWriterLockFactory<TestReaderWriterLock>();
      Assert.IsInstanceOfType(factory.Create(), typeof(TestReaderWriterLock));
    } /* End of Function - Test */
    /************************ Methods ****************************************/
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
    /************************ Types ******************************************/
    /// <summary>
    /// A test implementation of the <see cref="IReaderWriterLock"/>
    /// interface.
    /// </summary>
    private class TestReaderWriterLock : IReaderWriterLock
    {
      public bool IsReaderLockHeld => throw new NotImplementedException();
      public bool IsWriterLockHeld => throw new NotImplementedException();
      public void AcquireReaderLock(int msTimeout)
      {
        throw new NotImplementedException();
      }
      public void AcquireReaderLock(TimeSpan timeout)
      {
        throw new NotImplementedException();
      }
      public void DowngradeFromWriterLock()
      {
        throw new NotImplementedException();
      }
      public void ReleaseReaderLock()
      {
        throw new NotImplementedException();
      }
      public void ReleaseWriterLock()
      {
        throw new NotImplementedException();
      }
      public void UpgradeToWriterLock(int msTimeout)
      {
        throw new NotImplementedException();
      }
      public void UpgradeToWriterLock(TimeSpan timeout)
      {
        throw new NotImplementedException();
      }
    }
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /************************ Fields *****************************************/
    /************************ Static *****************************************/

  } /* End of Class - ReaderWriterLockFactoryUnitTest */
}
/* End of document - ReaderWriterLockFactoryUnitTest.cs */