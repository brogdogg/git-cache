﻿/******************************************************************************
 * File...: GitControllerUnitTest.cs
 * Remarks: 
 */
using git_cache.Controllers;
using git_cache.Git;
using git_cache.Shell;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Net;

namespace git_cache_mstest.Controllers
{
  /************************** GitControllerUnitTest **************************/
  /// <summary>
  /// Verifies the behavior of the <see cref="GitController"/> class
  /// </summary>
  [TestClass]
  public class GitControllerUnitTest
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /*----------------------- ThrowsWhenConfigurationNull -------------------*/
    /// <summary>
    /// Verifies the constructor throws when the configuration is null
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ThrowsWhenConfigurationNull()
    {
      var gitContext = Substitute.For<IGitContext>();
      var shell = Substitute.For<IShell>();
      var a = new GitController(null, gitContext, shell);
    } /* End of Function - ThrowsWhenConfigurationNull */

    /*----------------------- ThrowsWhenGitContextNull ----------------------*/
    /// <summary>
    /// Verifies the constructor throws when the git context is null
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ThrowsWhenGitContextNull()
    {
      var config = Substitute.For<IConfiguration>();
      var shell = Substitute.For<IShell>();
      var a = new GitController(config, null, shell);
    } /* End of Function - ThrowsWhenGitContextNull */

    /*----------------------- ThrowsWhenShellIsNull -------------------------*/
    /// <summary>
    /// Verifies the constructor throws when the shell object is null
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ThrowsWhenShellIsNull()
    {
      var config = Substitute.For<IConfiguration>();
      var gitContext = Substitute.For<IGitContext>();
      var a = new GitController(config, gitContext, null);
    } /* End of Function - ThrowsWhenShellIsNull */

    /*----------------------- ReportsSuccessOnDeleteCachedRepo --------------*/
    /// <summary>
    /// 
    /// </summary>
    [TestMethod]
    public void ReportsSuccessOnDeleteCachedRepo()
    {
      var server = "server";
      var owner = "owner";
      var repo = "repo";
      string auth = null;
      var gitCtrl = new GitController(m_config, m_git, m_shell);
      var remoteRepo = Substitute.For<IRemoteRepository>();
      m_remoteFactory.Build(server, owner, repo, auth).Returns(remoteRepo);
      var localRepo = Substitute.For<ILocalRepository>();
      m_localFactory.Build(remoteRepo, m_config).Returns(localRepo);
      string tmpPath = System.IO.Path.Combine(
        System.IO.Path.GetTempPath(),
        "ReportsSuccessOnDeleteCachedRepo");
      localRepo.Path.Returns(tmpPath);
      if (System.IO.Directory.Exists(tmpPath))
        System.IO.Directory.Delete(tmpPath, true);
      System.IO.Directory.CreateDirectory(tmpPath);

      var result = gitCtrl.DeleteCachedRepository(server, owner, repo);

      Assert.IsFalse(System.IO.Directory.Exists(tmpPath));
    } /* End of Function - ReportsSuccessOnDeleteCachedRepo */

    /*----------------------- Initialize ------------------------------------*/
    /// <summary>
    /// Initializes the fiels for all tests
    /// </summary>
    [TestInitialize]
    public void Initialize()
    {
      m_config = Substitute.For<IConfiguration>();
      m_git = Substitute.For<IGitContext>();
      m_gitExecuter = Substitute.For<IGitExecuter>();
      m_lfsExecuter = Substitute.For<IGitLFSExecuter>();
      m_localFactory = Substitute.For<ILocalRepositoryFactory>();
      m_remoteFactory = Substitute.For<IRemoteRepositoryFactory>();
      m_git.GitExecuter.Returns(m_gitExecuter);
      m_git.LFSExecuter.Returns(m_lfsExecuter);
      m_git.LocalFactory.Returns(m_localFactory);
      m_git.RemoteFactory.Returns(m_remoteFactory);
      m_shell = Substitute.For<IShell>();
    } /* End of Function - Initialize */
    /************************ Fields *****************************************/
    IConfiguration m_config;
    IGitContext m_git;
    IShell m_shell;
    IGitExecuter m_gitExecuter;
    IGitLFSExecuter m_lfsExecuter;
    ILocalRepositoryFactory m_localFactory;
    IRemoteRepositoryFactory m_remoteFactory;
    /************************ Static *****************************************/
  } /* End of Class - GitControllerUnitTest */
}
/* End of document - GitControllerUnitTest.cs */