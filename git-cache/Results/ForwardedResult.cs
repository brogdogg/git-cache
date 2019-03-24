using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace git_cache.Results
{
  public class ForwardedResult : ActionResult
  {
    private HttpResponseMessage BaseResponse { get; }
    public ForwardedResult(HttpResponseMessage baseResult)
    {
      BaseResponse = baseResult;
    }
    public override void ExecuteResult(ActionContext context)
    {
      var response = context.HttpContext.Response;
      response.StatusCode = (int)BaseResponse.StatusCode;
      BaseResponse.Headers.All((header) =>
      {
        response.Headers.Add(header.Key, new Microsoft.Extensions.Primitives.StringValues(header.Value.ToArray()));
        return true;
      });
      return;
    }
  }
}
