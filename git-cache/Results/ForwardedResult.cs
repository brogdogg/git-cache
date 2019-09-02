/******************************************************************************
 * File...: ForwardedResult.cs
 * Remarks: 
 */
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Linq;
using System.Net.Http;

namespace git_cache.Results
{
  /************************** ForwardedResult ********************************/
  /// <summary>
  /// Forwarded result class, extending the <see cref="ActionResult"/>
  /// </summary>
  public class ForwardedResult : ActionResult
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /*----------------------- ForwardedResult -------------------------------*/
    /// <summary>
    /// Constructor for the forwarded results
    /// </summary>
    /// <param name="baseResult"></param>
    public ForwardedResult(HttpResponseMessage baseResult)
    {
      BaseResponse = baseResult;
    } /* End of Function - ForwardedResult */
    /************************ Methods ****************************************/

    /*----------------------- ExecuteResult ---------------------------------*/
    /// <summary>
    /// Executes the result
    /// </summary>
    /// <param name="context"></param>
    public override void ExecuteResult(ActionContext context)
    {
      var response = context.HttpContext.Response;
      response.StatusCode = (int)BaseResponse.StatusCode;
      foreach (var header in BaseResponse.Headers)
        response.Headers.Add(
          header.Key,
          new StringValues(header.Value.ToArray()));
    } /* End of Function - ExecuteResult */
    /************************ Fields *****************************************/
    /************************ Static *****************************************/

    /*======================= PROTECTED =====================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /************************ Fields *****************************************/
    /************************ Static *****************************************/

    /*======================= PRIVATE =======================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ BaseResponse ***********************************/
    /// <summary>
    /// Gets the base response
    /// </summary>
    private HttpResponseMessage BaseResponse { get; } /* End of Property - BaseResponse */
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /************************ Fields *****************************************/
    /************************ Static *****************************************/

  } /* End of Class - ForwardedResult */
}
/* End of document - ForwardedResult.cs */