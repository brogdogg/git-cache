/******************************************************************************
 * File...: LFSErrorUnitTest.cs
 * Remarks: 
 */
using git_cache.Services.Git.LFS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace git_cache.Services.mstest.Git.LFS
{
  /************************** LFSErrorUnitTest *******************************/
  /// <summary>
  /// Tests the <see cref="LFSError"/> object
  /// </summary>
  [TestClass]
  public class LFSErrorUnitTest
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /*----------------------- DefaultsGetsSets ------------------------------*/
    /// <summary>
    /// Verifies we can construct with defaults and get/set values for
    /// properties
    /// </summary>
    [TestMethod]
    public void DefaultsGetsSets()
    {
      var error = new LFSError();
      Assert.AreEqual(0, error.Code);
      Assert.AreEqual("", error.Message);
      error.Code = 500;
      error.Message = "error";
      Assert.AreEqual(500, error.Code);
      Assert.AreEqual("error", error.Message);
    } /* End of Function - DefaultsGetsSets */
    /************************ Fields *****************************************/
    /************************ Static *****************************************/
  } /* End of Class - LFSErrorUnitTest */
}
/* End of document - LFSErrorUnitTest.cs */