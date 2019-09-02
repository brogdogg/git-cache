/******************************************************************************
 * File...: LocalRepository.cs
 * Remarks: 
 */
using git_cache.Services.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;

namespace git_cache.Services.Git
{
  /************************** LocalRepository ********************************/
  /// <summary>
  /// Represents a local repository, mirroring a remote repository
  /// </summary>
  public class LocalRepository : ILocalRepository
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Remote *****************************************/
    /// <summary>
    /// Gets the remote repository associated with the local repository
    /// </summary>
    public IRemoteRepository Remote { get; private set; } = null;
    /************************ Path *******************************************/
    /// <summary>
    /// Gets the local path for the repository data
    /// </summary>
    public string Path { get; private set; } = null;
    /************************ Construction ***********************************/
    /*----------------------- LocalRepo -------------------------------------*/
    /// <summary>
    /// Constructor, taking a remote repository and local configuration object
    /// </summary>
    /// <param name="remoteRepo">
    /// A remote repository to setup a local one for
    /// </param>
    /// <param name="config">
    /// A configuration object to use for the local repository
    /// </param>
    public LocalRepository(IRemoteRepository remoteRepo, IGitCacheConfiguration config)
    {
      if (null == (Remote = remoteRepo))
        throw new ArgumentNullException(
          nameof(remoteRepo),
          "Must provide a remote repository object");
      if (null == config)
        throw new ArgumentNullException(
          nameof(config),
          "Must provide a valid configuration item");
      Path = System.IO.Path.Combine(config.LocalStoragePath,
                                    Remote.Server,
                                    Remote.Owner,
                                    Remote.Name);
    } /* End of Function - LocalRepo */
    /************************ Methods ****************************************/
    /*----------------------- CreateLocalDirectory --------------------------*/
    /// <summary>
    /// Creates the local directory for the path
    /// </summary>
    public void CreateLocalDirectory()
    {
      if (!System.IO.Directory.Exists(Path))
      {
        var dir = System.IO.Directory.CreateDirectory(Path);
        if (!dir.Exists)
          dir.Create();
      } // end of if - path does not exists
    } /* End of Function - CreateLocalDirectory */
    /************************ Fields *****************************************/
    /************************ Static *****************************************/
  } /* End of Class - LocalRepository */

  /************************** LocalRepositoryFactory *************************/
  /// <summary>
  /// 
  /// </summary>
  public class LocalRepositoryFactory : ILocalRepositoryFactory
  {

    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /*----------------------- Build -----------------------------------------*/
    /// <summary>
    /// 
    /// </summary>
    /// <param name="repo"></param>
    /// <param name="config"></param>
    public ILocalRepository Build(
      IRemoteRepository repo,
      IGitCacheConfiguration config)
    {
      return new LocalRepository(repo, config);
    } /* End of Function - Build */
    /************************ Fields *****************************************/
    /************************ Static *****************************************/

  } /* End of Class - LocalRepositoryFactory */
}
/* End of document - LocalRepository.cs */
