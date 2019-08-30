/******************************************************************************
 * File...: GitController.cs
 * Remarks: 
 */
using git_cache.Services.Git;
using git_cache.Services.Git.LFS;
using git_cache.Services.Git.Results;
using git_cache.Results;
using git_cache.Services.Shell;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using git_cache.Filters;

namespace git_cache.Controllers
{

  /************************** GitController **********************************/
  /// <summary>
  /// Controller for git operations
  /// </summary>
  [Produces("application/json")]
  [Route("api/Git")]
  [TypeFilter(typeof(ResourceLockFilter))]
  public class GitController : Controller
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Configuration **********************************/
    /// <summary>
    /// 
    /// </summary>
    IGitContext GitContext { get; } = null;
    /// <summary>
    /// Gets the shell object
    /// </summary>
    IShell Shell { get; } = null;
    ILogger<GitController> Logger { get; } = null;
    /************************ Construction ***********************************/
    /*----------------------- GitController ---------------------------------*/
    /// <summary>
    /// Constructor for dependency injection
    /// </summary>
    /// <param name="config">
    /// application configuration object
    /// </param>
    /// <param name="localFactory">
    /// Factory for building <see cref="ILocalRepository"/> objects
    /// </param>
    /// <param name="remoteFactory">
    /// Factory for building <see cref="IRemoteRepository"/> objects
    /// </param>
    public GitController(
      IGitContext gitContext,
      IShell shell,
      ILogger<GitController> logger)
      : base()
    {
      if (null == (GitContext = gitContext))
        throw new ArgumentNullException("A valid git context must be provided");
      if (null == (Shell = shell))
        throw new ArgumentNullException("A valid shell object must be provided");
      if (null == (Logger = logger))
        throw new ArgumentNullException("A valid logger must be provided");
    } // end of function - GitController

    /************************ Methods ****************************************/
    [HttpGet("{destinationServer}/{repositoryOwner}/{repository}/fetch")]
    public async Task<ActionResult> FetchAsync(
      string destinationServer,
      string repositoryOwner,
      string repository,
      [FromQuery]string service,
      [FromHeader]string authorization)
    {
      ActionResult retval = null;
      var repo = GitContext.RemoteFactory.Build(destinationServer,
        repositoryOwner,
        repository,
        authorization);

      // Verify the authorization is OK, if not then return the error response
      // from the actual git server
      if (null != (retval = await CheckAuthorizationAsync(repo)))
        return retval;
      // Create a local repository based on the remote repo
      var local = GitContext.LocalFactory.Build(repo, GitContext.Configuration);
      retval = new JsonResult(Shell.Execute($"git -C \"{local.Path}\" log -2 --pretty=%h"));
      return retval;

    }
    // GET: api/Git/{server}/{repoOwner}/{repo}/info/refs?service={service}
    /// <summary>
    /// HTTP GET handler for fetch/clones from git
    /// </summary>
    /// <param name="destinationServer">
    /// Actual server to clone/fetch from
    /// </param>
    /// <param name="repositoryOwner">Owner of repository</param>
    /// <param name="repository">Repository name</param>
    /// <param name="service">Service from git</param>
    /// <returns>JSON Result (for now)</returns>
    [HttpGet("{destinationServer}/{repositoryOwner}/{repository}/info/refs", Name = "Get")]
    public async Task<ActionResult> GetAsync(
      string destinationServer,
      string repositoryOwner,
      string repository,
      [FromQuery]string service,
      [FromHeader]string authorization)
    {
      Logger.LogTrace($"GET for: server={destinationServer}; owner={repositoryOwner}; repo={repository}");
      ActionResult retval = null;
      var repo = GitContext.RemoteFactory.Build(destinationServer,
        repositoryOwner,
        repository,
        authorization);

      // Verify the authorization is OK, if not then return the error response
      // from the actual git server
      if (null != (retval = await CheckAuthorizationAsync(repo)))
      {
        Logger.LogInformation("Authorization required, can't continue");
        return retval;
      }
      // Create a local repository based on the remote repo
      var local = GitContext.LocalFactory.Build(repo, GitContext.Configuration);
      // Updates our local cache, if it has never been downloaded then it
      // will be cloned, otherwise just a fetch is performed to update
      // the local copy
      await GitContext.UpdateLocalAsync(local);
      // Then create a custom git service advertisement result to send
      // back to the client, basically forwarding everything we just
      // updated to the client now
      retval = new ServiceAdvertisementResult(service, local, Shell);
      return retval;
    } // end of function - GetAsync

