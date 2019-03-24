/******************************************************************************
 * File...: LFSActions.cs
 * Remarks: 
 */
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace git_cache.Git.LFS
{

  /************************** LFSActions *************************************/
  /// <summary>
  /// Represents the LFS actions
  /// </summary>
  [DataContract]
  public class LFSActions
  {
    /*======================= PUBLIC ========================================*/
    /************************ Types ******************************************/
    /// <summary>
    /// Basic action type
    /// </summary>
    public interface IAction
    {
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
      /// Gets the expires at value
      /// </summary>
      string ExpiresAt { get; }
    } // end of interface - IAction

    /************************ DownloadAction *********************************/
    /// <summary>
    /// Represents a download action
    /// </summary>
    [DataContract]
    public class DownloadAction : IAction
    {
      /*===================== PUBLIC ========================================*/
      /********************** Events *****************************************/
      /********************** Properties *************************************/
      /// <summary>
      /// URL to download the object from
      /// </summary>
      [DataMember(Name = "href")]
      public string HREF { get; set; }

      /// <summary>
      /// Gets/Sets an optional set of HTTP header key/value pairs to apply to the request
      /// </summary>
      [DataMember(Name = "header")]
      public Dictionary<string, string> Header { get; set; }

      /// <summary>
      /// Gets/Sets the number of whole seconds after local client time when transfer
      /// will expire.
      /// </summary>
      [DataMember(Name = "expires_in", EmitDefaultValue = false)]
      public int ExpiresIn { get; set; }

      /// <summary>
      /// Gets/Sets the ISO 8601 formatted timestamp for when the given action expires
      /// </summary>
      [DataMember(Name = "expires_at", EmitDefaultValue = false)]
      public string ExpiresAt { get; set; }

      /********************** Construction ***********************************/
      /********************** Methods ****************************************/
      /********************** Fields *****************************************/
      /********************** Static *****************************************/
    } // end of class - DownloadAction

    /************************ UploadAction ***********************************/
    /// <summary>
    /// Represents the _upload_ action
    /// </summary>
    [DataContract]
    public class UploadAction : IAction
    {
      /*===================== PUBLIC ========================================*/
      /********************** Events *****************************************/
      /********************** Properties *************************************/
      /********************** HREF *******************************************/
      /// <summary>
      /// Not implemented
      /// </summary>
      public string HREF => throw new NotImplementedException();
      /********************** Header *****************************************/
      /// <summary>
      /// Not implemented
      /// </summary>
      public Dictionary<string, string> Header => throw new NotImplementedException();
      /********************** ExpiresIn **************************************/
      /// <summary>
      /// Not implemented
      /// </summary>
      public int ExpiresIn => throw new NotImplementedException();
      /********************** ExpiresAt **************************************/
      /// <summary>
      /// Not implemented
      /// </summary>
      public string ExpiresAt => throw new NotImplementedException();
      /********************** Construction ***********************************/
      /********************** Methods ****************************************/
      /********************** Fields *****************************************/
      /********************** Static *****************************************/
    } // end of class - UploadAction

    /************************ VerifyAction ***********************************/
    /// <summary>
    /// Represents the _verify_ action
    /// </summary>
    [DataContract]
    public class VerifyAction : IAction
    {
      /*===================== PUBLIC ========================================*/
      /********************** Events *****************************************/
      /********************** Properties *************************************/
      /********************** HREF *******************************************/
      /// <summary>
      /// Not implemented
      /// </summary>
      public string HREF => throw new NotImplementedException();
      /********************** Header *****************************************/
      /// <summary>
      /// Not implemented
      /// </summary>
      public Dictionary<string, string> Header => throw new NotImplementedException();
      /********************** ExpiresIn **************************************/
      /// <summary>
      /// Not implemented
      /// </summary>
      public int ExpiresIn => throw new NotImplementedException();
      /********************** ExpiresAt **************************************/
      /// <summary>
      /// Not implemented
      /// </summary>
      public string ExpiresAt => throw new NotImplementedException();
      /********************** Construction ***********************************/
      /********************** Methods ****************************************/
      /********************** Fields *****************************************/
      /********************** Static *****************************************/
    } // end of class - VerifyAction

    /************************ Events *****************************************/
    /************************ Properties *************************************/

    /************************ Download ***************************************/
    /// <summary>
    /// Gets/Sets the download action
    /// </summary>
    [DataMember(Name = "download", EmitDefaultValue = false)]
    public DownloadAction Download { get; set; } /* End of Property - Download */

    /************************ Upload *****************************************/
    /// <summary>
    /// Gets/Sets the upload action
    /// </summary>
    [DataMember(Name = "upload", EmitDefaultValue = false)]
    public UploadAction Upload { get; set; } /* End of Property - Upload */

    /************************ Verify *****************************************/
    /// <summary>
    /// Gets/Sets the verify action
    /// </summary>
    [DataMember(Name = "verify", EmitDefaultValue = false)]
    public VerifyAction Verify { get; set; } /* End of Property - Verify */

    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /************************ Fields *****************************************/
    /************************ Static *****************************************/
  } // end of class - LFSActions

} // end of namespace - git_cache.Git.LFS

/* End of document - LFSActions.cs */