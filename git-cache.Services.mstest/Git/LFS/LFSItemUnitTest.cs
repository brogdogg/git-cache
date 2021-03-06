﻿/******************************************************************************
 * File...: LFSItemUnitTest.cs
 * Remarks: 
 */
using git_cache.Services.Git.LFS;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace git_cache.Services.mstest.Git.LFS
{
  /************************** LFSItemUnitTest ********************************/
  /// <summary>
  /// Tests the <see cref="LFSItem"/> object
  /// </summary>
  [TestClass]
  public class LFSItemUnitTest
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /*----------------------- DefaultsGetsSets ------------------------------*/
    /// <summary>
    /// Verifies the <see cref="Item"/> constructs with correct defaults
    /// and the properties are capable of getting/setting
    /// </summary>
    [TestMethod]
    public void DefaultsGetsSets()
    {
      var item = new Item();
      Assert.AreEqual(null, item.OID);
      Assert.AreEqual(0, item.Size);
      item.OID = "test";
      Assert.AreEqual("test", item.OID);
      item.Size = 1024;
      Assert.AreEqual(1024, item.Size);
    } /* End of Function - DefaultsGetsSets */
    /************************ Fields *****************************************/
    /************************ Static *****************************************/
  } /* End of Class - LFSItemUnitTest */
}
/* End of document - LFSItemUnitTest.cs */