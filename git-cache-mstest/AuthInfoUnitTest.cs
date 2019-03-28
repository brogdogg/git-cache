/******************************************************************************
 * File...: IRemoteRepository.cs
 * Remarks: 
 */
using git_cache.Git;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;

namespace git_cache_mstest
{
  /************************** AuthInfoUnitTest *******************************/
  /// <summary>
  /// Tests the <see cref="AuthInfo"/> object for correct behavior
  /// </summary>
  [TestClass]
  public class AuthInfoUnitTest
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

    /*----------------------- CanParseWithoutAuth ---------------------------*/
    /// <summary>
    /// Verifies the parse method works when the auth string is null
    /// </summary>
    [TestMethod]
    public void CanParseWithoutAuth()
    {
      var info = AuthInfo.ParseAuth(null);
      Assert.IsNull(info);
    } /* End of Function - CanParseWithoutAuth */

    /*----------------------- CanParseAuth ----------------------------------*/
    /// <summary>
    /// Verifies we can parse an auth correctly
    /// </summary>
    [TestMethod]
    public void CanParseAuth()
    {
      var dec = "test";
      var enc = Convert.ToBase64String(Encoding.UTF8.GetBytes(dec));
      var raw = $"Basic {enc}";
      var info = AuthInfo.ParseAuth(raw);
      Assert.AreEqual(dec, info.Decoded);
      Assert.AreEqual("Basic", info.Scheme);
      Assert.AreEqual(raw, info.RawAuth);
      Assert.AreEqual(enc, info.Encoded);
    } /* End of Function - CanParseAuth */
    /************************ Fields *****************************************/
    /************************ Static *****************************************/
  } /* End of Class - AuthInfoUnitTest */
}
/* End of document - AuthInfoUnitTest.cs */