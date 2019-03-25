/******************************************************************************
 * File...: IRemoteRepository.cs
 * Remarks: 
 */
namespace git_cache.Git
{
  /************************** IRemoteRepository ******************************/
  /// <summary>
  /// Remote repository interface
  /// </summary>
  public interface IRemoteRepository
  {
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Auth *******************************************/
    /// <summary>
    /// Gets the authentication information associated with the remote
    /// repository
    /// </summary>
    IAuthInfo Auth { get; }
    /************************ GitUrl *****************************************/
    /// <summary>
    /// Gets the GIT URL for the remote repository
    /// </summary>
    string GitUrl { get; }
    /************************ Name *******************************************/
    /// <summary>
    /// Gets/Sets the name of the repository
    /// </summary>
    string Name { get; set; }
    /************************ Owner ******************************************/
    /// <summary>
    /// Gets/Sets the owner's name of the repository
    /// </summary>
    string Owner { get; set; }
    /************************ Server *****************************************/
    /// <summary>
    /// Gets/Sets the server of the repository
    /// </summary>
    string Server { get; set; }
    /************************ Url ********************************************/
    /// <summary>
    /// Gets the URL for the repository
    /// </summary>
    string Url { get; }
    /************************ Methods ****************************************/
  } /* End of Interface - IRemoteRepository */

}/* End of document - IRemoteRepository.cs */