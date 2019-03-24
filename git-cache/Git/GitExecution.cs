/******************************************************************************
 * File...: GitExecution.cs
 * Remarks: 
 */
using git_cache.Shell;
using System;
using System.Threading.Tasks;

namespace git_cache.Git
{
  /************************** GitExecution ***********************************/
  /// <summary>
  /// Extension class to allow git operations on a <see cref="LocalRepo"/>
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
    /*----------------------- Clone -----------------------------------------*/
    /// <summary>
    /// Clones the repository
    /// </summary>
    /// <param name="local">Local repo</param>
    public static string Clone(this LocalRepo local)
    {
      local.CreateLocalDirectory();
      return $"git clone --quiet --mirror \"{local.Remote.GitUrl}\" \"{local.Path}\"".Bash();
    } /* End of Function - Clone */

    /*----------------------- CloneAsync ------------------------------------*/
    /// <summary>
    /// Clone in async
    /// </summary>
    /// <param name="local">Local repo</param>
    public static Task<string> CloneAsync(this LocalRepo local)
    {
      return Task.Run(() => Clone(local));
    } /* End of Function - CloneAsync */

    /*----------------------- LFSFetch --------------------------------------*/
    /// <summary>
    /// LFS fetch in sync
    /// </summary>
    /// <param name="local">Local repo</param>
    public static string LFSFetch(this LocalRepo local)
    {
      return $"cd \"{local.Path}\" && git-lfs fetch".Bash();
    } /* End of Function - LFSFetch */

    /*----------------------- LFSFetchAsync ---------------------------------*/
    /// <summary>
    /// LFS fetch async
    /// </summary>
    /// <param name="local">Local repo</param>
    public static Task<string> LFSFetchAsync(this LocalRepo local)
    {
      return Task.Run(() => LFSFetch(local));
    } /* End of Function - LFSFetchAsync */

    /*----------------------- Fetch -----------------------------------------*/
    /// <summary>
    /// Fetches for the repository, assumes already exists...
    /// </summary>
    /// <param name="local">Local repository</param>
    public static string Fetch(this LocalRepo local)
    {
      $"git -C \"{local.Path}\" remote set-url origin \"{local.Remote.GitUrl}\"".Bash();
      return $"git -C \"{local.Path}\" fetch --quiet".Bash();
    } /* End of Function - Fetch */

    /*----------------------- FetchAsync ------------------------------------*/
    /// <summary>
    /// Fetches git information
    /// </summary>
    /// <param name="local">The local repo</param>
    public static async Task<string> FetchAsync(this LocalRepo local)
    {
      await $"git -C \"{local.Path}\" remote set-url origin \"{local.Remote.GitUrl}\"".BashAsync();
      return await $"git -C \"{local.Path}\" fetch --quiet".BashAsync();
    } /* End of Function - FetchAsync */

    /*----------------------- UpdateLocalAsync ------------------------------*/
    /// <summary>
    /// Updates the local storage in async fashion
    /// </summary>
    /// <param name="local">
    /// The local repository
    /// </param>
    public static async Task<string> UpdateLocalAsync(this LocalRepo local)
    {
      string output = null;
      try
      {
        // First try to fetch the details...
        output = await FetchAsync(local);
        await LFSFetchAsync(local);
      }
      catch (Exception)
      {
        // If fetch failed, then we must need to clone everything!
        output = await CloneAsync(local);
        await LFSFetchAsync(local);
      }
      return output;
    } /* End of Function - UpdateLocalAsync */
  } /* End of Class - GitExecution */

}
/* End of document - GitExecution.cs */