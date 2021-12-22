/******************************************************************************
 * File...: 
 * Remarks: 
 */
using git_cache.Services.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace git_cache.Services.mstest.Configuration
{
  /************************** GitCacheConfigurationUnitTest ******************/
  /// <summary>
  /// Unit tests for <see cref="Services.Configuration.GitCacheConfiguration"/>
  /// </summary>
  [TestClass]
  public class GitCacheConfigurationUnitTest
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /*----------------------- TestDefaults ----------------------------------*/
    /// <summary>
    /// Tests the defaults of the git-cache configuration object.
    /// </summary>
    [TestMethod]
    public void TestDefaults()
    {
      var obj = new GitCacheConfiguration();
      Assert.AreEqual(false, obj.DisableHTTPS);
      Assert.AreEqual(null, obj.LocalStoragePath);
      Assert.AreEqual(600000, obj.OperationTimeout);
    } /* End of Function - TestDefaults */
    /************************ Fields *****************************************/
    /************************ Static *****************************************/

    /*======================= PROTECTED =====================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /************************ Fields *****************************************/
    /************************ Static *****************************************/

    /*======================= PRIVATE =======================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /************************ Fields *****************************************/
    /************************ Static *****************************************/

  } /* End of Class - GitCacheConfigurationUnitTest */
}
/* End of document - GitCacheConfigurationUnitTest.cs */