/******************************************************************************
 * File...: LFSError.cs
 * Remarks: 
 */
using System.Runtime.Serialization;

namespace git_cache.Services.Git.LFS
{
  /************************** LFSError ***************************************/
  /// <summary>
  /// Represents an error for LFS
  /// </summary>
  [DataContract(Name = "error")]
  public class LFSError : IError
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Code *******************************************/
    /// <summary>
    /// Gets/Sets the error code
    /// </summary>
    [DataMember(Name = "code")]
    public int Code { get; set; } = 0;
    /************************ Message ****************************************/
    /// <summary>
    /// Gets/Sets the error message
    /// </summary>
    [DataMember(Name = "message")]
    public string Message { get; set; } = "";
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /************************ Fields *****************************************/
    /************************ Static *****************************************/
  } /* End of Class - LFSError */
}
/* End of document - LFSError.cs */