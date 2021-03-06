﻿/******************************************************************************
 * File...: IRemoteRepository.cs
 * Remarks: 
 */
namespace git_cache.Services.Git
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

  /************************** IRemoteRepositoryFactory ***********************/
  /// <summary>
  /// Represents a factory class capable of building
  /// <see cref="IRemoteRepository"/> objects
  /// </summary>
  public interface IRemoteRepositoryFactory
  {
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Methods ****************************************/
    /// <summary>
    /// Builds a <see cref="IRemoteRepository"/> object
    /// </summary>
    /// <param name="server">
    /// Remote server address
    /// </param>
    /// <param name="owner">
    /// Repository owner
    /// </param>
    /// <param name="name">
    /// Name of the repository
    /// </param>
    /// <param name="auth">
    /// (Optional) authorization object
    /// </param>
    /// <returns></returns>
    IRemoteRepository Build(string server, string owner, string name, string auth);
  } /* End of Interface - IRemoteRepositoryFactory */
}/* End of document - IRemoteRepository.cs */