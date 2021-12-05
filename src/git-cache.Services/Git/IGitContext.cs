/******************************************************************************
 * File...: IRemoteRepository.cs
 * Remarks: 
 */
using git_cache.Services.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace git_cache.Services.Git
{
  /************************** IGitContext ************************************/
  /// <summary>
  /// 
  /// </summary>
  public interface IGitContext
  {
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    IGitCacheConfiguration Configuration { get; }
    /// <summary>
    /// 
    /// </summary>
    ILocalRepositoryFactory LocalFactory { get; }
    /// <summary>
    /// 
    /// </summary>
    IRemoteRepositoryFactory RemoteFactory { get; }
    /// <summary>
    /// 
    /// </summary>
    IGitExecuter GitExecuter { get; }
    /// <summary>
    /// 
    /// </summary>
    IGitLFSExecuter LFSExecuter { get; }
    /************************ Methods ****************************************/
    Task<string> UpdateLocalAsync(ILocalRepository local);
  } /* End of Interface - IGitContext */
}
/* End of document - IGitContext.cs */