/******************************************************************************
 * File...: GitExecution.cs
 * Remarks: 
 */
using git_cache.Services.Shell;
using System;
using System.Threading.Tasks;

namespace git_cache.Services.Git
{

  /************************** GitExecuter ************************************/
  /// <summary>
  /// 
  /// </summary>
  public class GitExecuter : IGitExecuter
  {

    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    public IShell Shell { get; } = null;
    /************************ Construction ***********************************/
    public GitExecuter(IShell shell) { Shell = shell; }
    /************************ Methods ****************************************/
    /************************ Fields *****************************************/
    /*----------------------- Clone -----------------------------------------*/
    /// <summary>
    /// 
    /// </summary>
    /// <param name="local"></param>
    public string Clone(ILocalRepository local)
    {
      local.CreateLocalDirectory();
      return Shell.Execute($"git clone --quiet --mirror \"{local.Remote.GitUrl}\" \"{local.Path}\"");
    } /* End of Function - Clone */

    /*----------------------- CloneAsync ------------------------------------*/
    /// <summary>
    /// 
    /// </summary>
    /// <param name="local"></param>
    public Task<string> CloneAsync(ILocalRepository local)
    {
      return Task<string>.Factory.StartNew(() => Clone(local));
    } /* End of Function - CloneAsync */

    /*----------------------- Fetch -----------------------------------------*/
    /// <summary>
    /// 
    /// </summary>
    /// <param name="local"></param>
    public string Fetch(ILocalRepository local, bool doDryRun = false)
    {
      Shell.Execute($"git -C \"{local.Path}\" remote set-url origin \"{local.Remote.GitUrl}\"");
      return Shell.Execute($"git -C \"{local.Path}\" fetch --prune"
        + (doDryRun ? " --verbose --dry-run 2>&1" : " --quiet"));
    } /* End of Function - Fetch */

    /*----------------------- FetchAsync ------------------------------------*/
    /// <summary>
    /// 
    /// </summary>
    /// <param name="local"></param>
    public Task<string> FetchAsync(ILocalRepository local, bool doDryRun = false)
    {
      return Task<string>.Factory.StartNew(() => Fetch(local, doDryRun));
    } /* End of Function - FetchAsync */
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
  } /* End of Class - GitExecuter */

  /************************** GitLFSExecutor *********************************/
  /// <summary>
  /// 
  /// </summary>
  public class GitLFSExecutor : IGitLFSExecuter
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    public IShell Shell { get; } = null;
    /************************ Construction ***********************************/
    public GitLFSExecutor(IShell shell) { Shell = shell; }
    /************************ Methods ****************************************/
    /*----------------------- Fetch -----------------------------------------*/
    /// <summary>
    /// 
    /// </summary>
    /// <param name="local"></param>
    public string Fetch(ILocalRepository local)
    {
      return Shell.Execute($"cd \"{local.Path}\" && git-lfs fetch");
    } /* End of Function - Fetch */

    /*----------------------- FetchAsync ------------------------------------*/
    /// <summary>
    /// 
    /// </summary>
    /// <param name="local"></param>
    public Task<string> FetchAsync(ILocalRepository local)
    {
      return Task.Run(() => Fetch(local));
    } /* End of Function - FetchAsync */
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
  } /* End of Class - GitLFSExecutor */
}
/* End of document - GitExecution.cs */