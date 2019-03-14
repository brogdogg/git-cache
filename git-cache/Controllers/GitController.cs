using System;
using Microsoft.AspNetCore.Mvc;
using git_cache.Git;
using Microsoft.Extensions.Configuration;
using git_cache.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.Diagnostics;
using git_cache.Results;
using System.Text;
using Newtonsoft.Json;
using git_cache.Git.LFS;
using System.IO;
using git_cache.Shell;

namespace git_cache.Controllers
{
  [Produces("application/json")]
  [Route("api/Git")]
  public class GitController : Controller
  {
    // Holding spot for the injected configuration
    IConfiguration Configuration { get; } = null;

    /// <summary>
    /// Constructor for dependency injection
    /// </summary>
    /// <param name="config">
    /// application configuration object
    /// </param>
    public GitController(IConfiguration config)
      : base()
    {
      if (null == (Configuration = config))
        throw new ArgumentNullException("A valid configuration must be provided");
    } // end of function - GitController

    /// <summary>
    /// 
    /// </summary>
    /// <param name="auth"></param>
    /// <returns></returns>
    private AuthInfo ParseAuth(string auth)
    {
      var pair = auth.Split(" ");
      return new AuthInfo
      {
        Scheme = pair[0],
        RawAuth = auth,
        Decoded = Encoding.UTF8.GetString(Convert.FromBase64String(pair[1])),
        Encoded = pair[1]

      };
    } // end of function - ParseAuth

    /// <summary>
    /// 
    /// </summary>
    /// <param name="repo"></param>
    /// <returns></returns>
    private async Task<HttpResponseMessage> AuthenticateAsync(RemoteRepo repo)
    {
      HttpClient client = new HttpClient();
      client.DefaultRequestHeaders.Accept.Clear();
      if (null != repo.Auth)
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(repo.Auth.Scheme, repo.Auth.Encoded);
      client.DefaultRequestHeaders.UserAgent.Add(
        new System.Net.Http.Headers.ProductInfoHeaderValue("git", "1.0"));
      string url = $"{repo.Url}/info/refs?service=git-upload-pack";
      var resp = await client.GetAsync(url);
      return resp;
    } // end of function - AuthenticateAsync

    /// <summary>
    /// 
    /// </summary>
    /// <param name="destinationServer"></param>
    /// <param name="repoOwner"></param>
    /// <param name="repo"></param>
    /// <param name="authorization"></param>
    /// <returns></returns>
    private RemoteRepo BuildRemoteRepo(string destinationServer, string repoOwner, string repo, string authorization)
    {
      AuthInfo authInfo = null;
      if (null != authorization)
        authInfo = ParseAuth(authorization);
      return new RemoteRepo(destinationServer,
                            repoOwner,
                            repo,
                            authInfo,
                            Configuration.DisableHTTPS());
    } // end of function - BuildRemoteRepo

    /// <summary>
    /// 
    /// </summary>
    /// <param name="repo"></param>
    /// <returns></returns>
    private async Task<ActionResult> CheckAuthorizationAsync(RemoteRepo repo)
    {
      ActionResult retval = null;
      var resp = await AuthenticateAsync(repo);
      if (System.Net.HttpStatusCode.OK != resp.StatusCode)
        retval = new ForwardedResult(resp);
      return retval;
    } // end of function - CheckAuthorizationAsync

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
    public async Task<ActionResult> GetAsync(string destinationServer, string repositoryOwner, string repository, [FromQuery]string service, [FromHeader]string authorization)
    {
      ActionResult retval = null;
      var repo = BuildRemoteRepo(destinationServer, repositoryOwner, repository, authorization);
      if (null != (retval = await CheckAuthorizationAsync(repo)))
        return retval;
      // 3. Build local repository information using the remote respository info
      LocalRepo local = new LocalRepo(repo, new LocalConfiguration(Configuration));
      // 6. Is this an info request??
      //   6a. YES
      //     *. Update local contents
      try
      {
        await GitExecution.UpdateLocalAsync(local);
        retval = new GitServiceAdvertisementResult(service, local);
      }
      catch (Exception exc)
      {
        Debug.WriteLine("Exception: " + exc);
        retval = new StatusCodeResult(500);
      }
      return retval;
    } // end of function - GetAsync

    /// <summary>
    /// 
    /// </summary>
    /// <param name="destinationServer"></param>
    /// <param name="repositoryOwner"></param>
    /// <param name="repository"></param>
    /// <param name="service"></param>
    /// <param name="authorization"></param>
    /// <param name="contentEncoding"></param>
    /// <returns></returns>
    [HttpPost("{destinationServer}/{repositoryOwner}/{repository}/{service}", Name = "Post")]
    public async Task<ActionResult> PostUploadPack(string destinationServer, string repositoryOwner, string repository, string service, [FromHeader]string authorization, [FromHeader(Name = "content-encoding")]string contentEncoding)
    {
      ActionResult retval = new OkResult();
      var repo = BuildRemoteRepo(destinationServer, repositoryOwner, repository, authorization);
      if (null != (retval = await CheckAuthorizationAsync(repo)))
        return retval;
      LocalRepo local = new LocalRepo(repo, new LocalConfiguration(Configuration));
      try
      {
        retval = new GitServiceResultResult(service, local, contentEncoding == "gzip");
      }
      catch (Exception exc)
      {
        Debug.WriteLine("Exception: " + exc);
        retval = new StatusCodeResult(500);
      }
      return retval;
    } // end of function - PostUploadPack

