/******************************************************************************
 * File...: ServiceAdvertisementResult.cs
 * Remarks: 
 */
using git_cache.Services.Shell;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;

namespace git_cache.Services.Git.Results
{
  /************************** ServiceAdvertisementResult *********************/
  /// <summary>
  /// A HTTP response representing a git service advertisement result
  /// </summary>
  public class ServiceAdvertisementResult : ActionResult
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
    public ILocalRepository Repository { get; } /* End of Property - Repository */

    /************************ Shell ******************************************/
    /// <summary>
    /// 
    /// </summary>
    public IShell Shell { get; } /* End of Property - Shell */
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
    public ServiceAdvertisementResult(
      string service,
      ILocalRepository repo,
      IShell shell)
    {
      Service = service;
      if (null == (Repository = repo))
        throw new ArgumentNullException(nameof(repo), "Invalid repository given, must not be null");
      if (null == (Shell = shell))
        throw new ArgumentNullException(nameof(shell), "Invalid shell object givin, must not be null");
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
      Shell.Execute($"{Service} --stateless-rpc --advertise-refs \"{Repository.Path}\"",
        (code) => code != 0, response.Body);
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
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /************************ Fields *****************************************/
    /************************ Static *****************************************/

  } /* End of Class - ServiceAdvertisementResult */
}
/* End of document - ServiceAdvertisementResult.cs */