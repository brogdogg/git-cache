/******************************************************************************
 * File...: GitAuthorizationCheckFilterUnitTest.cs
 * Remarks: 
 */
using git_cache.Filters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace git_cache.mstest.Filters
{
  /************************** GitAuthorizationCheckFilterUnitTest ************/
  /// <summary>
  /// Tests the behavior of <see cref="GitAuthorizationCheckFilter"/>
  /// </summary>
  [TestClass]
  public class GitAuthorizationCheckFilterUnitTest
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /*----------------------- ThrowsWithNullFactory -------------------------*/
    /// <summary>
    /// 
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ThrowsWithNullFactory()
    {
      var filter = new GitAuthorizationCheckFilterAttribute(null);
    } /* End of Function - ThrowsWithNullFactory */
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

  } /* End of Class - GitAuthorizationCheckFilterUnitTest */
}
/* End of document - GitAuthorizationCheckFilterUnitTest.cs */