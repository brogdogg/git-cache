using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace git_cache.Git.LFS
{

  /// <summary>
  /// Represents the de-serialized JSON string from the LFS request.
  /// </summary>
  [DataContract(Name = "BatchRequest")]
  public class BatchRequestObject
  {

    [DataContract(Name = "ref")]
    public class BatchRefObject
    {
      [DataMember(Name="name")]
      public string Name { get; set; }
    }

    [DataContract(Name = "object")]
    public class BatchItemObject
    {
      /// <summary>
      /// Gets/Sets the string OID of the LFS object
      /// </summary>
      [DataMember(Name = "oid")]
      public string OID { get; set; }
      
      /// <summary>
      /// Gets/Sets byte size of the LFS object. Must be at least zero.
      /// </summary>
      [DataMember(Name = "size")]
      public int Size { get; set; }
    }

    /// <summary>
    /// Gets/Sets the operation set by the request
    /// </summary>
    /// <remarks>
    /// Should be `download` or `upload`
    /// </remarks>
    [DataMember(Name = "operation")]
    public string Operation
    {
      get; set;
    } // end of property - Operation

    /// <summary>
    /// Gets/Sets the optional array of string identifiers for transfer adapters
    /// that the client has configured. If omitted, the `basic` transfer
    /// adapter MUST be assumed by the server.
    /// </summary>
    [DataMember(Name = "transfers")]
    public List<string> Transfers
    {
      get; set;
    } // end of property - Transfers

    /// <summary>
    /// Gets/Sets the optional object describing the server
    /// </summary>
    [DataMember(Name = "ref")]
    public BatchRefObject Ref
    {
      get; set;
    } // end of property - Ref


    /// <summary>
    /// Gets/Sets an array of objects to download
    /// </summary>
    [DataMember(Name = "objects")]
    public List<BatchItemObject> Objects
    {
      get; set;
    } // end of property - Objects

  } // end of class - BatchRequestObject

} // end of namespace - git_cache.Git.LFS

