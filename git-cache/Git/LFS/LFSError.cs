using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace git_cache.Git.LFS
{
  [DataContract(Name = "error")]
  public class LFSError
  {
    [DataMember(Name = "code")]
    public int Code { get; set; } = 0;
    [DataMember(Name = "message")]
    public string Message { get; set; } = "";
  }
}
