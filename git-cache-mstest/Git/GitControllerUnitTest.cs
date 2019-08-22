/******************************************************************************
 * File...: IRemoteRepository.cs
 * Remarks: 
 */
using git_cache.Controllers;
using git_cache.Services.Configuration;
using git_cache.Services.Git;
using git_cache.Services.Shell;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;

namespace git_cache_mstest.Git
{
  /************************** GitControllerUnitTest **************************/
  /// <summary>
  /// Unit tests for the <see cref="GitController"/> class
  /// </summary>
  [TestClass]
  public class GitControllerUnitTest
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /*----------------------- TestInitialize --------------------------------*/
    /// <summary>
    /// Initializes fields used by each test case.
    /// </summary>
    [TestInitialize]
    public void TestInitialize()
    {
      m_remoteFactory = Substitute.For<IRemoteRepositoryFactory>();
      m_localFactory = Substitute.For<ILocalRepositoryFactory>();
      m_gitExec = Substitute.For<IGitExecuter>();
      m_lfsExec = Substitute.For<IGitLFSExecuter>();
      m_config = Substitute.For<IGitCacheConfiguration>();
      m_gitContext = new GitContext(
        m_config,
        m_localFactory,
        m_remoteFactory,
        m_gitExec,
        m_lfsExec);
    } /* End of Function - TestInitialize */

    /*----------------------- ThrowsWhenInvalidConfig -----------------------*/
    /// <summary>
    /// Verifies controller throws when configuration is null
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ThrowsWhenInvalidConfig()
    {
      var ctrl = new GitController(m_gitContext, m_shell);
    } /* End of Function - ThrowsWhenInvalidConfig */

    /*----------------------- ThrowsWhenInvalidContext ----------------------*/
    /// <summary>
    /// Verifies controller throws when the Git context is null
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ThrowsWhenInvalidContext()
    {
      var ctrl = new GitController(null, m_shell);
    } /* End of Function - ThrowsWhenInvalidContext */

    /************************ Fields *****************************************/
    /// <summary>
    /// Remote repository factory class
    /// </summary>
    IRemoteRepositoryFactory m_remoteFactory;
    /// <summary>
    /// Local repository factory class
    /// </summary>
    ILocalRepositoryFactory m_localFactory;
    /// <summary>
    /// Configuration item
    /// </summary>
    IGitCacheConfiguration m_config;
    /// <summary>
    /// GitContext
    /// </summary>
    IGitContext m_gitContext;
    /// <summary>
    /// LFS executor
    /// </summary>
    IGitLFSExecuter m_lfsExec;
    /// <summary>
    /// Git executor
    /// </summary>
    IGitExecuter m_gitExec;
    /// <summary>
    /// Gets the shell object
    /// </summary>
    IShell m_shell;
    /************************ Static *****************************************/
  } /* End of Class - GitControllerUnitTest */
}
/* End of document - GitControllerUnitTest.cs */