    /// <summary>
    /// HTTP POST handler for receiving from client 
    /// </summary>
    /// <param name="destinationServer"></param>
    /// <param name="repositoryOwner"></param>
    /// <param name="repository"></param>
    /// <param name="service"></param>
    /// <param name="authorization"></param>
    /// <param name="contentEncoding"></param>
    /// <returns></returns>
    [HttpPost("{destinationServer}/{repositoryOwner}/{repository}/{service}", Name = "Post")]
    public async Task<ActionResult> PostUploadPack(
      string destinationServer,
      string repositoryOwner,
      string repository,
      string service,
      [FromHeader]string authorization,
      [FromHeader(Name = "content-encoding")]string contentEncoding)
    {
      ActionResult retval = new OkResult();
      var repo = GitContext.RemoteFactory.Build(destinationServer,
        repositoryOwner,
        repository,
        authorization);
      if (null != (retval = await CheckAuthorizationAsync(repo)))
        return retval;
      var local = GitContext.LocalFactory.Build(repo, GitContext.Configuration);
      retval = new ServiceResult(service,
                                 local,
                                 Shell,
                                 contentEncoding == "gzip");
      return retval;
    } // end of function - PostUploadPack

    /// <summary>
    /// HTTP POST handler for LFS batch API, receiving a JSON object with items the
    /// client wishes to know more about
    /// </summary>
    /// <param name="destinationServer">
    /// Destination server
    /// </param>
    /// <param name="repositoryOwner">
    /// Repository owner
    /// </param>
    /// <param name="repository">
    /// Name of the repository
    /// </param>
    /// <param name="value">
    /// JSON batch request object, describing LFS objects to get more information on
    /// </param>
    /// <param name="authorization">
    /// An authorization associated with the request
    /// </param>
    /// <returns>
    /// Task with a JSON result with more information pertaining to the request
    /// for the LFS objects
    /// </returns>
    /// <remarks>
    /// For more details, see https://github.com/git-lfs/git-lfs/blob/master/docs/api/batch.md
    /// </remarks>
    [HttpPost("{destinationServer}/{repositoryOwner}/{repository}/info/lfs/objects/batch")]
    public async Task<ActionResult> LFSBatchPost(
      string destinationServer,
      string repositoryOwner,
      string repository,
      [FromBody]BatchRequestObject value,
      [FromHeader]string authorization)
    {
      // Create a reference to the local cached repository
      var local = GetLocalRepo(destinationServer, repositoryOwner, repository, authorization);
      // And then return a JSON result for the LFS objects requested
      return new JsonResult(await CreateBatchLFSResponse(local, value));
    } // end of function - LFSBatchPost

    /// <summary>
    /// Creates a <see cref="BatchResponseObject"/> object with information for
    /// each LFS object in the batch request
    /// </summary>
    /// <param name="repo">
    /// The local cached repository
    /// </param>
    /// <param name="reqObj">
    /// The LFS batch request object, with information requested by the client
    /// </param>
    /// <returns>
    /// Task generating a <see cref="BatchResponseObject"/>, with information to
    /// send to the client
    /// </returns>
    public async Task<BatchResponseObject> CreateBatchLFSResponse(ILocalRepository repo, BatchRequestObject reqObj)
    {
      // Create our main response object
      BatchResponseObject retval = new BatchResponseObject();

      // Loop through all of the objects in the request object
      foreach(var obj in reqObj.Objects)
      {
        // We will need a list of actions to return
        LFSActions actions = new LFSActions();

        // We will use the file info object to get the file size later
        var fileInfo = new FileInfo(repo.LFSObjectPath(obj.OID));
#warning Find a way to get the controller's mapping entry point to build a base URL
        string baseUrl = $"{Request.Scheme}://{Request.Host}/{Request.PathBase}/";

        // Right now, we only support a "download" action, so build it out
        actions.Download = new LFSActions.DownloadAction()
          {
            ExpiresAt = null,
            ExpiresIn = 0,
            HREF = $"{baseUrl}/{repo.Remote.Server}/{repo.Remote.Owner}/{repo.Remote.Name}/lfs/download/{obj.OID}"
          };

        retval.Objects.Add(new BatchResponseObject.ResponseLFSItem()
          {
            Authenticated = true,
            OID = obj.OID,
            Size = (int)fileInfo.Length,
            Actions = actions,
            Error = null
          });
      } // end of foreach - object in the LFS request objects call
      return retval;
    } // end of function - CreateBatchLFSResponse

