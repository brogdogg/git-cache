/******************************************************************************
 * File...: IRemoteRepository.cs
 * Remarks: 
 */
using System;
using System.Text;

namespace git_cache.Git
{
  /************************** AuthInfo ***************************************/
  /// <summary>
  /// Authentication information
  /// </summary>
  public class AuthInfo : IAuthInfo
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /// <summary>
    /// Gets/Sets the scheme for the authentication information
    /// </summary>
    public string Scheme { get; set; } /* End of Property - Scheme */
    /********************** RawAuth ****************************************/
    /// <summary>
    /// Gets/Sets the raw authentication information
    /// </summary>
    public string RawAuth { get; set; } /* End of Property - RawAuth */
    /********************** Decoded ****************************************/
    /// <summary>
    /// Gets/Sets the decoded value
    /// </summary>
    public string Decoded { get; set; } /* End of Property - Decoded */
    /********************** Encoded ****************************************/
    /// <summary>
    /// Gets/Sets the encoded value
    /// </summary>
    public string Encoded { get; set; } /* End of Property - Encoded */
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /************************ Fields *****************************************/
    /************************ Static *****************************************/
    /*----------------------- ParseAuth -------------------------------------*/
    /// <summary>
    /// Parses the string for a basic authorization entry
    /// </summary>
    /// <param name="auth"></param>
    public static IAuthInfo ParseAuth(string auth)
    {
      AuthInfo retval = null;
      if (null != auth)
      {
        var pair = auth.Split(" ");
        retval = new AuthInfo()
        {
          Decoded = Encoding.UTF8.GetString(Convert.FromBase64String(pair[1])),
          Encoded = pair[1],
          RawAuth = auth,
          Scheme = pair[0]
        };
      }
      return retval;
    } // end of function - ParseAuth
  } /* End of Class - AuthInfo */
}
/* End of document - AuthInfo.cs */