    // POST: objects/batch - https://github.com/git-lfs/git-lfs/blob/master/docs/api/batch.md
    [HttpPost("{destinationServer}/{repositoryOwner}/{repository}/info/lfs/objects/batch")]
    public async Task<ActionResult> LFSBatchPost(string destinationServer, string repositoryOwner, string repository, [FromBody]BatchRequestObject value, [FromHeader]string authorization)
    {
      ActionResult retval = new OkResult();

      HttpClient client = new HttpClient();
      var repo = BuildRemoteRepo(destinationServer, repositoryOwner, repository, authorization);
      client.DefaultRequestHeaders.Accept.Clear();
      if (null != repo.Auth)
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(repo.Auth.Scheme, repo.Auth.Encoded);
      client.DefaultRequestHeaders.UserAgent.Add(
        new System.Net.Http.Headers.ProductInfoHeaderValue("git", "1.0"));
      client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/vnd.git-lfs+json"));
      string url = $"{repo.Url}/info/lfs/objects/batch";
      //var resp = await client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(value), Encoding.UTF8, "application/vnd.git-lfs+json"));
      //return new ForwardedResult(resp);
      retval = new JsonResult(await CreateBatchLFSResponse(repo, value));
      return retval;
    } // end of function - LFSBatchPost

    public async Task<BatchResponseObject> CreateBatchLFSResponse(RemoteRepo repo, BatchRequestObject reqObj)
    {
      BatchResponseObject retval = new BatchResponseObject();
      foreach(var obj in reqObj.Objects)
      {
        LFSActions actions = new LFSActions();
        //string baseUrl = $"{this.Request.Scheme}://{this.Request.Host}/{this.Request.PathBase}";
        string baseUrl = $"{this.Request.Scheme}://{this.Request.Host}/api/Git";
        actions.Download = new LFSActions.DownloadAction()
        {
          ExpiresAt = null,
          ExpiresIn = 0,
          HREF = $"{baseUrl}/{repo.Server}/{repo.Owner}/{repo.Name}/lfs/download/{obj.OID}"
        };
        retval.Objects = new System.Collections.Generic.List<BatchResponseObject.ResponseLFSItem>();
        retval.Objects.Add(new BatchResponseObject.ResponseLFSItem()
        {
          Authenticated = true,
          OID = obj.OID,
          Size = 0,
          Actions = actions,
          Error = null
        });
      } // end of foreach - object in the LFS request objects call
      return retval;
    } // end of function - CreateBatchLFSResponse

    [HttpGet("{destinationServer}/{repositoryOwner}/{repository}/lfs/download/{oid}")]
    public async Task<ActionResult> LFSDownload(string destinationServer, string repositoryOwner, string repository, string oid, [FromHeader]string authorization)
    {
      ActionResult retval = new OkResult();
      var repo = BuildRemoteRepo(destinationServer, repositoryOwner, repository, authorization);
      var local = new LocalRepo(repo, new LocalConfiguration(Configuration));
      var objFilePath = ($"git-lfs ls-files -l | grep {oid} | " + "awk -F'*' '{print $2}'").Bash();
      var pathToDownload = System.IO.Path.Combine(local.Path, objFilePath);
      var fs = new FileStream(pathToDownload, FileMode.Open);
      retval = File(fs, "application/octect-stream", Path.GetFileName(pathToDownload));
      return retval;
    }

    // PUT: api/Git/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody]string value)
    {
    }

    // DELETE: api/ApiWithActions/5
    [HttpDelete("{destinationServer}/{repositoryOwner}/{repository}")]
    public async Task<JsonResult> Delete(string destinationServer, string repositoryOwner, string repository)
    {
      var statusCode = System.Net.HttpStatusCode.OK;
      string message = null;
      var repo = BuildRemoteRepo(destinationServer, repositoryOwner, repository, null);
      var local = new LocalRepo(repo, new LocalConfiguration(Configuration));
      if (System.IO.Directory.Exists(local.Path))
      {
        System.IO.Directory.Delete(local.Path, true);
        if (!System.IO.Directory.Exists(local.Path))
          message = "Deleted local cached directory";
        else
        {
          message = "Failed to delete local cached directory";
          statusCode = System.Net.HttpStatusCode.InternalServerError;
        }
      }
      else
      {
        message = "The repository was not found in the local cache";
        statusCode = System.Net.HttpStatusCode.NotFound;
      }
      return new JsonResult(new
      {
        Message = message,
        StatusCode = statusCode
      });
    }
  } // end of class - GitController

} // end of namespace - git_cache.Controllers


