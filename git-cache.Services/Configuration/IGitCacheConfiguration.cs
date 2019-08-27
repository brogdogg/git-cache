/******************************************************************************
 * File...: IGitCacheConfiguration.cs
 * Remarks: 
 */
namespace git_cache.Services.Configuration
{
  /************************** IGitCacheConfiguration *************************/
  /// <summary>
  /// Describes the configuration for the main git-cache operations
  /// </summary>
  public interface IGitCacheConfiguration
  {
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Methods ****************************************/
    /// <summary>
    /// Gets/Sets a flag indicating if HTTPS should be disabled
    /// </summary>
    bool DisableHTTPS { get; set; }
    /// <summary>
    /// Gets/Sets the local storage path for cached items
    /// </summary>
    string LocalStoragePath { get; set; }
  } /* End of Interface - IGitCacheConfiguration */

}/* End of document - IGitCacheConfiguration.cs */
