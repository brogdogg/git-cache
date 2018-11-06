using git_cache.Configuration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace git_cache.Git
{
  public class RemoteRepo
  {
    public string Server { get; set; } = null;
    public string Owner { get; set; } = null;
    public string Name { get; set; } = null;
    public string Url { get; private set; } = null;
    public RemoteRepo(string server, string owner, string name, bool disableHTTPS = false)
    {
      if(null == (Server = server))
        throw new ArgumentNullException("Must provide a property server name");
      if (null == (Owner = owner))
        throw new ArgumentNullException("Must provide a valid owner name");
      if (null == (Name = name))
        throw new ArgumentNullException("Must provide a valid name for the repository");

      string protocol = disableHTTPS ? "http" : "https";
      Url = $"{protocol}://{server}/{owner}/{name}";
    }
  }

  public class LocalConfiguration
  {
    public string Path { get; private set; } = null;
    public LocalConfiguration(IConfiguration configuration)
      : this(configuration.GetLocalStoragePath())
    {
      return;
    }
    public LocalConfiguration(string path)
    {
      Path = path;
      return;
    }
  }
  public class LocalRepo
  {
    public RemoteRepo Remote { get; private set; } = null;
    public LocalConfiguration Config { get; private set; } = null;
    public string Path { get; private set; } = null;
    public LocalRepo(RemoteRepo remoteRepo, LocalConfiguration config)
    {
      if(null == (Remote = remoteRepo))
      {
        throw new ArgumentNullException("Must provide a remote repository object");
      }
      if(null == (Config = config))
      {
        throw new ArgumentNullException("Must provide a configuration item");
      }
      Path = System.IO.Path.Combine(Config.Path, Remote.Server, Remote.Owner, Remote.Name);
    }

    public void CreateLocalDirectory()
    {
      if(!System.IO.Directory.Exists(Path))
      {
        var dir = System.IO.Directory.CreateDirectory(Path);
        if(!dir.Exists)
        {
          dir.Create();
        }
      }
    }

  }
}
