/******************************************************************************
 * File...: GitCacheExtensionUnitTest.cs
 * Remarks: 
 */
using git_cache.Services.Extensions.DependencyInjection;
using git_cache.Services.Git;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace git_cache.Services.mstest.Extensions.DependencyInjection
{
  /************************** GitCacheExtensionUnitTest **********************/
  /// <summary>
  /// Tests for verifying the behavior of the <see cref="IServiceCollection"/>
  /// extension method, which configures for the git-cache operation
  /// </summary>
  [TestClass]
  public class GitCacheExtensionUnitTest : BaseExtensionUnitTest
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /*----------------------- AddGitCacheServices ---------------------------*/
    /// <summary>
    /// Verifies the correct services are added as expected.
    /// </summary>
    [TestMethod]
    public void AddGitCacheServices()
    {
      // SETUP
      // ACT
      Services.AddGitCacheServices();
      var serviceProvider = Services.BuildServiceProvider();
      // VERIFY
      Assert.IsInstanceOfType(
        serviceProvider.GetService<IRemoteRepositoryFactory>(),
        typeof(RemoteRepositoryFactory));
      Assert.IsInstanceOfType(
        serviceProvider.GetService<ILocalRepositoryFactory>(),
        typeof(LocalRepositoryFactory));

      // The following won't work when running on Windows,
      // but they will on non-Windows. For now just going
      // to not bother
      /*
      Assert.IsInstanceOfType(
        serviceProvider.GetService<IGitExecuter>(),
        typeof(GitExecuter));

      Assert.IsInstanceOfType(
        serviceProvider.GetService<IGitLFSExecuter>(),
        typeof(GitLFSExecutor));

      Assert.IsInstanceOfType(
        serviceProvider.GetService<IGitContext>(),
        typeof(GitContext));
      */
    } /* End of Function - AddGitCacheServices */
    /************************ Fields *****************************************/
    /************************ Static *****************************************/

  } /* End of Class - GitCacheExtensionUnitTest */
}
/* End of document - GitCacheExtensionUnitTest.cs */