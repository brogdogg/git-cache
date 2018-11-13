using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace git_cache.Controllers
{
  [Produces("application/json")]
  [Route("/")]
  public class GitCredController : Controller
  {
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [HttpHead("")]
    public ActionResult GitCredHEAD()
    {
      return new StatusCodeResult(500);
    }
  }
}