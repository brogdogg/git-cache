/******************************************************************************
 * File...: ILocalRepository.cs
 * Remarks: 
 */
using Microsoft.Extensions.Configuration;

namespace git_cache.Git
{
  /************************** ILocalRepository *******************************/
  /// <summary>
  /// Local repository
  /// </summary>
  public interface ILocalRepository
  {
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Config *****************************************/
    /// <summary>
    /// Gets the local configuration associated with the local repository
    /// </summary>
    IConfiguration Config { get; }
    /************************ Path *******************************************/
    /// <summary>
    /// Gets the path for this local repository
    /// </summary>
    string Path { get; }
    /************************ Remote *****************************************/
    /// <summary>
    /// Gets the remote repository associated with this local repository
    /// </summary>
    IRemoteRepository Remote { get; }
    /************************ Methods ****************************************/
    /*----------------------- CreateLocalDirectory --------------------------*/
    /// <summary>
    /// Creates the local directory if it does not exists
    /// </summary>
    void CreateLocalDirectory();
  } /* End of Interface - ILocalRepository */

  /************************** ILocalRepositoryFactory ************************/
  /// <summary>
  /// Represents an object capable of building valid
  /// <see cref="ILocalRepository"/> objects.
  /// </summary>
  public interface ILocalRepositoryFactory
  {
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Methods ****************************************/
    /// <summary>
    /// Builds a <see cref="ILocalRepository"/> object
    /// </summary>
    /// <param name="repo">
    /// The <see cref="IRemoteRepository"/> object to base upon
    /// </param>
    /// <param name="config">
    /// Configuration item
    /// </param>
    /// <returns></returns>
    ILocalRepository Build(IRemoteRepository repo, IConfiguration config);
  } /* End of Interface - ILocalRepositoryFactory */
}/* End of document - ILocalRepository.cs */