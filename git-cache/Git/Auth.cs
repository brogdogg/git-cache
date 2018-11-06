using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;

namespace git_cache.Git
{
  public class Auth
  {
    public string User { get; } = null;
    public SecureString Password { get; } = null;

    public Auth(string user, string password)
    {
      User = user;
      Password = new SecureString();
      password.Select(c => { Password.AppendChar(c); return c; });
    }
  }
}
