using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace git_cache.Filters
{
  public class GitExceptionFilter : ExceptionFilterAttribute
  {
    public override void OnException(ExceptionContext context)
    {
      base.OnException(context);
    }
  }
}
