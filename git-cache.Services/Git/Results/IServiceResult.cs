/******************************************************************************
 * File...: IServiceResult.cs
 * Remarks: 
 */

using Microsoft.AspNetCore.Mvc;

namespace git_cache.Services.Git.Results
{
  /************************** IServiceResult *********************************/
  /// <summary>
  /// Represents the service result interface
  /// </summary>
  public interface IServiceResult : IActionResult
  {
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Service ****************************************/
    /************************ Repository *************************************/
    /// <summary>
    /// Gets the local repository
    /// </summary>
    ILocalRepository Repository { get; } /* End of Property - Repository */
    /// <summary>
    /// Gets the service name
    /// </summary>
    string Service { get; } /* End of Property - Service */
    /************************ UseGZip ****************************************/
    /// <summary>
    /// Gets a flag indicating if gzip should be used or not
    /// </summary>
    bool UseGZip { get; } /* End of Property - UseGZip */
    /************************ Methods ****************************************/
  } /* End of Interface - IServiceResult */
}
/* End of document - IServiceResult.cs */