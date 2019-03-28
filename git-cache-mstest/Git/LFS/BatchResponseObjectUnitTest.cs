/******************************************************************************
 * File...: BatchResponseObjectUnitTest.cs
 * Remarks: 
 */
using git_cache.Git.LFS;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace git_cache_mstest.Git.LFS
{
  /************************** BatchResponseObjectUnitTest ********************/
  /// <summary>
  /// Verifies the behavior of the <see cref="BatchResponseObject"/> class
  /// </summary>
  [TestClass]
  public class BatchResponseObjectUnitTest
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /*----------------------- DefaultsGetSets -------------------------------*/
    /// <summary>
    /// Verifies the constructor defaults and gets/sets of properties
    /// </summary>
    [TestMethod]
    public void DefaultsGetSets()
    {
      var obj = new BatchResponseObject();
      Assert.IsNotNull(obj.Objects);
      Assert.AreEqual("basic", obj.Transfer);
      Assert.IsNull((obj.Objects = null));
      Assert.AreEqual("advanced", (obj.Transfer = "advanced"));
    } /* End of Function - DefaultsGetSets */
    /************************ Fields *****************************************/
    /************************ Static *****************************************/
  } /* End of Class - BatchResponseObjectUnitTest */

  /************************** ResponseLFSItemUnitTest ************************/
  /// <summary>
  /// Verifies the behavior of <see cref="BatchResponseObject.ResponseLFSItem"/>
  /// class
  /// </summary>
  [TestClass]
  public class ResponseLFSItemUnitTest
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /*----------------------- DefaultsGetsSets ------------------------------*/
    /// <summary>
    /// Verifies the constructed defaults for the class object
    /// </summary>
    [TestMethod]
    public void DefaultsGetsSets()
    {
      var obj = new BatchResponseObject.ResponseLFSItem();
      Assert.IsFalse(obj.Authenticated);
      Assert.IsNull(obj.Actions);
      Assert.IsNull(obj.Error);
      Assert.IsTrue((obj.Authenticated = true));
      Assert.IsNotNull((obj.Actions = new LFSActions()));
      Assert.IsNotNull((obj.Error = new LFSError()));
    } /* End of Function - DefaultsGetsSets */
    /************************ Fields *****************************************/
    /************************ Static *****************************************/
  } /* End of Class - ResponseLFSItemUnitTest */
}
/* End of document - BatchResponseObjectUnitTest.cs */