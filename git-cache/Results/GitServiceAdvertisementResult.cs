/******************************************************************************
 * File...: GitServiceAdvertisementResult.cs
 * Remarks: 
 */
using git_cache.Git;
using git_cache.Shell;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace git_cache.Results
{
  /************************** GitServiceAdvertisementResult ******************/
  /// <summary>
  /// A HTTP response representing a git service advertisement result
  /// </summary>
  public class GitServiceAdvertisementResult : ActionResult
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Service ****************************************/
    /// <summary>
    /// Gets the service associated with the result
    /// </summary>
    public string Service { get; } /* End of Property - Service */
    /************************ Repository *************************************/
    /// <summary>
    /// Gets the repository associated with the result
    /// </summary>
    public LocalRepo Repository { get; } /* End of Property - Repository */
    /************************ Construction ***********************************/

    /*----------------------- GitServiceAdvertisementResult -----------------*/
    /// <summary>
    /// Constructor for the service advertisement result
    /// </summary>
    /// <param name="service">
    /// Service the result is for
    /// </param>
    /// <param name="repo">
    /// The repository the result is for
    /// </param>
    public GitServiceAdvertisementResult(string service, LocalRepo repo)
    {
      Service = service;
      if (null == (Repository = repo))
        throw new ArgumentNullException("Invalid repository given, must not be null");
      return;
    } /* End of Function - GitServiceAdvertisementResult */

    /************************ Methods ****************************************/
    /*----------------------- ExecuteResult ---------------------------------*/
    /// <summary>
    /// Executes the logic to create the result
    /// </summary>
    /// <param name="context">
    /// The context for the action
    /// </param>
    public override void ExecuteResult(ActionContext context)
    {
      var response = context.HttpContext.Response;
      response.StatusCode = 200;
      response.ContentType = $"application/x-{Service}-advertisement";
      response.Headers.Add("Cache-Control", "no-cache");
      var bytes = Encoding.UTF8.GetBytes($"001e# service={Service}\n0000");
      response.Body.Write(bytes, 0, bytes.Length);
      $"{Service} --stateless-rpc --advertise-refs \"{Repository.Path}\"".Bash((code) => code != 0, response.Body);
      return;
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
    /************************ GetBodyContentsAsync ***************************/
    /// <summary>
    /// Gets the functor for getting the body contents asynchronously
    /// </summary>
    private Func<Stream, Task<int>> GetBodyContentsAsync { get; } /* End of Property - GetBodyContentsAsync */
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /************************ Fields *****************************************/
    /************************ Static *****************************************/

  } /* End of Class - GitServiceAdvertisementResult */
}
/* End of document - GitServiceAdvertisementResult.cs */