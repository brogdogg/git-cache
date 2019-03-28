/******************************************************************************
 * File...: 
 * Remarks: 
 */
using git_cache.Git.LFS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace git_cache_mstest.Git.LFS
{
  /************************** LFSErrorUnitTest *******************************/
  /// <summary>
  /// 
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
    /// 
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