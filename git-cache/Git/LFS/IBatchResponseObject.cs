/******************************************************************************
 * File...: IBatchResponseObject.cs
 * Remarks: 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace git_cache.Git.LFS
{
  /************************** IBatchResponseObject ***************************/
  /// <summary>
  /// 
  /// </summary>
  public interface IBatchResponseObject
  {
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    string Transfer { get; }
    ICollection<IResponseLFSItem> Objects { get; }
    /************************ Methods ****************************************/
  } /* End of Interface - IBatchResponseObject */

  /************************** IResponseLFSItem *******************************/
  /// <summary>
  /// Represents a response LFS item type
  /// </summary>
  public interface IResponseLFSItem : IItem
  {
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /// <summary>
    /// Gets a flag indicating if authenticated or not
    /// </summary>
    bool Authenticated { get; }
    /// <summary>
    /// Gets the actions associated with this response item
    /// </summary>
    IActions Actions { get; }
    /// <summary>
    /// Gets the error, if any, associated with this item
    /// </summary>
    IError Error { get; }
    /************************ Methods ****************************************/
  } /* End of Interface - IResponseLFSItem */
}
/* End of document - IBatchResponseObject.cs */