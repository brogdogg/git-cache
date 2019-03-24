using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace git_cache.Git.LFS
{
  public static class LFSLocalRepoExtension
  {
    public static string LFSObjectPath(this LocalRepo repo, string oid)
    {
      if (oid == null)
        throw new ArgumentNullException("OID must not be null");
      if (oid.Length != 64)
        throw new ArgumentException("OID does not appear to be correct length");
      var objFilePath = $"lfs/objects/{oid.Substring(0, 2)}/{oid.Substring(2, 2)}/{oid}";
      var fullLfsObjectPath = System.IO.Path.Combine(repo.Path, objFilePath);
      return fullLfsObjectPath;
    }
  }
}
