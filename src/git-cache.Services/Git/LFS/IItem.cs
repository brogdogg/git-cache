/******************************************************************************
 * File...: ILFSItem.cs
 * Remarks: 
 */

namespace git_cache.Services.Git.LFS
{
  /************************** ILFSItem ***************************************/
  /// <summary>
  /// Represents an LFS item
  /// </summary>
  public interface IItem
  {
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /// <summary>
    /// Gets the object ID for the LFS item
    /// </summary>
    string OID { get; }
    /// <summary>
    /// Gets the size of the item
    /// </summary>
    int Size { get; }
    /************************ Methods ****************************************/
  } /* End of Interface - ILFSItem */
}
/* End of document - ILFSItem.cs */