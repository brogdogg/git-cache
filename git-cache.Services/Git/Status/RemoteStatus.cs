/******************************************************************************
 * File...: RemoteStatus.cs
 * Remarks: 
 */
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace git_cache.Services.Git.Status
{
  /************************** RemoteStatus ***********************************/
  /// <summary>
  /// Basic implementation of the <see cref="IRemoteStatus"/> interface using
  /// a <see cref="IGitExecuter"/> for getting the status.
  /// </summary>
  public class RemoteStatus : IRemoteStatus
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    protected IGitExecuter GitExecutor { get; } 
    /************************ Construction ***********************************/
    /*----------------------- RemoteStatus ----------------------------------*/
    /// <summary>
    /// Injection constructor
    /// </summary>
    /// <param name="gitExecuter">
    /// Reference to a valid <see cref="IGitExecuter"/> object, for performing
    /// a dry run fetch operation
    /// </param>
    public RemoteStatus(IGitExecuter gitExecuter)
    {
      if (null == (GitExecutor = gitExecuter))
        throw new ArgumentNullException(nameof(gitExecuter), "A valid Git executor must be specified");
    } /* End of Function - RemoteStatus */

    /************************ Methods ****************************************/
    /*----------------------- GetAsync --------------------------------------*/
    /// <summary>
    /// Asynchronously gets the status for all of the references associated
    /// with the repository.
    /// </summary>
    /// <param name="localRepo">
    /// Reference to a repository to get the status for all references
    /// </param>
    public Task<ICollection<RefStatus>> GetAsync(ILocalRepository localRepo)
    {
      return Task<ICollection<RefStatus>>.Factory.StartNew(() =>
      {
        var retval = new List<RefStatus>();
        lock (m_lock)
        {
          var result = GitExecutor.FetchAsync(localRepo, true).Result;
          if (null != result)
          {
            Regex pattern = new Regex(@"\s(?<flag>.)\s(?<summary>\b(\[up to date])|.*)\s(?<from>[^\s]+)\s+->\s+(?<to>[^\s]+)(?<reason> ?.*)");
            foreach (var line in result.Split('\n'))
            {
              Match m = pattern.Match(line);
              if (null != m && m.Success)
              {
                retval.Add(new RefStatus()
                {
                  Flag = RefStatus.GetMappedState(m.Groups["flag"].Value[0]),
                  From = m.Groups["from"].Value,
                  To = m.Groups["to"].Value,
                  Summary = m.Groups["summary"].Value,
                  Reason = m.Groups["reason"].Value
                });

              } // end of if - successful match
            } // end of foreach - line in the result
          } // end of if - valid result
        }
        return retval;
      });
    } /* End of Function - GetAsync */
    /************************ Fields *****************************************/
    /************************ Static *****************************************/

    /*======================= PROTECTED =====================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /************************ Fields *****************************************/
    /************************ Static *****************************************/

    /*======================= PRIVATE =======================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /************************ Fields *****************************************/
    private readonly object m_lock = new object();
    /************************ Static *****************************************/
  } /* End of Class - RemoteStatus */
}
/* End of document - RemoteStatus.cs */