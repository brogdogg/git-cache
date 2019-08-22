/******************************************************************************
 * File...: IRemoteRepository.cs
 * Remarks: 
 */
using System;

namespace git_cache.Services.Git
{
  /************************** RemoteRepository *******************************/
  /// <summary>
  /// Represents a remote repository, which provides the URL used for a
  /// configuration
  /// </summary>
  public class RemoteRepository : IRemoteRepository
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
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
    public IAuthInfo Auth { get; } = null;
    /************************ Construction ***********************************/
    /*----------------------- RemoteRepository ------------------------------*/
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
    public RemoteRepository(
      string server,
      string owner,
      string name,
      IAuthInfo auth,
      bool disableHTTPS = false)
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
      Url = $"{protocol}://{Server}/{Owner}/{Name}";
      GitUrl = auth == null ? Url : $"{protocol}://{auth.Decoded.ToString()}@{Server}/{Owner}/{Name}";
    } /* End of Function - RemoteRepository */
    /************************ Methods ****************************************/
    /************************ Fields *****************************************/
    /************************ Static *****************************************/
  } /* End of Class - RemoteRepository */


  /************************** RemoteRepositoryFactory ************************/
  /// <summary>
  /// 
  /// </summary>
  public class RemoteRepositoryFactory : IRemoteRepositoryFactory
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /*----------------------- Build -----------------------------------------*/
    /// <summary>
    /// 
    /// </summary>
    /// <param name="server"></param>
    /// <param name="owner"></param>
    /// <param name="name"></param>
    /// <param name="auth"></param>
    public IRemoteRepository Build(
      string server,
      string owner,
      string name,
      string auth)
    {
      return new RemoteRepository(server, owner, name, AuthInfo.ParseAuth(auth));
    } /* End of Function - Build */
    /************************ Fields *****************************************/
    /************************ Static *****************************************/
  } /* End of Class - RemoteRepositoryFactory */
}
/* End of document - RemoteRepository.cs */