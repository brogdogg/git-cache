/******************************************************************************
 * File...: GitCacheConfiguration.cs
 * Remarks: 
 */
using System.ComponentModel.DataAnnotations;

namespace git_cache.Services.Configuration
{
  /************************** GitCacheConfiguration **************************/
  /// <summary>
  /// Configuration object for the git-cache
  /// </summary>
  public class GitCacheConfiguration : IGitCacheConfiguration
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ LocalStoragePath *******************************/
    /// <summary>
    /// Gets/Sets the local storage path to use.
    /// </summary>
    [Required]
    public string LocalStoragePath
    {
      get; set;
    } = null; /* End of Property - LocalStoragePath */

    /************************ DisableHTTPS ***********************************/
    /// <summary>
    /// Gets/Sets a flag indicating to disable HTTPS 
    /// </summary>
    public bool DisableHTTPS
    {
      get; set;
    } = false; /* End of Property - DisableHTTPS */
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /************************ Fields *****************************************/
    /************************ Static *****************************************/

  } /* End of Class - GitCacheConfiguration */
}
/* End of document - GitCacheConfiguration.cs */