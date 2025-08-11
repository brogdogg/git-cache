/******************************************************************************
 * File...: RefStatus.cs
 * Remarks: 
 */

using System.Collections.Generic;

namespace git_cache.Services.Git.Status
{
  /************************** RefStatus **************************************/
  /// <summary>
  /// Status model for a git reference
  /// </summary>
  public class RefStatus
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Types ******************************************/
    /************************ State ******************************************/
    /// <summary>
    /// Possible states for a git ref
    /// </summary>
    public enum State
    {
      /// <summary>
      /// Unknown state
      /// </summary>
      Unknown = 0,
      /// <summary>
      /// Ref is considered up-to-date
      /// </summary>
      UpToDate,
      /// <summary>
      /// Ref has failed/rejected to merge
      /// </summary>
      FailedOrRejected,
      /// <summary>
      /// Ref is new
      /// </summary>
      New,
      /// <summary>
      /// Local ref was pruned
      /// </summary>
      Pruned,
      /// <summary>
      /// Ref was forcefully updated
      /// </summary>
      ForcedUpdated,
      /// <summary>
      /// Ref is updated
      /// </summary>
      Updated,
      TagUpdated
    } /* End of Enum - State */

    /// <summary>
    /// Mapping table for the flag from the fetch verbose output
    /// </summary>
    public static Dictionary<char, State> MappingTable = new Dictionary<char, State>()
    {
      { '!', State.FailedOrRejected },
      { '+', State.ForcedUpdated },
      { '*', State.New },
      { '-', State.Pruned },
      { 't', State.TagUpdated },
      { '\0', State.Unknown },
      { ' ', State.Updated },
      { '=', State.UpToDate }
    };

    /************************ Properties *************************************/
    /// <summary>
    /// Gets the current state for ref
    /// </summary>
    public State Flag { get; set; } = State.Unknown;
    /// <summary>
    /// A summary of the ref
    /// </summary>
    public string Summary { get; set; } = null;
    /// <summary>
    /// From which commit
    /// </summary>
    public string From { get; set; } = null;
    /// <summary>
    /// To which commit
    /// </summary>
    public string To { get; set; } = null;
    /// <summary>
    /// Reason for status
    /// </summary>
    public string Reason { get; set; } = null;
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /************************ Fields *****************************************/
    /************************ Static *****************************************/
    /*----------------------- GetMappedState --------------------------------*/
    /// <summary>
    /// Gets the state associated with the specified flag character
    /// </summary>
    /// <param name="flag"></param>
    public static State GetMappedState(char flag)
    {
      if (MappingTable.ContainsKey(flag))
        return MappingTable[flag];
      else
        throw new System.ArgumentOutOfRangeException(
          nameof(flag), $"Unknown flag value: {flag}");
    } /* End of Function - GetMappedState */

  } /* End of Class - RefStatus */
}
/* End of document - RefStatus.cs */