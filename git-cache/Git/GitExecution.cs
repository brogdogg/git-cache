using git_cache.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace git_cache.Git
{
  public static class GitExecution
  {
    /// <summary>
    /// Clones the repository
    /// </summary>
    /// <param name="repo"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public static string Clone(this LocalRepo local)
    {
      local.CreateLocalDirectory();
      return $"git clone --quiet --mirror \"{local.Remote.Url}\" \"{local.Path}\"".Bash();
    }

    public static Task<string> CloneAsync(this LocalRepo local)
    {
      return Task.Run(() => Clone(local));
    }

    /// <summary>
    /// Fetches for the repository, assumes already exists...
    /// </summary>
    /// <param name="repo"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public static string Fetch(this LocalRepo local)
    {
      $"git -C \"{local.Path}\" remote set-url origin \"{local.Remote.Url}\"".Bash();
      return $"git -C \"{local.Path}\" fetch --quiet".Bash();
    }

    public static Task<string> FetchAsync(this LocalRepo local)
    {
      return Task.Run(() => Fetch(local));
    }

    public static Task<string> UpdateLocalAsync(this LocalRepo local)
    {
      return FetchAsync(local)
        .ContinueWith((output) => CloneAsync(local).Result, TaskContinuationOptions.OnlyOnRanToCompletion)
        .ContinueWith((a) => a.Result, TaskContinuationOptions.OnlyOnFaulted);
    }
  }

}
