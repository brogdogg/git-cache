using git_cache.Git;
using git_cache.Shell;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace git_cache.Results
{
  public class GitServiceAdvertisementResult : ActionResult
  {
    public string Service { get; }
    public LocalRepo Repository { get; }
    private Func<Stream, Task<int>> GetBodyContentsAsync { get; }

    public GitServiceAdvertisementResult(string service, LocalRepo repo)
    {
      Service = service;
      if (null == (Repository = repo))
      {
        throw new ArgumentNullException("Invalid repository given, must not be null");
      }
      return;
    }

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
    }
  }
}
