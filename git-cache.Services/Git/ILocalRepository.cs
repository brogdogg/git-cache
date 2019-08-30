/******************************************************************************
 * File...: ILocalRepository.cs
 * Remarks: 
 */
using git_cache.Services.Configuration;
using Microsoft.Extensions.Configuration;

namespace git_cache.Services.Git
{
  /************************** ILocalRepository *******************************/
  /// <summary>
  /// Local repository
  /// </summary>
  public interface ILocalRepository
  {
    /************************ Events *****************************************/
    /************************ Properties *************************************/
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
  /// 
  /// </summary>
  public interface ILocalRepositoryFactory
  {

    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Methods ****************************************/
    ILocalRepository Build(IRemoteRepository repo, IGitCacheConfiguration config);
  } /* End of Interface - ILocalRepositoryFactory */

}/* End of document - ILocalRepository.cs */