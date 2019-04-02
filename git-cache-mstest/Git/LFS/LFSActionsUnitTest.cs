/******************************************************************************
 * File...: LFSActionsUnitTest.cs
 * Remarks: 
 */
using git_cache.Git.LFS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace git_cache_mstest.Git.LFS
{
  /************************** LFSActionsUnitTest *****************************/
  /// <summary>
  /// Verifies the behavior of <see cref="LFSActions"/> object
  /// </summary>
  [TestClass]
  public class LFSActionsUnitTest
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /*----------------------- DefaultsGetsSets ------------------------------*/
    /// <summary>
    /// Verifies the default construtor behavior and get/set
    /// </summary>
    [TestMethod]
    public void DefaultsGetsSets()
    {
      var actions = new LFSActions();
      Assert.IsNull(actions.Download);
      Assert.IsNull(actions.Upload);
      Assert.IsNull(actions.Verify);

      var download = new LFSActions.DownloadAction();
      var upload = new LFSActions.UploadAction();
      var verify = new LFSActions.VerifyAction();
      actions.Download = download;
      actions.Upload = upload;
      actions.Verify = verify;
      Assert.AreEqual(download, actions.Download);
      Assert.AreEqual(upload, actions.Upload);
      Assert.AreEqual(verify, actions.Verify);
    } /* End of Function - DefaultsGetsSets */
    /************************ Fields *****************************************/
    /************************ Static *****************************************/
    /************************ Types ******************************************/
    public static class LFSActionBase<T>
      where T : IAction
    {
      public static void TestDefaultsGetsSets()
      {
        var action = Activator.CreateInstance<T>();
        Assert.IsNull(action.ExpiresAt);
        Assert.AreEqual(0, action.ExpiresIn);
        Assert.IsNull(action.Header);
        Assert.IsNull(action.HREF);
      }

    }
  } /* End of Class - LFSActionsUnitTest */

  /// <summary>
  /// 
  /// </summary>
  [TestClass]
  public class LFSActionsDownloadTest
  {
    /*----------------------- DefaultsGetsSets ------------------------------*/
    /// <summary>
    /// 
    /// </summary>
    [TestMethod]
    public void DefaultsGetsSets()
    {
      LFSActionsUnitTest.LFSActionBase<LFSActions.DownloadAction>.TestDefaultsGetsSets();
    } /* End of Function - DefaultsGetsSets */
  }

  [TestClass]
  public class LFSActionsUploadTest
  {
    /*----------------------- DefaultsGetsSets ------------------------------*/
    /// <summary>
    /// 
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(NotImplementedException))]
    public void DefaultsGetsSets()
    {

      LFSActionsUnitTest.LFSActionBase<LFSActions.UploadAction>.TestDefaultsGetsSets();
    } /* End of Function - DefaultsGetsSets */
  }
  [TestClass]
  public class LFSActionsVerifyTest
  {
    /*----------------------- DefaultsGetsSets ------------------------------*/
    /// <summary>
    /// 
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(NotImplementedException))]
    public void DefaultsGetsSets()
    {
      LFSActionsUnitTest.LFSActionBase<LFSActions.VerifyAction>.TestDefaultsGetsSets();
    } /* End of Function - DefaultsGetsSets */
  }
}
/* End of document - LFSActionsUnitTest.cs */