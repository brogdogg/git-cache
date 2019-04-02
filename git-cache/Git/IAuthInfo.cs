/******************************************************************************
 * File...: IAuthInfo.cs
 * Remarks: 
 */
namespace git_cache.Git
{
  /************************** IAuthInfo **************************************/
  /// <summary>
  /// Represents an authentication information object
  /// </summary>
  public interface IAuthInfo
  {
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Decoded ****************************************/
    /// <summary>
    /// Gets/Sets the decoded value for authentication
    /// </summary>
    string Decoded { get; set; }
    /************************ Encoded ****************************************/
    /// <summary>
    /// Gets/SEts the encoded value for authentication
    /// </summary>
    string Encoded { get; set; }
    /************************ RawAuth ****************************************/
    /// <summary>
    /// Gets/Sets the raw authentication information
    /// </summary>
    string RawAuth { get; set; }
    /************************ Scheme *****************************************/
    /// <summary>
    /// Gets/Sets the authentication scheme
    /// </summary>
    string Scheme { get; set; }
    /************************ Methods ****************************************/
  } /* End of Interface - IAuthInfo */

}
/* End of document - IAuthInfo.cs */
