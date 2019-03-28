/******************************************************************************
 * File...: 
 * Remarks: 
 */
using System.Runtime.Serialization;

namespace git_cache.Git.LFS
{
  /************************** LFSItem ****************************************/
  /// <summary>
  /// Represents a LFS request object and the base LFS response object
  /// </summary>
  [DataContract]
  public class LFSItem
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

/* End of document - LFSItem.cs */