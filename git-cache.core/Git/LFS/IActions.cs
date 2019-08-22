/******************************************************************************
 * File...: IActions.cs
 * Remarks: 
 */
using System.Collections.Generic;

namespace git_cache.Services.Git.LFS
{
  /************************** ILFSActions ************************************/
  /// <summary>
  /// Represents the LFS actions
  /// </summary>
  public interface IActions
  {
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /// <summary>
    /// Gets the download action
    /// </summary>
    IDownloadAction Download { get; }
    /// <summary>
    /// Gets the upload action
    /// </summary>
    IUploadAction Upload { get; }
    /// <summary>
    /// Gets the verify action
    /// </summary>
    IVerifyAction Verify { get; }
    /************************ Methods ****************************************/
  } /* End of Interface - IActions */

  /************************** IAction ****************************************/
  /// <summary>
  /// Basic action type
  /// </summary>
  public interface IAction
  {
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /// <summary>
    /// Gets the HREF for the action
    /// </summary>
    string HREF { get; }
    /// <summary>
    /// Gets header values for the action
    /// </summary>
    Dictionary<string, string> Header { get; }
    /// <summary>
    /// Gets the expires in seconds
    /// </summary>
    int ExpiresIn { get; }
    /// <summary>
    /// Gets an expire time
    /// </summary>
    string ExpiresAt { get; }
    /************************ Methods ****************************************/
  } /* End of Interface - IAction */

  /************************** IDownloadAction ********************************/
  /// <summary>
  /// Represents a download action for LFS
  /// </summary>
  public interface IDownloadAction : IAction
  {
  } /* End of Interface - IDownloadAction */

  /************************** IUploadAction **********************************/
  /// <summary>
  /// Represents an upload action for LFS
  /// </summary>
  public interface IUploadAction : IAction
  {
  } /* End of Interface - IUploadAction */

  /************************** IVerifyAction **********************************/
  /// <summary>
  /// Represents a verify action for LFS
  /// </summary>
  public interface IVerifyAction : IAction
  {
  } /* End of Interface - IVerifyAction */
}
/* End of document - IActions.cs */