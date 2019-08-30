/******************************************************************************
 * File...: GitContextUnitTest.cs
 * Remarks: 
 */
using git_cache.Services.Shell;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;

namespace git_cache.Services.mstest.Shell
{
  /************************** BashShellUnitTest ******************************/
  /// <summary>
  /// Verifies the behavior of the BashShell instance
  /// </summary>
  [TestClass]
  public class BashShellUnitTest
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /*----------------------- CanCreate -------------------------------------*/
    /// <summary>
    /// Verifies we can create an instance
    /// </summary>
    [TestMethod]
    public void CanCreate()
    {
      var shell = new BashShell(Substitute.For<ILogger<BashShell>>());
    } /* End of Function - CanCreate */
    /*----------------------- ThrowsOnNullLogger ----------------------------*/
    /// <summary>
    /// Verifies the constructor throws when a logger is not provided
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ThrowsOnNullLogger()
    {
      var shell = new BashShell(null);
    } /* End of Function - ThrowsOnNullLogger */

    /************************ Fields *****************************************/
    /************************ Static *****************************************/

  } /* End of Class - BashShellUnitTest */
}
/* End of document - BashShellUnitTest.cs */