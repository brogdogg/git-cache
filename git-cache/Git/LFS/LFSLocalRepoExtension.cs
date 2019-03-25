/******************************************************************************
 * File...: LFSLocalRepoExtension
 * Remarks: 
 */
using System;

namespace git_cache.Git.LFS
{
  /************************** LFSLocalRepoExtension **************************/
  /// <summary>
  /// Extension class for the <see cref="ILocalRepository"/>
  /// </summary>
  public static class LFSLocalRepoExtension
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /************************ Fields *****************************************/
    /************************ Static *****************************************/
    /*----------------------- LFSObjectPath ---------------------------------*/
    /// <summary>
    /// Gets an LFS object path in the local repo
    /// </summary>
    /// <param name="repo">Local repo</param>
    /// <param name="oid">LFS object ID</param>
    public static string LFSObjectPath(this ILocalRepository repo, string oid)
    {
      if (oid == null)
        throw new ArgumentNullException("OID must not be null");
      if (oid.Length != 64)
        throw new ArgumentException("OID does not appear to be correct length");
      var objFilePath = $"lfs/objects/{oid.Substring(0, 2)}/{oid.Substring(2, 2)}/{oid}";
      var fullLfsObjectPath = System.IO.Path.Combine(repo.Path, objFilePath);
      return fullLfsObjectPath;
    } /* End of Function - LFSObjectPath */
  } /* End of Class - LFSLocalRepoExtension */
}
/* End of document - LFSLocalRepoExtension.cs */