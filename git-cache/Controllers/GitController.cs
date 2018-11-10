using System;
using Microsoft.AspNetCore.Mvc;
using git_cache.Git;
using Microsoft.Extensions.Configuration;
using git_cache.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.Diagnostics;
using git_cache.Results;

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

    private object ParseAuth(string auth) {
      var pair = auth.Split(" ");
      return new {
        Scheme = pair[0],
        RawAuth = auth,
        Decoded = Convert.FromBase64String(pair[1]),
        Encoded = pair[1]

      };
    }
    private async Task<int> AuthenticateAsync(RemoteRepo repo, dynamic auth){
      HttpClient client = new HttpClient();
      client.DefaultRequestHeaders.Accept.Clear();
      client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(auth.Scheme, auth.Decoded);
      string url = $"{repo.Url}/info/refs?service=git-upload-pack";
      var resp = await client.GetAsync(url);
      return (int)resp.StatusCode;
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
    public async Task<ActionResult> GetAsync(string destinationServer, string repositoryOwner, string repository, [FromQuery]string service, [FromHeader]string authorization)
    {
      ActionResult retval = null;
      // 1. Parse Authorization from headers, if exists
      // 2. Build remote repository information using auth
      RemoteRepo repo = new RemoteRepo(destinationServer, repositoryOwner, repository, Configuration.DisableHTTPS());
      // 3. Build local repository information using the remote respository info
      LocalRepo local = new LocalRepo(repo, new LocalConfiguration(Configuration));
      if(null != authorization ) {
        var authInfo = ParseAuth(authorization);
        // 4. Authenticate with remote repository
        switch(await AuthenticateAsync(repo, authInfo)){
          case 401:
          case 403:
          case 404:
            throw new HttpRequestException("Failed to authenticate");
          case 200: // OK
          default:
            break;
        }
        // 5. If response code 200 all is fine... 4xx is failed authenticate
      }
      // 6. Is this an info request??
      //   6a. YES
      //     *. Update local contents
      try
      {
        await GitExecution.UpdateLocalAsync(local);
        retval = new GitServiceAdvertisementResult(service, local);
      }
      catch(Exception exc)
      {
        Debug.WriteLine("Exception: " + exc);
        retval = new StatusCodeResult(500);
      }
      //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("basic",)
      //     *. Set response headers
      //        Content-Type: application/x-${service}-advertisement
      //        Cache-Control: no-cache
      //     *. Write to the response:
      //        001e# service=${service}\n0000
      //     *. Write stdout of service output
      //   6b. NO
      //     *. Set response headers
      //        Content-Type: application/x-${service}-result
      //        Cache-Control: no-cache
      //     *. Write stdout of service output
      //     *. Do we have gzip?
      //        *. YES - write gzip contents to req
      //        *. NO  - write contents to req
      return retval;
    }

    [HttpPost("{destinationServer}/{repositoryOwner}/{repository}/{service}", Name ="Post")]
    public async Task<ActionResult> PostUploadPack(string destinationServer, string repositoryOwner, string repository, string service, [FromHeader]string authorization, [FromHeader(Name ="content-encoding")]string contentEncoding)
    {
      ActionResult retval = new OkResult();
      RemoteRepo repo = new RemoteRepo(destinationServer, repositoryOwner, repository);
      LocalRepo local = new LocalRepo(repo, new LocalConfiguration(Configuration));
      if(null != authorization)
      {
        dynamic auth = ParseAuth(authorization);
        switch(await AuthenticateAsync(repo, auth))
        {
          case 400:
          case 401:
          case 403:
          case 404:
            throw new HttpRequestException("Failed to authenticate with remote server");
          case 200:
          default:
            break;
        }
      }
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
    }

    // POST: objects/batch - https://github.com/git-lfs/git-lfs/blob/master/docs/api/batch.md
    [HttpPost("{destinationServer}/{repositoryOwner}/{repository}/info/lfs/objects/batch")]
    public void LFSBatchPost(string destinationServer, string repositoryOwner, string repository, [FromBody]string value)
    {
    } // end of function - LFSBatchPost

    // PUT: api/Git/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody]string value)
    {
    }

    // DELETE: api/ApiWithActions/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }

  } // end of class - GitController

} // end of namespace - git_cache.Controllers


