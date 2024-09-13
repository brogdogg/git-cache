/******************************************************************************
 * File...: IRemoteRepository.cs
 * Remarks: 
 */
using git_cache.Services.Configuration;
using System;
using System.Threading.Tasks;

namespace git_cache.Services.Git
{
  /************************** GitContext *************************************/
  /// <summary>
  /// Basic implementation of the <see cref="IGitContext"/> interface
  /// </summary>
  public class GitContext : IGitContext
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ LocalFactory ***********************************/
    /// <summary>
    /// Gets the git-cache configuration object
    /// </summary>
    public IGitCacheConfiguration Configuration { get; } = null;
    /// <summary>
    /// Gets the local repository factory
    /// </summary>
    public ILocalRepositoryFactory LocalFactory { get; } = null;

    /************************ RemoteFactory **********************************/
    /// <summary>
    /// Gets the remote repository factory
    /// </summary>
    public IRemoteRepositoryFactory RemoteFactory { get; } = null;

    /************************ GitExecuter ************************************/
    /// <summary>
    /// Gets the git executer
    /// </summary>
    public IGitExecuter GitExecuter { get; } = null;

    /************************ LFSExecuter ************************************/
    /// <summary>
    /// Gets the LFS executer
    /// </summary>
    public IGitLFSExecuter LFSExecuter { get; } = null;
    /************************ Construction ***********************************/
    /*----------------------- GitContext ------------------------------------*/
    /// <summary>
    /// Injection constructor
    /// </summary>
    /// <param name="localFactory">
    /// Local repository factory
    /// </param>
    /// <param name="remoteFactory">
    /// Remote repository factory
    /// </param>
    /// <param name="gitExec">
    /// Git executor
    /// </param>
    /// <param name="lfsExec">
    /// LFS executor
    /// </param>
    public GitContext(
      IGitCacheConfiguration gitCacheConfiguration,
      ILocalRepositoryFactory localFactory,
      IRemoteRepositoryFactory remoteFactory,
      IGitExecuter gitExec,
      IGitLFSExecuter lfsExec)
    {
      if (null == (Configuration = gitCacheConfiguration))
        throw new ArgumentNullException(nameof(gitCacheConfiguration), "Missing git-cache configuration");
      if (null == (LocalFactory = localFactory))
        throw new ArgumentNullException(nameof(localFactory), "Missing required local repository factory");
      if (null == (RemoteFactory = remoteFactory))
        throw new ArgumentNullException(nameof(remoteFactory),"Remote repository factory must be valid");
      if (null == (GitExecuter = gitExec))
        throw new ArgumentNullException(nameof(gitExec), "Git executor must be valid");
      if (null == (LFSExecuter = lfsExec))
        throw new ArgumentNullException(nameof(lfsExec), "LFS executor must be valid");
    } /* End of Function - GitContext */
    /************************ Methods ****************************************/
    /*----------------------- UpdateLocalAsync ------------------------------*/
    /// <summary>
    /// Updates the local storage in async fashion
    /// </summary>
    /// <param name="local">
    /// The local repository
    /// </param>
    public async Task<string> UpdateLocalAsync(ILocalRepository local)
    {
      string output = null;
      try
      {
        // First try to fetch the details...
        output = await GitExecuter.FetchAsync(local);
        await LFSExecuter.FetchAsync(local);
      }
      catch (Exception)
      {
        // If fetch failed, then we must need to clone everything!
        output = await GitExecuter.CloneAsync(local);
        await LFSExecuter.FetchAsync(local);
      }
      return output;
    } /* End of Function - UpdateLocalAsync */
    /************************ Fields *****************************************/
    /************************ Static *****************************************/

    /*======================= PROTECTED =====================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /************************ Fields *****************************************/
    /************************ Static *****************************************/

    /*======================= PRIVATE =======================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /************************ Fields *****************************************/
    /************************ Static *****************************************/
  } /* End of Class - GitContext */
}
/* End of document - GitContext.cs */