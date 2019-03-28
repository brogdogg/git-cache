/******************************************************************************
 * File...: BatchRequestObjectUnitTest.cs
 * Remarks: 
 */
using git_cache.Git.LFS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace git_cache_mstest.Git.LFS
{
  /************************** BatchRequestObjectUnitTest *********************/
  /// <summary>
  /// Verifies the behavior of the <see cref="BatchRequestObject"/> class
  /// </summary>
  [TestClass]
  public class BatchRequestObjectUnitTest
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /*----------------------- DefaultsGetsSets ------------------------------*/
    /// <summary>
    /// Verifies the constructor defaults and gets/sets of the object
    /// </summary>
    [TestMethod]
    public void DefaultsGetsSets()
    {
      var obj = new BatchRequestObject();
      Assert.IsNull(obj.Objects);
      Assert.IsNull(obj.Ref);
      Assert.IsNull(obj.Transfers);
      Assert.IsNull(obj.Operation);
      Assert.IsNotNull((obj.Objects = new List<BatchRequestObject.BatchItemObject>()));
      Assert.AreEqual("op", (obj.Operation = "op"));
      Assert.IsNotNull((obj.Transfers = new List<string>()));
      Assert.IsNotNull((obj.Ref = new BatchRequestObject.BatchRefObject()));
    } /* End of Function - DefaultsGetsSets */
    /************************ Fields *****************************************/
    /************************ Static *****************************************/
  } /* End of Class - BatchRequestObjectUnitTest */

  /************************** BatchRefObjUnitTest ****************************/
  /// <summary>
  /// Verifies the behavior of <see cref="BatchRequestObject.BatchRefObject"/>
  /// </summary>
  [TestClass]
  public class BatchRefObjUnitTest
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /*----------------------- DefaultsGetsSets ------------------------------*/
    /// <summary>
    /// Verifies the constructor defaults and gets/sets of the class
    /// </summary>
    [TestMethod]
    public void DefaultsGetsSets()
    {
      var obj = new BatchRequestObject.BatchRefObject();
      Assert.IsNull(obj.Name);
      Assert.AreEqual("name", (obj.Name = "name"));
    } /* End of Function - DefaultsGetsSets */
    /************************ Fields *****************************************/
    /************************ Static *****************************************/
  } /* End of Class - BatchRefObjUnitTest */

  /************************** BatchItemObjectUnitTest ************************/
  /// <summary>
  /// Verifies the behavior of <see cref="BatchRequestObject.BatchItemObject"/>
  /// </summary>
  [TestClass]
  public class BatchItemObjectUnitTest
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /*----------------------- DefaultsGetsSets ------------------------------*/
    /// <summary>
    /// Verifies the constructor defaults and gets/sets
    /// </summary>
    [TestMethod]
    public void DefaultsGetsSets()
    {
      var obj = new BatchRequestObject.BatchItemObject();
      Assert.IsNull(obj.OID);
      Assert.AreEqual(0, obj.Size);
      Assert.AreEqual("test", (obj.OID = "test"));
      Assert.AreEqual(1024, (obj.Size = 1024));
    } /* End of Function - DefaultsGetsSets */
    /************************ Fields *****************************************/
    /************************ Static *****************************************/
  } /* End of Class - BatchItemObjectUnitTest */
}
/* End of document - BatchRequestObjectUnitTest.cs */