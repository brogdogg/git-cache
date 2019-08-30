/******************************************************************************
 * File...: ResourceLockUnitTest.cs
 * Remarks: 
 */
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Threading.Tasks;

namespace git_cache.Services.mstest.ResourceLock
{
  /************************** ResourceLockUnitTest ***************************/
  /// <summary>
  /// Tests the operations of <see cref="Services.ResourceLock.ResourceLock"/>
  /// </summary>
  [TestClass]
  public class ResourceLockUnitTest
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /*----------------------- ConstructDispose ------------------------------*/
    /// <summary>
    /// Verifies we can construct and dispose of a
    /// <see cref="Services.ResourceLock.ResourceLock"/> object.
    /// </summary>
    [TestMethod]
    public void ConstructDispose()
    {
      using (var item = new Services.ResourceLock.ResourceLock())
      {; }
    } /* End of Function - ConstructDispose */

    /*----------------------- WaitAndRelease --------------------------------*/
    /// <summary>
    /// Verifies we can wait
    /// </summary>
    [TestMethod]
    public void WaitAndRelease()
    {
      var obj = new AutoResetEvent(false);
      using (var item = new Services.ResourceLock.ResourceLock(obj))
      {
        // But on a different thread, no go
        Assert.IsFalse(Task<bool>.Factory.StartNew(() => item.WaitOne(0)).Result);
        // Until we release it here
        item.Release();
        // Then we should be able to get it.
        Assert.IsTrue(Task<bool>.Factory.StartNew(() => item.WaitOne(0)).Result);
        item.Release();
      } // end of using - resource
    } /* End of Function - WaitAndRelease */
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

  } /* End of Class - ResourceLockUnitTest */
}
/* End of document - ResourceLockUnitTest.cs */