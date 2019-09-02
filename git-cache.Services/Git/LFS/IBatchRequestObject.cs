/******************************************************************************
 * File...: IBatchRequestObject.cs
 * Remarks: 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace git_cache.Services.Git.LFS
{
  /************************** IBatchRequestObject ****************************/
  /// <summary>
  /// Describes a batch request object
  /// </summary>
  public interface IBatchRequestObject
  {
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Transfer ***************************************/
    string Operation { get; }
    ICollection<string> Transfers { get; }
    IBatchRefObject Ref { get; }
    ICollection<IItem> Objects { get; }
    /************************ Methods ****************************************/
  } /* End of Interface - IBatchRequestObject */

  /************************** IBatchRefObject ********************************/
  /// <summary>
  /// 
  /// </summary>
  public interface IBatchRefObject
  {
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    string Name { get; }
    /************************ Methods ****************************************/
  } /* End of Interface - IBatchRefObject */
}
/* End of document - IBatchRequestObject.cs */
