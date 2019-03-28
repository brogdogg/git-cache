/******************************************************************************
 * File...: IRemoteRepository.cs
 * Remarks: 
 */
using git_cache.Git;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace git_cache_mstest.Git
{
  /************************** RemoteRepositoryUnitTest ***********************/
  /// <summary>
  /// Tests the <see cref="RemoteRepository"/> object for correct behavior
  /// </summary>
  [TestClass]
  public class RemoteRepositoryUnitTest
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /*----------------------- DefaultsCorrectly -----------------------------*/
    /// <summary>
    /// Verifies the remote repository works correctly with defaults
    /// </summary>
    [TestMethod]
    public void DefaultsCorrectly()
    {
      // Setup
      string server = "server_test";
      string owner = "owner_test";
      string repoName = "repo_name";
      var authInfo = new AuthInfo()
      {
        Decoded = "authInfo"
      };

      // Act
      RemoteRepository repo = new RemoteRepository(server, owner, repoName, authInfo, false);

      // Setup
      Assert.AreEqual(server, repo.Server);
      Assert.AreEqual(owner, repo.Owner);
      Assert.AreEqual(repoName, repo.Name);
      Assert.AreEqual(authInfo, repo.Auth);
      Assert.AreEqual($"https://{server}/{owner}/{repoName}", repo.Url);
      Assert.AreEqual($"https://{authInfo.Decoded}@{server}/{owner}/{repoName}", repo.GitUrl);
    } /* End of Function - DefaultsCorrectly */

    /*----------------------- DefaultsCorrectlyWithGitExtension -------------*/
    /// <summary>
    /// Verifies the behavior when the repo name ends in ".git".
    /// </summary>
    [TestMethod]
    public void DefaultsCorrectlyWithGitExtension()
    {
      // Setup
      string server = "server_test";
      string owner = "owner_test";
      string repoName = "repo_name";
      string repoNameGit = $"{repoName}.git";
      var authInfo = new AuthInfo()
      {
        Decoded = "authInfo"
      };

      // Act
      //   Using the repository name that ends in .git should yield similar results
      //   when it doesn't end in .git
      RemoteRepository repo = new RemoteRepository(server, owner, repoNameGit, authInfo, false);

      // Assert
      Assert.AreEqual(server, repo.Server);
      Assert.AreEqual(owner, repo.Owner);
      Assert.AreEqual(repoName, repo.Name);
      Assert.AreEqual(authInfo, repo.Auth);
      Assert.AreEqual($"https://{server}/{owner}/{repoName}", repo.Url);
      Assert.AreEqual($"https://{authInfo.Decoded}@{server}/{owner}/{repoName}", repo.GitUrl);
    } /* End of Function - DefaultsCorrectlyWithGitExtension */

    /*----------------------- CanHandleDisabledHTTPS ------------------------*/
    /// <summary>
    /// Verifies the behavior when HTTPS is disabled
    /// </summary>
    [TestMethod]
    public void CanHandleDisabledHTTPS()
    {
      // Setup
      string server = "server_test";
      string owner = "owner_test";
      string repoName = "repo_name";
      var authInfo = new AuthInfo()
      {
        Decoded = "authInfo"
      };

      // Act
      RemoteRepository repo = new RemoteRepository(server, owner, repoName, authInfo, true);

      // Setup
      Assert.AreEqual(server, repo.Server);
      Assert.AreEqual(owner, repo.Owner);
      Assert.AreEqual(repoName, repo.Name);
      Assert.AreEqual(authInfo, repo.Auth);
      Assert.AreEqual($"http://{server}/{owner}/{repoName}", repo.Url);
      Assert.AreEqual($"http://{authInfo.Decoded}@{server}/{owner}/{repoName}", repo.GitUrl);

    } /* End of Function - CanHandleDisabledHTTPS */

    /*----------------------- ThrowsForInvalidServer ------------------------*/
    /// <summary>
    /// Verifies the <see cref="RemoteRepository"/> constructor throws when the
    /// server is null
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ThrowsForInvalidServer()
    {
      var auth = new RemoteRepository(null, "", "", new AuthInfo());
    } /* End of Function - ThrowsForInvalidServer */

    /*----------------------- ThrowsForInvalidOwner -------------------------*/
    /// <summary>
    /// Verifies the <see cref="RemoteRepository"/> constructor throws when the
    /// owner is null
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ThrowsForInvalidOwner()
    {
      var auth = new RemoteRepository("", null, "", new AuthInfo());
    } /* End of Function - ThrowsForInvalidOwner */

    /*----------------------- ThrowsForInvalidName --------------------------*/
    /// <summary>
    /// Verifies the <see cref="RemoteRepository"/> constructor throws when the
    /// repository name is null
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ThrowsForInvalidName()
    {
      var auth = new RemoteRepository("", "", null, new AuthInfo());
    } /* End of Function - ThrowsForInvalidName */

    /************************ Fields *****************************************/
    /************************ Static *****************************************/
  } /* End of Class - RemoteRepositoryUnitTest */
}
/* End of document - RemoteRepositoryUnitTest.cs */