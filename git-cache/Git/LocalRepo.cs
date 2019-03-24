/******************************************************************************
 * File...: LocalRepo
 * Remarks: 
 */
using git_cache.Configuration;
using Microsoft.Extensions.Configuration;
using System;

namespace git_cache.Git
{
  /************************** AuthInfo ***************************************/
  /// <summary>
  /// Authentication information
  /// </summary>
  public class AuthInfo
  {
    /*===================== PUBLIC ========================================*/
    /********************** Events *****************************************/
    /********************** Properties *************************************/
    /********************** Scheme *****************************************/
    /// <summary>
    /// Gets/Sets the scheme for the authentication information
    /// </summary>
    public string Scheme { get; set; } /* End of Property - Scheme */
    /********************** RawAuth ****************************************/
    /// <summary>
    /// Gets/Sets the raw authentication information
    /// </summary>
    public string RawAuth { get; set; } /* End of Property - RawAuth */
    /********************** Decoded ****************************************/
    /// <summary>
    /// Gets/Sets the decoded value
    /// </summary>
    public string Decoded { get; set; } /* End of Property - Decoded */
    /********************** Encoded ****************************************/
    /// <summary>
    /// Gets/Sets the encoded value
    /// </summary>
    public string Encoded { get; set; } /* End of Property - Encoded */
    /********************** Construction ***********************************/
    /********************** Methods ****************************************/
    /********************** Fields *****************************************/
    /********************** Static *****************************************/
  } /* End of Class - AuthInfo */

  /************************** RemoteRepo *************************************/
  /// <summary>
  /// 
  /// </summary>
  public class RemoteRepo
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Server *****************************************/
    /// <summary>
    /// Gets/Sets the server for the repository
    /// </summary>
    public string Server { get; set; } = null;
    /************************ Owner ******************************************/
    /// <summary>
    /// Gets/Sets the owner of the repostiory
    /// </summary>
    public string Owner { get; set; } = null;
    /************************ Name *******************************************/
    /// <summary>
    /// Gets/Sets the name of the repository
    /// </summary>
    public string Name { get; set; } = null;
    /************************ Url ********************************************/
    /// <summary>
    /// Gets the URL for the remote repository
    /// </summary>
    public string Url { get; private set; } = null;
    /************************ GitUrl *****************************************/
    /// <summary>
    /// Gets the git URL
    /// </summary>
    public string GitUrl { get; } /* End of Property - GitUrl */
    /************************ Auth *******************************************/
    /// <summary>
    /// Gets the authentication information associated with this repository
    /// </summary>
    public AuthInfo Auth { get; } = null;
    /************************ Construction ***********************************/
    /*----------------------- RemoteRepo ------------------------------------*/
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="server">
    /// The real server
    /// </param>
    /// <param name="owner">
    /// Owner of the repository
    /// </param>
    /// <param name="name">
    /// Name of the repository
    /// </param>
    /// <param name="auth">
    /// Authentication associated with the remote repository
    /// </param>
    /// <param name="disableHTTPS">
    /// Should secure be disabled
    /// </param>
    public RemoteRepo(string server, string owner, string name, AuthInfo auth, bool disableHTTPS = false)
    {
      if (null == (Server = server))
        throw new ArgumentNullException("Must provide a property server name");
      if (null == (Owner = owner))
        throw new ArgumentNullException("Must provide a valid owner name");
      if (null == (Name = name))
        throw new ArgumentNullException("Must provide a valid name for the repository");
      else if (Name.EndsWith(".git"))
        Name = Name.Remove(Name.LastIndexOf(".git"));
      Auth = auth;

      string protocol = disableHTTPS ? "http" : "https";
      Url = $"{protocol}://{server}/{owner}/{name}";
      GitUrl = auth == null ? Url : $"{protocol}://{auth.Decoded.ToString()}@{server}/{owner}/{name}";
    } /* End of Function - RemoteRepo */
    /************************ Methods ****************************************/
    /************************ Fields *****************************************/
    /************************ Static *****************************************/
  } /* End of Class - RemoteRepo */

  /************************** LocalConfiguration *****************************/
  /// <summary>
  /// 
  /// </summary>
  public class LocalConfiguration
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    public string Path { get; private set; } = null;
    /************************ Construction ***********************************/
    /*----------------------- LocalConfiguration ----------------------------*/
    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="configuration">
    /// Configuration
    /// </param>
    public LocalConfiguration(IConfiguration configuration)
      : this(configuration.GetLocalStoragePath())
    {
      return;
    } /* End of Function - LocalConfiguration */

    /*----------------------- LocalConfiguration ----------------------------*/
    /// <summary>
    /// Constructor taking a raw path to the local storage
    /// </summary>
    /// <param name="path">
    /// Path to the local storage
    /// </param>
    public LocalConfiguration(string path)
    {
      Path = path;
      return;
    } /* End of Function - LocalConfiguration */
    /************************ Methods ****************************************/
    /************************ Fields *****************************************/
    /************************ Static *****************************************/
  } /* End of Class - LocalConfiguration */

  /************************** LocalRepo **************************************/
  /// <summary>
  /// 
  /// </summary>
  public class LocalRepo
  {

    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    public RemoteRepo Remote { get; private set; } = null;
    public LocalConfiguration Config { get; private set; } = null;
    public string Path { get; private set; } = null;
    /************************ Construction ***********************************/
    /*----------------------- LocalRepo -------------------------------------*/
    /// <summary>
    /// Constructor, taking a remote repository and local configuration object
    /// </summary>
    /// <param name="remoteRepo">
    /// A remote repository to setup a local one for
    /// </param>
    /// <param name="config">
    /// A configuration object to use for the local repository
    /// </param>
    public LocalRepo(RemoteRepo remoteRepo, LocalConfiguration config)
    {
      if (null == (Remote = remoteRepo))
        throw new ArgumentNullException(
          "Must provide a remote repository object");
      if (null == (Config = config))
        throw new ArgumentNullException("Must provide a configuration item");
      Path = System.IO.Path.Combine(Config.Path,
                                    Remote.Server,
                                    Remote.Owner,
                                    Remote.Name);
    } /* End of Function - LocalRepo */
    /************************ Methods ****************************************/
    /*----------------------- CreateLocalDirectory --------------------------*/
    /// <summary>
    /// Creates the local directory for the path
    /// </summary>
    public void CreateLocalDirectory()
    {
      if (!System.IO.Directory.Exists(Path))
      {
        var dir = System.IO.Directory.CreateDirectory(Path);
        if (!dir.Exists)
          dir.Create();
      } // end of if - path does not exists
    } /* End of Function - CreateLocalDirectory */
    /************************ Fields *****************************************/
    /************************ Static *****************************************/
  } /* End of Class - LocalRepo */
}
/* End of document - LocalRepo.cs */
