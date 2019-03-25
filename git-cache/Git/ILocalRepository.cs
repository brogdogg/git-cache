/******************************************************************************
 * File...: ILocalRepository.cs
 * Remarks: 
 */
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
    ILocalConfiguration Config { get; }
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
}/* End of document - ILocalRepository.cs */