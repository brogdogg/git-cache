/******************************************************************************
 * File...: IRemoteRepository.cs
 * Remarks: 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace git_cache.Git
{
  /************************** GitContext *************************************/
  /// <summary>
  /// 
  /// </summary>
  public class GitContext : IGitContext
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    public ILocalRepositoryFactory LocalFactory { get; } = null;

    public IRemoteRepositoryFactory RemoteFactory { get; } = null;

    public IGitExecuter GitExecuter { get; } = null;

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
      ILocalRepositoryFactory localFactory,
      IRemoteRepositoryFactory remoteFactory,
      IGitExecuter gitExec,
      IGitLFSExecuter lfsExec)
    {
      if(null == (LocalFactory = localFactory))
        throw new ArgumentNullException("Local repository factory must be valid");
      if (null == (RemoteFactory = remoteFactory))
        throw new ArgumentNullException("Remote repository factory must be valid");
      if (null == (GitExecuter = gitExec))
        throw new ArgumentNullException("Git executor must be valid");
      if (null == (LFSExecuter = lfsExec))
        throw new ArgumentNullException("LFS executor must be valid");
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