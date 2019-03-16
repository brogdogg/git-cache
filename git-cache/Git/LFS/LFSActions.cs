using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace git_cache.Git.LFS
{

  /// <summary>
  /// Represents the LFS actions
  /// </summary>
  [DataContract]
  public class LFSActions
  {

    /// <summary>
    /// Basic action type
    /// </summary>
    public interface IAction
    {
      string HREF { get; }
      Dictionary<string, string> Header { get; }
      int ExpiresIn { get; }
      string ExpiresAt { get; }
    }

    /// <summary>
    /// Represents a download action
    /// </summary>
    [DataContract]
    public class DownloadAction : IAction
    {
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

    } // end of class - DownloadAction

    /// <summary>
    /// Represents the _upload_ action
    /// </summary>
    [DataContract]
    public class UploadAction : IAction
    {
      public string HREF => throw new NotImplementedException();
      public Dictionary<string, string> Header => throw new NotImplementedException();
      public int ExpiresIn => throw new NotImplementedException();
      public string ExpiresAt => throw new NotImplementedException();
    } // end of class - UploadAction

    /// <summary>
    /// Represents the _verify_ action
    /// </summary>
    [DataContract]
    public class VerifyAction : IAction
    {
      public string HREF => throw new NotImplementedException();
      public Dictionary<string, string> Header => throw new NotImplementedException();
      public int ExpiresIn => throw new NotImplementedException();
      public string ExpiresAt => throw new NotImplementedException();
    } // end of class - VerifyAction

    [DataMember(Name = "download", EmitDefaultValue = false)]
    public DownloadAction Download { get; set; }

    [DataMember(Name = "upload", EmitDefaultValue = false)]
    public UploadAction Upload { get; set; }

    [DataMember(Name = "verify", EmitDefaultValue = false)]
    public VerifyAction Verify { get; set; }

  } // end of class - LFSActions

} // end of namespace - git_cache.Git.LFS

