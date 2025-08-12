/******************************************************************************
 * File...: RemoteStatusUnitTest.cs
 * Remarks: 
 */
using git_cache.Services.Git;
using git_cache.Services.Git.Status;
using git_cache.Services.Shell;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;

namespace git_cache.Services.mstest.Git.Status
{
  /************************** RemoteStatusUnitTest ***************************/
  /// <summary>
  /// Tests to verify the behaviors of <see cref="RemoteStatus"/> class
  /// </summary>
  [TestClass]
  public class RemoteStatusUnitTest
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /*----------------------- GetsFine --------------------------------------*/
    /// <summary>
    /// Verifies we can get without issue.
    /// </summary>
    [TestMethod]
    public void GetsFine()
    {
      // Setup
      var localRepo = Substitute.For<ILocalRepository>();
      var gitExecutor = Substitute.For<IGitExecuter>();
      gitExecutor.FetchAsync(localRepo, true).Returns("\n" +
        "   88ff88ff88..ff88ff88ff dev  -> origin/dev\n" +
        "   865e139..431f7e9  dev                   -> origin/dev\n" +
        "   140942d..434368d  fix/DPDL-1546/noMoRPC -> origin/fix/DPDL-1546/noMoRPC\n" +
        " = [up to date]      master                -> origin/master\n" +
        " = [up to date]      test/DPDL-1571/stringResGlitch -> origin/test/DPDL-1571/stringResGlitch\n" +
        "\n");
      var status = new RemoteStatus(gitExecutor);
      // Act
      var result = status.GetAsync(localRepo).Result;
      // Verify
    } /* End of Function - GetsFine */

    /*----------------------- ThrowsExceptionForNullShell -------------------*/
    /// <summary>
    /// Verifies the <see cref="RemoteStatus"/> constructor throws when the
    /// <see cref="IShell"/> object is null
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ThrowsExceptionForNullShell()
    {
      // Act
      var status = new RemoteStatus(null);
    } /* End of Function - ThrowsExceptionForNullShell */
    /************************ Fields *****************************************/
    /************************ Static *****************************************/
  } /* End of Class - RemoteStatusUnitTest */
}
/* End of document - RemoteStatusUnitTest.cs */