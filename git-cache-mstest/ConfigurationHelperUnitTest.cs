/******************************************************************************
 * File...: IRemoteRepository.cs
 * Remarks: 
 */
using Microsoft.VisualStudio.TestTools.UnitTesting;
using git_cache.Configuration;
using NSubstitute;
using Microsoft.Extensions.Configuration;

namespace git_cache_mstest
{
  /************************** ConfigurationHelperUnitTest ********************/
  /// <summary>
  /// Unit tests for the <see cref="ConfigurationHelper"/> class
  /// </summary>
  [TestClass]
  public class ConfigurationHelperUnitTest
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /*----------------------- GetLocalStoragePathWithoutDefault -------------*/
    /// <summary>
    /// Verifies the <see cref="ConfigurationHelper"/> returns the correct
    /// value when called without a default specified.
    /// </summary>
    [TestMethod]
    public void GetLocalStoragePathWithoutDefault()
    {
      var pretendPath = "nonPath";
      var config = Substitute.For<IConfiguration>();
      config.GetValue(
        ConfigurationHelper.CACHE_DIRECTORY_KEY,
        ConfigurationHelper.DEFAULT_STORAGE_PATH).Returns(pretendPath);
      var path = ConfigurationHelper.GetLocalStoragePath(config);
      Assert.AreEqual(path, pretendPath);
    } /* End of Function - GetLocalStoragePathWithoutDefault */

    /*----------------------- GetLocalStoragePathWithDefault ----------------*/
    /// <summary>
    /// Verifies the <see cref="ConfigurationHelper"/> returns the correct
    /// value when called with a default specified.
    /// </summary>
    [TestMethod]
    public void GetLocalStoragePathWithDefault()
    {
      var pretendPath = "nonPath";
      var defPath = "defaultPath";
      var config = Substitute.For<IConfiguration>();
      config.GetValue(
        ConfigurationHelper.CACHE_DIRECTORY_KEY,
        defPath).Returns(pretendPath);
      var path = ConfigurationHelper.GetLocalStoragePath(config, defPath);
      Assert.AreEqual(path, pretendPath);
    } /* End of Function - GetLocalStoragePathWithDefault */

    /*----------------------- GetDisableHTTPSWithoutDefault -----------------*/
    /// <summary>
    /// Verifies the <see cref="ConfigurationHelper"/> returns the correct
    /// value when caleld without specifying a default for disable HTTPS
    /// </summary>
    [TestMethod]
    public void GetDisableHTTPSWithoutDefault()
    {
      // Setup
      //  Fake the configuration section
      var cfgSection = Substitute.For<IConfigurationSection>();
      //  Pretend it doesn't have a value, so the default value will be used
      cfgSection.Value.Returns((string)null);
      //  Now setup the fake configuration
      var config = Substitute.For<IConfiguration>();
      //  Returning our fake section when asked for it
      config.GetSection(ConfigurationHelper.DISABLE_HTTPS_KEY).Returns(cfgSection);
      // Act - should return the default value of false
      var res = ConfigurationHelper.DisableHTTPS(config);
      // Assert
      Assert.IsFalse(res);
    } /* End of Function - GetDisableHTTPSWithoutDefault */

    /*----------------------- GetDisableHTTPSWithDefault --------------------*/
    /// <summary>
    /// Verifies the <see cref="ConfigurationHelper"/> returns the correct
    /// value when called with specifying a default for disable HTTPS
    /// </summary>
    [TestMethod]
    public void GetDisableHTTPSWithDefault()
    {
      // Setup
      //  Fake the configuration section
      var cfgSection = Substitute.For<IConfigurationSection>();
      //  Pretending "FALSE" is specified in the configuration
      cfgSection.Value.Returns("false");
      //  And then hook it into the main fake configuration
      var config = Substitute.For<IConfiguration>();
      config.GetSection(ConfigurationHelper.DISABLE_HTTPS_KEY).Returns(cfgSection);
      // Act - Specifying a different default value
      var res = ConfigurationHelper.DisableHTTPS(config, true);
      // Assert
      Assert.IsFalse(res);
    } /* End of Function - GetDisableHTTPSWithDefault */
    /************************ Fields *****************************************/
    /************************ Static *****************************************/
  } /* End of Class - ConfigurationHelperUnitTest */
}
/* End of document - ConfigurationHelperUnitTest.cs */