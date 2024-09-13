/******************************************************************************
 * File...: IRemoteStatus.cs
 * Remarks: 
 */
using System.Collections.Generic;
using System.Threading.Tasks;

namespace git_cache.Services.Git.Status
{
  /************************** IRemoteStatus **********************************/
  /// <summary>
  /// Describes a service capable of determining the status of the references
  /// of a local repository against the remote.
  /// </summary>
  public interface IRemoteStatus
  {
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Methods ****************************************/
    /// <summary>
    /// Gets a collection of reference status' for the specified local
    /// repository.
    /// </summary>
    /// <param name="repo">
    /// Repository to get status for
    /// </param>
    /// <returns>
    /// A collection of status objects for references in the local repository
    /// </returns>
    Task<ICollection<RefStatus>> GetAsync(ILocalRepository repo);
  } /* End of Interface - IRemoteStatus */
}
/* End of document - IRemoteStatus.cs */