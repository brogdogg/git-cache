/******************************************************************************
 * File...: IRemoteRepository.cs
 * Remarks: 
 */
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using git_cache.Services.Git;
using NSubstitute;
using System.Threading.Tasks;
using git_cache.Services.Configuration;

namespace git_cache.Services.mstest.Git
{
  /************************** GitContextUnitTest *****************************/
  /// <summary>
  /// Verifies the behavior of the <see cref="GitContext"/> object
  /// </summary>
  [TestClass]
  public class GitContextUnitTest
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /*----------------------- Initialize ------------------------------------*/
    /// <summary>
    /// Initializes for each test case
    /// </summary>
    [TestInitialize]
    public void Initialize()
    {
      m_git = Substitute.For<IGitExecuter>();
      m_lfs = Substitute.For<IGitLFSExecuter>();
      m_local = Substitute.For<ILocalRepositoryFactory>();
      m_remote = Substitute.For<IRemoteRepositoryFactory>();
      m_config = Substitute.For<IGitCacheConfiguration>();
    } /* End of Function - Initialize */

    /*----------------------- ThrowsWhenInvalidLocalFactory -----------------*/
    /// <summary>
    /// Constructor should throw when local factory is null
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ThrowsWhenInvalidLocalFactory()
    {
      var t = new GitContext(m_config, null, m_remote, m_git, m_lfs);
    } /* End of Function - ThrowsWhenInvalidLocalFactory */

    /*----------------------- ThrowsWhenInvalidRemoteFactory ----------------*/
    /// <summary>
    /// Constructor should throw when remote factory is null
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ThrowsWhenInvalidRemoteFactory()
    {
      var t = new GitContext(m_config, m_local, null, m_git, m_lfs);
    } /* End of Function - ThrowsWhenInvalidRemoteFactory */

    /*----------------------- ThrowsWhenInvalidGitExecuter ------------------*/
    /// <summary>
    /// Constructor should throw when git executer is null
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ThrowsWhenInvalidGitExecuter()
    {
      var t = new GitContext(m_config, m_local, m_remote, null, m_lfs);
    } /* End of Function - ThrowsWhenInvalidGitExecuter */

    /*----------------------- ThrowsWhenInvalidGitLFSExecuter ---------------*/
    /// <summary>
    /// Constructor should throw when lfs executor is null
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ThrowsWhenInvalidGitLFSExecuter()
    {
      var t = new GitContext(m_config, m_local, m_remote, m_git, null);
    } /* End of Function - ThrowsWhenInvalidGitLFSExecuter */

    /*----------------------- ContructsCorrectly ----------------------------*/
    /// <summary>
    /// Verifies the context constructs correctly holding references
    /// </summary>
    [TestMethod]
    public void ContructsCorrectly()
    {
      var t = new GitContext(m_config, m_local, m_remote, m_git, m_lfs);
      Assert.AreEqual(m_git, t.GitExecuter);
      Assert.AreEqual(m_lfs, t.LFSExecuter);
      Assert.AreEqual(m_local, t.LocalFactory);
      Assert.AreEqual(m_remote, t.RemoteFactory);
    } /* End of Function - ContructsCorrectly */

    /*----------------------- UpdateLocalAsyncWithFailedGitFetch ------------*/
    /// <summary>
    /// Verifies when the first git fetch throws exception, the rest of the
    /// behavior works as expected.
    /// </summary>
    [TestMethod]
    public void UpdateLocalAsyncWithFailedGitFetch()
    {
      // Setup
      var control = "test";
      var repo = Substitute.For<ILocalRepository>();
      var t = new GitContext(m_config, m_local, m_remote, m_git, m_lfs);
      // The very first call to git fetch will throw
      m_git.FetchAsync(repo).Returns(x => Task.Run(() => { throw new Exception("test"); }));
      // Which should cause a clone to happen instead
      m_git.CloneAsync(repo).Returns(x => Task.Run(() => control));
      // Act
      var output = t.UpdateLocalAsync(repo).Result;
      // Assert
      Assert.AreEqual(control, output);
      m_git.Received(1).FetchAsync(repo).Wait();
      m_git.Received(1).CloneAsync(repo).Wait();
      m_lfs.Received(1).FetchAsync(repo).Wait();
    } /* End of Function - UpdateLocalAsyncWithFailedGitFetch */

    /*----------------------- UpdateLocalAsyncWithFailedLFSFetch ------------*/
    /// <summary>
    /// Verifies when the fetch is good, but then the LFS fetch throws,
    /// the rest of the execution carries on
    /// </summary>
    [TestMethod]
    public void UpdateLocalAsyncWithFailedLFSFetch()
    {
      // Setup
      var control = "test";
      var repo = Substitute.For<ILocalRepository>();
      var t = new GitContext(m_config, m_local, m_remote, m_git, m_lfs);
      // The first time the git fetch is called it will pretend success
      m_git.FetchAsync(repo).Returns(x => Task.Run(() => control));
      // but the subsequent call to LFS fetch will fail
      m_lfs.FetchAsync(repo).Returns(x => Task.Run(() => { throw new Exception("test"); }));
      // Which should cause the git clone action
      m_git.CloneAsync(repo).Returns(x => Task.Run(() => control));
      // Followed by a LFS fetch again
      //m_lfs.FetchAsync(repo).Returns(x => Task.Run(() => control));
      m_lfs.FetchAsync(repo).Returns(x => Task.Run(() => control));
      // Act
      var output = t.UpdateLocalAsync(repo).Result;
      // Assert
      Assert.AreEqual(control, output);
      m_git.Received(1).FetchAsync(repo).Wait();
      m_git.Received(1).CloneAsync(repo).Wait();
      m_lfs.Received(2).FetchAsync(repo).Wait();
    } /* End of Function - UpdateLocalAsyncWithFailedLFSFetch */

    /*----------------------- UpdateLocalAsync ------------------------------*/
    /// <summary>
    /// Verifies normal operation of the method
    /// </summary>
    [TestMethod]
    public void UpdateLocalAsync()
    {
      // Setup
      var control = "test";
      var repo = Substitute.For<ILocalRepository>();
      var t = new GitContext(m_config, m_local, m_remote, m_git, m_lfs);
      m_git.FetchAsync(repo).Returns(x => Task.Run(() => control));
      m_lfs.FetchAsync(repo).Returns(x => Task.Run(() => "foobar"));
      // Act
      var output = t.UpdateLocalAsync(repo).Result;
      // Assert
      Assert.AreEqual(control, output);
      m_git.Received(1).FetchAsync(repo).Wait();
      m_lfs.Received(1).FetchAsync(repo).Wait();
    } /* End of Function - UpdateLocalAsync */
    /************************ Fields *****************************************/
    IGitCacheConfiguration m_config;
    ILocalRepositoryFactory m_local;
    IRemoteRepositoryFactory m_remote;
    IGitExecuter m_git;
    IGitLFSExecuter m_lfs;
    /************************ Static *****************************************/
  } /* End of Class - GitContextUnitTest */
}
/* End of document - GitContextUnitTest.cs */