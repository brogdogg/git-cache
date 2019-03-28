/******************************************************************************
 * File...: GitExecution.cs
 * Remarks: 
 */
using git_cache.Shell;
using System;
using System.Threading.Tasks;

namespace git_cache.Git
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
    /************************ Construction ***********************************/
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
      return $"git clone --quiet --mirror \"{local.Remote.GitUrl}\" \"{local.Path}\"".Bash();
    } /* End of Function - Clone */

    /*----------------------- CloneAsync ------------------------------------*/
    /// <summary>
    /// 
    /// </summary>
    /// <param name="local"></param>
    public Task<string> CloneAsync(ILocalRepository local)
    {
      return Task.Run(() => Clone(local));
    } /* End of Function - CloneAsync */

    /*----------------------- Fetch -----------------------------------------*/
    /// <summary>
    /// 
    /// </summary>
    /// <param name="local"></param>
    public string Fetch(ILocalRepository local)
    {
      $"git -C \"{local.Path}\" remote set-url origin \"{local.Remote.GitUrl}\"".Bash();
      return $"git -C \"{local.Path}\" fetch --quiet".Bash();
    } /* End of Function - Fetch */

    /*----------------------- FetchAsync ------------------------------------*/
    /// <summary>
    /// 
    /// </summary>
    /// <param name="local"></param>
    public async Task<string> FetchAsync(ILocalRepository local)
    {
      await $"git -C \"{local.Path}\" remote set-url origin \"{local.Remote.GitUrl}\"".BashAsync();
      return await $"git -C \"{local.Path}\" fetch --quiet".BashAsync();
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
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /*----------------------- Fetch -----------------------------------------*/
    /// <summary>
    /// 
    /// </summary>
    /// <param name="local"></param>
    public string Fetch(ILocalRepository local)
    {
      return $"cd \"{local.Path}\" && git-lfs fetch".Bash();
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

  /************************** GitExecution ***********************************/
  /// <summary>
  /// Extension class to allow git operations on a <see cref="ILocalRepository"/>
  /// object.
  /// </summary>
  public static class GitExecution
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /************************ Fields *****************************************/
    /************************ Static *****************************************/
    /*----------------------- UpdateLocalAsync ------------------------------*/
    /// <summary>
    /// Updates the local storage in async fashion
    /// </summary>
    /// <param name="local">
    /// The local repository
    /// </param>
    //public static async Task<string> UpdateLocalAsync(this ILocalRepository local)
    //{
    //  string output = null;
    //  try
    //  {
    //    // First try to fetch the details...
    //    output = await FetchAsync(local);
    //    await LFSFetchAsync(local);
    //  }
    //  catch (Exception)
    //  {
    //    // If fetch failed, then we must need to clone everything!
    //    output = await CloneAsync(local);
    //    await LFSFetchAsync(local);
    //  }
    //  return output;
    //} /* End of Function - UpdateLocalAsync */
  } /* End of Class - GitExecution */

}
/* End of document - GitExecution.cs */