    /// <summary>
    /// HTTP GET handler for downloading a LFS object file
    /// </summary>
    /// <param name="destinationServer">
    /// Actual git server
    /// </param>
    /// <param name="repositoryOwner">
    /// Owner of the repository
    /// </param>
    /// <param name="repository">
    /// Name of the repository
    /// </param>
    /// <param name="oid">
    /// Object ID of the LFS object requesting
    /// </param>
    /// <param name="authorization">
    /// Authorization associated with the download object
    /// </param>
    /// <returns>
    /// Task generating a <see cref="FileStreamResult"/> object, for the 
    /// LFS object file
    /// </returns>
    [HttpGet("{destinationServer}/{repositoryOwner}/{repository}/lfs/download/{oid}")]
    public ActionResult LFSDownload(
      string destinationServer,
      string repositoryOwner,
      string repository,
      string oid,
      [FromHeader]string authorization)
    {
      // Get the local repository information
      var local = GetLocalRepo(destinationServer, repositoryOwner, repository, authorization);
      // Find the LFS object path in our repository to open a stream on and return
      var pathToDownload = local.LFSObjectPath(oid);
      var fs = new FileStream(pathToDownload, FileMode.Open);
      return File(fs, "application/octect-stream", Path.GetFileName(pathToDownload));
    } // end of function - LFSDownload

    /// <summary>
    /// DELETE entry point, to remove a cached value for a git repository
    /// </summary>
    /// <param name="destinationServer">
    /// Git server, e.g. github.com
    /// </param>
    /// <param name="repositoryOwner">
    /// Repository owner, e.g. brogdogg
    /// </param>
    /// <param name="repository">
    /// Name of the repository
    /// </param>
    /// <returns>
    /// A JSON result with message and error status code
    /// </returns>
    [HttpDelete("{destinationServer}/{repositoryOwner}/{repository}")]
    public JsonResult DeleteCachedRepository(string destinationServer, string repositoryOwner, string repository)
    {
      var statusCode = System.Net.HttpStatusCode.OK;
      string message = null;
      // Get reference to the remote
      var local = GetLocalRepo(destinationServer, repositoryOwner, repository);
      if (Directory.Exists(local.Path))
      {
        Directory.Delete(local.Path, true);
        // See if we were successful?
        if (!Directory.Exists(local.Path))
          message = "Deleted local cached directory";
        // Otherwise, indicate a different error message
        else
        {
          message = "Failed to delete local cached directory";
          statusCode = System.Net.HttpStatusCode.InternalServerError;
        } // end of else - error deleting
      } // end of if - repository exists on local disk
      // Otherwise, we don't have a local copy of the repository to delete
      else
      {
        message = "The repository was not found in the local cache";
        statusCode = System.Net.HttpStatusCode.NotFound;
      } // end of else - repository does not exists locally

      return new JsonResult(new
        {
          Message = message,
          StatusCode = statusCode
        });
    } // end of function - DeleteCachedRepository

    /************************ Fields *****************************************/
    /************************ Static *****************************************/

    /*======================= PROTECTED =====================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /// <summary>
    /// Gets the local repository associated with the given parameters
    /// </summary>
    /// <param name="server"></param>
    /// <param name="owner"></param>
    /// <param name="name"></param>
    /// <param name="auth"></param>
    /// <returns></returns>
    protected ILocalRepository GetLocalRepo(string server, string owner, string name, string auth = null)
    {
      return GitContext.LocalFactory.Build(
        GitContext.RemoteFactory.Build(server, owner, name, auth),
        GitContext.Configuration);
    } // end of function - GetLocalRepo

    /************************ Fields *****************************************/
    /************************ Static *****************************************/

    /*======================= PRIVATE =======================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/

    /// <summary>
    /// Authenticates asynchronously
    /// </summary>
    /// <param name="repo">
    /// The remote repository to authenticate against
    /// </param>
    /// <returns>
    /// Task for getting the HTTP Response from the authentication request
    /// </returns>
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

    /// <summary>
    /// Checks the authorization against the remote repository
    /// </summary>
    /// <param name="repo">
    /// Remote repository to work with
    /// </param>
    /// <returns>
    /// The forwarded result task
    /// </returns>
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
    /************************ Static *****************************************/

  } // end of class - GitController

} // end of namespace - git_cache.Controllers


/* End of document - GitController.cs */