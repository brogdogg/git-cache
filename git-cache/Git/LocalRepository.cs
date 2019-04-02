/******************************************************************************
 * File...: LocalRepository.cs
 * Remarks: 
 */
using git_cache.Configuration;
using Microsoft.Extensions.Configuration;
using System;

namespace git_cache.Git
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
    /************************ Config *****************************************/
    /// <summary>
    /// Gets the configuration associated with the local repository
    /// </summary>
    public IConfiguration Config { get; private set; } = null;
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
    public LocalRepository(IRemoteRepository remoteRepo, IConfiguration config)
    {
      if (null == (Remote = remoteRepo))
        throw new ArgumentNullException(
          "Must provide a remote repository object");
      if (null == (Config = config))
        throw new ArgumentNullException("Must provide a configuration item");
      Path = System.IO.Path.Combine(ConfigurationHelper.GetLocalStoragePath(Config),
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
}
/* End of document - LocalRepository.cs */
