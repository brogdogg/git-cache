/******************************************************************************
 * File...: ILocalConfiguration.cs
 * Remarks: 
 */
namespace git_cache.Git
{
  /************************** ILocalConfiguration ****************************/
  /// <summary>
  /// Local configuration object
  /// </summary>
  public interface ILocalConfiguration
  {
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Path *******************************************/
    /// <summary>
    /// Gets the local path to use for storage purposes
    /// </summary>
    string Path { get; }
    /************************ Methods ****************************************/
  } /* End of Interface - ILocalConfiguration */
}/* End of document - ILocalConfiguration.cs */