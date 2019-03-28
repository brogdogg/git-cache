/******************************************************************************
 * File...: 
 * Remarks: 
 */
using git_cache.Git;
using git_cache.Git.LFS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;

namespace git_cache_mstest.Git.LFS
{
  /************************** LFSLocalRepoExtensionUnitTest ******************/
  /// <summary>
  /// Verifies the <see cref="ILocalRepository"/> extension methods
  /// </summary>
  [TestClass]
  public class LFSLocalRepoExtensionUnitTest
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /*----------------------- Initialize ------------------------------------*/
    /// <summary>
    /// Initialies the fake local repository item for testing the
    /// extension method with
    /// </summary>
    [TestInitialize]
    public void Initialize()
    {
      m_repo = Substitute.For<ILocalRepository>();
    } /* End of Function - Initialize */

    /*----------------------- ThrowsWhenOIDIsNull ---------------------------*/
    /// <summary>
    /// Verifies method throws when OID is null
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ThrowsWhenOIDIsNull()
    {
      m_repo.LFSObjectPath(null);
    } /* End of Function - ThrowsWhenOIDIsNull */

    /*----------------------- ThrowsWhenOIDIsNot64Long ----------------------*/
    /// <summary>
    /// Verifies method throws when OID is not correct length
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ThrowsWhenOIDIsNot64Long()
    {
      m_repo.LFSObjectPath("123");
    } /* End of Function - ThrowsWhenOIDIsNot64Long */

    /*----------------------- BuildsPathCorrectly ---------------------------*/
    /// <summary>
    /// Verifies correct behavior
    /// </summary>
    [TestMethod]
    public void BuildsPathCorrectly()
    {
      // Define a 64-char oid
      var oid = "1111111111111111111111111111111111111111111111111111111111111111";
      var result = m_repo.LFSObjectPath(oid);
      Assert.AreEqual($"lfs/objects/11/11/{oid}", result);
    } /* End of Function - BuildsPathCorrectly */
    /************************ Fields *****************************************/
    ILocalRepository m_repo;
    /************************ Static *****************************************/
  } /* End of Class - LFSLocalRepoExtensionUnitTest */
}
/* End of document - LFSLocalRepoExtensionUnitTest.cs */