/******************************************************************************
 * File...: RefStatusUnitTest.cs
 * Remarks: 
 */
using git_cache.Services.Git.Status;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace git_cache.Services.mstest.Git.Status
{
  /*========================= RefStatusUnitTest =============================*/
  /// <summary>
  /// 
  /// </summary>
  /// <remarks></remarks>
  [TestClass]
  public class RefStatusUnitTest
  {
    /************************ PUBLIC *****************************************/
    /************************ Types ******************************************/
    /************************ Static Values **********************************/
    /************************ Fields *****************************************/
    /************************ Properties *************************************/
    /************************ Construction/Destruction ***********************/
    /************************ Methods ****************************************/
    /*----------------------- GettersSettersOhMy ----------------------------*/
    /// <summary>
    /// Verifies the getters/setters of the <see cref="RefStatus"/> class.
    /// </summary>
    [TestMethod]
    public void GettersSettersOhMy()
    {
      // Setup
      var from = "From-test";
      var to = "to-test";
      var reason = "reason for the test";
      var summary = "summary of the test";

      // Act
      var refStatus = new RefStatus()
      {
        Flag = RefStatus.State.New,
        From = from,
        Reason = reason,
        Summary = summary,
        To = to
      };

      // Verify
      Assert.AreEqual(from, refStatus.From);
      Assert.AreEqual(to, refStatus.To);
      Assert.AreEqual(reason, refStatus.Reason);
      Assert.AreEqual(summary, refStatus.Summary);

    } // end of function - GettersSettersOhMy

    /*----------------------- GetValidMappedStateValues ---------------------*/
    /// <summary>
    /// Verifies the mapping for character to <see cref="RefStatus.State"/>
    /// enum value.
    /// </summary>
    [TestMethod]
    public void GetValidMappedStateValues()
    {
      // Setup
      // ...
      // Act
      // ...
      // Verify
      Assert.AreEqual(RefStatus.State.FailedOrRejected, RefStatus.GetMappedState('!'));
      Assert.AreEqual(RefStatus.State.ForcedUpdated, RefStatus.GetMappedState('+'));
      Assert.AreEqual(RefStatus.State.New, RefStatus.GetMappedState('*'));
      Assert.AreEqual(RefStatus.State.Pruned, RefStatus.GetMappedState('-'));
      Assert.AreEqual(RefStatus.State.TagUpdated, RefStatus.GetMappedState('t'));
      Assert.AreEqual(RefStatus.State.Unknown, RefStatus.GetMappedState('\0'));
      Assert.AreEqual(RefStatus.State.Updated, RefStatus.GetMappedState(' '));
      Assert.AreEqual(RefStatus.State.UpToDate, RefStatus.GetMappedState('='));
    } // end of function - GetValidMappedStateValues

    /*----------------------- ThrowsForInvalidStateFlag ---------------------*/
    /// <summary>
    /// Verifies the <see cref="RefStatus.GetMappedState(char)"/> method
    /// correctly throws exception for an invalid flag value
    /// </summary>
    /// <returns></returns>
    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void ThrowsForInvalidStateFlag()
    {
      // Setup
      // Act/Verify
      var state = RefStatus.GetMappedState('m');
    } // end of function - ThrowsForInvalidStateFlag
    /************************ Static Functions *******************************/

    /************************ PROTECTED **************************************/
    /************************ Types ******************************************/
    /************************ Static Values **********************************/
    /************************ Fields *****************************************/
    /************************ Properties *************************************/
    /************************ Methods ****************************************/
    /************************ Static Functions *******************************/

    /************************ PRIVATE ****************************************/
    /************************ Types ******************************************/
    /************************ Static Values **********************************/
    /************************ Fields *****************************************/
    /************************ Properties *************************************/
    /************************ Methods ****************************************/
    /************************ Static Functions *******************************/

  } // end of class - RefStatusUnitTest

}
/*............................. End of RefStatusUnitTest.cs .................*/
