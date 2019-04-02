/******************************************************************************
 * File...: BatchResponseObject.cs
 * Remarks: 
 */
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace git_cache.Git.LFS
{
  /************************** BatchResponseObject ****************************/
  /// <summary>
  /// Represents the batch response object for LFS BATCH API
  /// </summary>
  [DataContract(Name = "response")]
  public class BatchResponseObject
  {

    /*======================= PUBLIC ========================================*/
    /************************ Types ******************************************/
    /************************ ResponseLFSItem ********************************/
    /// <summary>
    /// Represents a LFS object as represented in a response
    /// </summary>
    public class ResponseLFSItem : Item, IResponseLFSItem
    {
      /*===================== PUBLIC ========================================*/
      /********************** Events *****************************************/
      /********************** Properties *************************************/
      /// <summary>
      /// Gets/Sets an optional boolean specifying whether the request for
      /// this object is authenticated or not.
      /// </summary>
      public bool Authenticated { get; set; } = false;

      /// <summary>
      /// Gets/Sets the object containing the next actions for this object.
      /// </summary>
      [DataMember(Name = "actions", EmitDefaultValue = false)]
      public LFSActions Actions { get; set; }

      /// <summary>
      /// Gets/Sets the error, if any, associated with the item
      /// </summary>
      [DataMember(Name ="error", EmitDefaultValue = false)]
      public LFSError Error { get; set; } = null;

      /// <summary>
      /// Gets the underlying authenticated flag
      /// </summary>
      bool IResponseLFSItem.Authenticated { get { return Authenticated; } }

      /// <summary>
      /// Gets the underlying actions
      /// </summary>
      IActions IResponseLFSItem.Actions { get { return Actions; } }

      /// <summary>
      /// Gets the underlying error
      /// </summary>
      IError IResponseLFSItem.Error { get { return Error; } }

      /********************** Construction ***********************************/
      /********************** Methods ****************************************/
      /********************** Fields *****************************************/
      /********************** Static *****************************************/
    } // end of class - ResponseLFSItem
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /// <summary>
    /// Gets/Sets the transfer adapter that the server prefers. This MUST
    /// be one of the given transfer identifiers from the request.
    /// </summary>
    /// <remarks>
    /// _basic_ is assumed if nothing is specified.
    /// </remarks>
    [DataMember(Name = "transfer")]
    public string Transfer { get; set; } = "basic";

    /// <summary>
    /// Gets/Sets the list of objects to download
    /// </summary>
    [DataMember(Name = "objects")]
    public List<ResponseLFSItem> Objects { get; set; } = new List<ResponseLFSItem>();
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /************************ Fields *****************************************/
    /************************ Static *****************************************/

  } // end of class - BatchResponseObject

} // end of namespace - git_cache.Git.LFS

/* End of document - BatchResponseObject.cs */