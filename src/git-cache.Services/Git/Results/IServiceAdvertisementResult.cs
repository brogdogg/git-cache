/******************************************************************************
 * File...: IServiceAdvertisementResult.cs
 * Remarks: 
 */
using Microsoft.AspNetCore.Mvc;

namespace git_cache.Services.Git.Results
{
  /************************** IServiceAdvertisementResult ********************/
  /// <summary>
  /// 
  /// </summary>
  public interface IServiceAdvertisementResult : IActionResult
  {
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Service ****************************************/
    /// <summary>
    /// Gets the service associated with the advertisement
    /// </summary>
    string Service { get; } /* End of Property - Service */

    /************************ Repository *************************************/
    /// <summary>
    /// 
    /// </summary>
    ILocalRepository Repository { get; } /* End of Property - Repository */
    /************************ Methods ****************************************/
  } /* End of Interface - IServiceAdvertisementResult */
}
/* End of document - IServiceAdvertisementResult.cs */