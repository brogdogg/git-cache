/******************************************************************************
 * File...: ILFSError.cs
 * Remarks: 
 */

namespace git_cache.Git.LFS
{
  /************************** ILFSError **************************************/
  /// <summary>
  /// Represents an LFS error
  /// </summary>
  public interface IError
  {
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /// <summary>
    /// Gets the error code
    /// </summary>
    int Code { get; }
    /// <summary>
    /// Gets the error message
    /// </summary>
    string Message { get; }
    /************************ Methods ****************************************/
  } /* End of Interface - ILFSError */
}
/* End of document - ILFSError.cs */