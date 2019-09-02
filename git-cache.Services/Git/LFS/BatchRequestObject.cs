/******************************************************************************
 * File...: BatchRequestObject.cs
 * Remarks: 
 */
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace git_cache.Services.Git.LFS
{

  /************************** BatchRequestObject *****************************/
  /// <summary>
  /// Represents the de-serialized JSON string from the LFS request.
  /// </summary>
  [DataContract(Name = "BatchRequest")]
  public class BatchRequestObject : IBatchRequestObject
  {
    /*======================= PUBLIC ========================================*/
    /************************ Types ******************************************/
    /************************ BatchRefObject *********************************/
    /// <summary>
    /// Represents the batch reference object
    /// </summary>
    [DataContract(Name = "ref")]
    public class BatchRefObject: IBatchRefObject
    {
      /*===================== PUBLIC ========================================*/
      /********************** Events *****************************************/
      /********************** Properties *************************************/
      /********************** Name *******************************************/
      /// <summary>
      /// Gets/Sets the name of the batch reference object
      /// </summary>
      [DataMember(Name="name")]
      public string Name { get; set; } /* End of Property - Name */
      /********************** Construction ***********************************/
      /********************** Methods ****************************************/
      /********************** Fields *****************************************/
      /********************** Static *****************************************/
    } /* End of Class - BatchRefObject */

    /************************ Events *****************************************/
    /************************ Properties *************************************/
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
    public List<Item> Objects
    {
      get; set;
    } // end of property - Objects

    ICollection<string> IBatchRequestObject.Transfers { get { return Transfers; } }

    IBatchRefObject IBatchRequestObject.Ref { get { return Ref; } }

    ICollection<IItem> IBatchRequestObject.Objects
    {
      get
      {
        return Objects.Select(v => v as IItem).ToList();
      }
    }
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /************************ Fields *****************************************/
    /************************ Static *****************************************/
  } // end of class - BatchRequestObject

} // end of namespace - git_cache.Git.LFS

/* End of document - BatchRequestObject.cs */