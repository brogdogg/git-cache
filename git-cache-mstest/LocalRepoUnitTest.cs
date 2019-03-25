/******************************************************************************
 * File...: 
 * Remarks: 
 */
using git_cache.Git;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;

namespace git_cache_mstest
{
  /************************** LocalRepoAuthInfoUnitTest **********************/
  /// <summary>
  /// Tests the <see cref="AuthInfo"/> object for correct behavior
  /// </summary>
  [TestClass]
  public class LocalRepoAuthInfoUnitTest
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /*----------------------- GetAndSetValues -------------------------------*/
    /// <summary>
    /// Simple test to verify what we set is what we get for the
    /// <see cref="AuthInfo"/> class.
    /// </summary>
    [TestMethod]
    public void GetAndSetValues()
    {
      var dec = "Decoded_UT";
      var enc = "Encoded_UT";
      var raw = "RawAuth_UT";
      var scm = "Scheme_UT";
      AuthInfo info = new AuthInfo();
      info.Decoded = dec;
      info.Encoded = enc;
      info.RawAuth = raw;
      info.Scheme = scm;
      Assert.AreEqual(dec, info.Decoded);
      Assert.AreEqual(enc, info.Encoded);
      Assert.AreEqual(raw, info.RawAuth);
      Assert.AreEqual(scm, info.Scheme);
    } /* End of Function - GetAndSetValues */
    /************************ Fields *****************************************/
    /************************ Static *****************************************/
  } /* End of Class - LocalRepoAuthInfoUnitTest */

  /************************** LocalRepoRemoteRepoUnitTest ********************/
  /// <summary>
  /// Tests the <see cref="RemoteRepo"/> object for correct behavior
  /// </summary>
  [TestClass]
  public class LocalRepoRemoteRepoUnitTest
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
      RemoteRepo repo = new RemoteRepo(server, owner, repoName, authInfo, false);

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
      RemoteRepo repo = new RemoteRepo(server, owner, repoNameGit, authInfo, false);

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
    /// 
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
      RemoteRepo repo = new RemoteRepo(server, owner, repoName, authInfo, true);

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
    /// Verifies the <see cref="RemoteRepo"/> constructor throws when the
    /// server is null
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ThrowsForInvalidServer()
    {
      var auth = new RemoteRepo(null, "", "", new AuthInfo());
    } /* End of Function - ThrowsForInvalidServer */

    /*----------------------- ThrowsForInvalidOwner -------------------------*/
    /// <summary>
    /// Verifies the <see cref="RemoteRepo"/> constructor throws when the
    /// owner is null
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ThrowsForInvalidOwner()
    {
      var auth = new RemoteRepo("", null, "", new AuthInfo());
    } /* End of Function - ThrowsForInvalidOwner */

    /*----------------------- ThrowsForInvalidName --------------------------*/
    /// <summary>
    /// Verifies the <see cref="RemoteRepo"/> constructor throws when the
    /// repository name is null
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ThrowsForInvalidName()
    {
      var auth = new RemoteRepo("", "", null, new AuthInfo());
    } /* End of Function - ThrowsForInvalidName */

    /************************ Fields *****************************************/
    /************************ Static *****************************************/
  } /* End of Class - LocalRepoRemoteRepoUnitTest */

} /* End of Namespace - git_cache_mstest */

/* End of document - LocalRepoUnitTest.cs */