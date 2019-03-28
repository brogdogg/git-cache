/******************************************************************************
 * File...: 
 * Remarks: 
 */
using git_cache.Configuration;
using git_cache.Git;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;

namespace git_cache_mstest.Git
{
  /************************** LocalRepoUnitTest ******************************/
  /// <summary>
  /// Verifies the behavior of <see cref="LocalRepository"/>
  /// </summary>
  [TestClass]
  public class LocalRepoUnitTest
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /*----------------------- ThrowsWhenRemoteRepositoryIsInvalid -----------------*/
    /// <summary>
    /// Verifies the <see cref="LocalRepository"/> constructor throws when remote
    /// repo is null
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ThrowsWhenRemoteRepositoryIsInvalid()
    {
      var localRepo =
        new LocalRepository(null,
                      Substitute.For<IConfiguration>());
    } /* End of Function - ThrowsWhenRemoteRepositoryIsInvalid */

    /*----------------------- ThrowsWhenConfigIsInvalid ---------------------*/
    /// <summary>
    /// Verifies the <see cref="LocalRepository"/> constructor throws when the
    /// configuration is null
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ThrowsWhenConfigIsInvalid()
    {
      var remoteRepo = new RemoteRepository("server", "owner", "name", null);
      var localRepo = new LocalRepository(remoteRepo, null);
    } /* End of Function - ThrowsWhenConfigIsInvalid */

    /*----------------------- ConstructsPathCorrectly -----------------------*/
    /// <summary>
    /// Verifies the <see cref="LocalRepository"/> constructs the local path
    /// correctly.
    /// </summary>
    [TestMethod]
    public void ConstructsPathCorrectly()
    {
      // Setup
      var server = "server";
      var owner = "owner";
      var name = "name";
      var config = Substitute.For<IConfiguration>();
      config.GetValue<string>(ConfigurationHelper.CACHE_DIRECTORY_KEY, ConfigurationHelper.DEFAULT_STORAGE_PATH).Returns("/test");
      var remoteRepo = new RemoteRepository(server, owner, name, null);

      // Act
      var localRepo = new LocalRepository(remoteRepo, config);

      // Assert
      Assert.AreEqual(System.IO.Path.Combine("/test", server, owner, name),
                      localRepo.Path);
    } /* End of Function - ConstructsPathCorrectly */

    /*----------------------- CreatesLocalDirectory -------------------------*/
    /// <summary>
    /// Verifies the <see cref="LocalRepository"/> can create the local directory
    /// without issue.
    /// </summary>
    [TestMethod]
    public void CreatesLocalDirectory()
    {
      // Setup
      var server = "server";
      var owner = "owner";
      var name = "name";
      var basePath = System.IO.Path.GetTempPath();
      var config = Substitute.For<IConfiguration>();
      config.GetValue<string>(
        ConfigurationHelper.CACHE_DIRECTORY_KEY,
        ConfigurationHelper.DEFAULT_STORAGE_PATH).Returns(basePath);
      var remoteRepo = new RemoteRepository(server, owner, name, null);
      var srvPath = System.IO.Path.Combine(basePath, server);
      var fullPath = System.IO.Path.Combine(srvPath, owner, name);

      // Act
      var localRepo = new LocalRepository(remoteRepo, config);

      // Assert
      if (System.IO.Directory.Exists(srvPath))
        System.IO.Directory.Delete(srvPath, true);
      Assert.IsFalse(System.IO.Directory.Exists(fullPath));
      localRepo.CreateLocalDirectory();
      Assert.IsTrue(System.IO.Directory.Exists(fullPath));
      System.IO.Directory.Delete(srvPath, true);
    } /* End of Function - CreatesLocalDirectory */
    /************************ Fields *****************************************/
    /************************ Static *****************************************/
  } /* End of Class - LocalRepoUnitTest */

} /* End of Namespace - git_cache_mstest */

/* End of document - LocalRepoUnitTest.cs */
