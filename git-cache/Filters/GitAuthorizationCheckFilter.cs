/******************************************************************************
 * File...: GitAuthorizationCheckFilter.cs
 * Remarks: 
 */
using git_cache.Results;
using git_cache.Services.Git;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace git_cache.Filters
{
  /************************** GitAuthorizationCheckFilter ********************/
  /// <summary>
  /// Filter for checking to see if authentication is required for
  /// syncing
  /// </summary>
  public class GitAuthorizationCheckFilter : Attribute, IAsyncResourceFilter
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /*----------------------- GitAuthorizationCheckFilter -------------------*/
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="factory">
    /// Factory for creating the remote repository objects
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// If the factory is null
    /// </exception>
    public GitAuthorizationCheckFilter(IRemoteRepositoryFactory factory)
    {
      if (null == (m_factory = factory))
        throw new ArgumentNullException("Must provide a valid factory class");
    } /* End of Function - GitAuthorizationCheckFilter */
    /************************ Methods ****************************************/
    /*----------------------- OnResourceExecutionAsync ----------------------*/
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="next"></param>
    public async Task OnResourceExecutionAsync(
      ResourceExecutingContext context,
      ResourceExecutionDelegate next)
    {
      string auth = null;
      // Will look in the headers for an "Authentication" record
      var headers = context.HttpContext.Request.Headers;
      // If so, then we will get the value to use (assuming the first is
      // good enough, since there should just be one)
      if(headers.ContainsKey("Authentication"))
        auth = headers["Authentication"].First();
      // And we will need our repository object for checking
      var repo = m_factory.Build(
                      context.RouteData.Values["destinationServer"].ToString(),
                      context.RouteData.Values["repositoryOwner"].ToString(),
                      context.RouteData.Values["repository"].ToString(),
                      auth);
      // Check to see if we have authority to access the repository
      var result = await CheckAuthorizationAsync(repo);
      // If we got a result back, it is the forwarded response from
      // the check, so go ahead and use it and shortcut to the end of the
      // pipeline
      if(null != result)
        context.Result = result;
      // otherwise, we are good, let the pipeline continue
      else
        await next();
    } /* End of Function - OnResourceExecutionAsync */
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
    /*----------------------- AuthenticateAsync -----------------------------*/
    /// <summary>
    /// 
    /// </summary>
    /// <param name="repo"></param>
    private async Task<HttpResponseMessage> AuthenticateAsync(IRemoteRepository repo)
    {
      // Create a HTTP client to work with
      HttpClient client = new HttpClient();
      // Clear out the headers and setup our own
      client.DefaultRequestHeaders.Accept.Clear();
      // Attach the authentication if we have it
      if (null != repo.Auth)
        client.DefaultRequestHeaders.Authorization
          = new AuthenticationHeaderValue(repo.Auth.Scheme,
                                          repo.Auth.Encoded);
      // Pretend we are a git client
      client.DefaultRequestHeaders.UserAgent.Add(
        new ProductInfoHeaderValue("git", "1.0"));
      // Finally go ahead and authenticate against the info/refs url
      string url = $"{repo.Url}/info/refs?service=git-upload-pack";
      return await client.GetAsync(url);
    } // end of function - AuthenticateAsync

    /*----------------------- CheckAuthorizationAsync -----------------------*/
    /// <summary>
    /// 
    /// </summary>
    /// <param name="repo"></param>
    private async Task<ActionResult> CheckAuthorizationAsync(IRemoteRepository repo)
    {
      ActionResult retval = null;
      // Authenticate the repository
      var resp = await AuthenticateAsync(repo);
      // If we were not successful, then we will forward the result back to the
      // user
      if (System.Net.HttpStatusCode.OK != resp.StatusCode)
        retval = new ForwardedResult(resp);
      return retval;
    } // end of function - CheckAuthorizationAsync


    /************************ Fields *****************************************/
    IRemoteRepositoryFactory m_factory;
    /************************ Static *****************************************/
  } /* End of Class - GitAuthorizationCheckFilter */
}
/* End of document - GitAuthorizationCheckFilter.cs */