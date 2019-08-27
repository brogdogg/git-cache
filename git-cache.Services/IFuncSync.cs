/******************************************************************************
 * File...: ThreadSafeShell.cs
 * Remarks: 
 */
using System;

namespace git_cache.Services
{
  /************************** IFuncSync **************************************/
  /// <summary>
  /// Represents the interface for an object which executes a functor in a
  /// synchronous matter
  /// </summary>
  public interface IFuncSync<TResult, TInput>
  {
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Methods ****************************************/
    /// <summary>
    /// Executes, using the command as a key of sorts to make sure the command
    /// is not executed more than once at the same time.
    /// </summary>
    /// <param name="key">Key for syncing on</param>
    /// <param name="action">Functor to execute</param>
    /// <param name="input">Input argument for functor</param>
    /// <returns></returns>
    TResult Execute(string key, Func<TInput, TResult> action, TInput input);
  } /* End of Interface - IFuncSync */
}/* End of document - IFuncSync.cs */
