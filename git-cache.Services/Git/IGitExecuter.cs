/******************************************************************************
 * File...: IRemoteRepository.cs
 * Remarks: 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace git_cache.Services.Git
{
  /************************** IGitExecuter ***********************************/
  /// <summary>
  /// Describes an object that can be used to execute git commands on
  /// repositories
  /// </summary>
  public interface IGitExecuter
  {
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Methods ****************************************/
    string Clone(ILocalRepository local);
    Task<string> CloneAsync(ILocalRepository local);
    string Fetch(ILocalRepository local, bool doDryRun=false);
    Task<string> FetchAsync(ILocalRepository local, bool doDryRun=false);
  } /* End of Interface - IGitExecuter */

  /************************** IGitLFSExecuter ********************************/
  /// <summary>
  /// 
  /// </summary>
  public interface IGitLFSExecuter
  {
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Methods ****************************************/
    string Fetch(ILocalRepository local);
    Task<string> FetchAsync(ILocalRepository local);
  } /* End of Interface - IGitLFSExecuter */
}
/* End of document - IGitExecuter.cs */