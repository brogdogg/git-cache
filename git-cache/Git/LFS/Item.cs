/******************************************************************************
 * File...: Item.cs
 * Remarks: 
 */
using System.Runtime.Serialization;

namespace git_cache.Git.LFS
{
  /************************** Item *******************************************/
  /// <summary>
  /// Represents a LFS request object and the base LFS response object
  /// </summary>
  [DataContract(Name = "object")]
  public class Item : IItem
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /// <summary>
    /// Gets/Sets the OID of the LFS object
    /// </summary>
    [DataMember(Name = "oid")]
    public string OID { get; set; } = null;

    /// <summary>
    /// Gets/Sets byte size of the LFS object. Must be at least zero.
    /// </summary>
    [DataMember(Name = "size")]
    public int Size { get; set; } = 0;

    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /************************ Fields *****************************************/
    /************************ Static *****************************************/
  } // end of class - LFSItem

} // end of namespace - git_cache.Git.LFS

/* End of document - Item.cs */