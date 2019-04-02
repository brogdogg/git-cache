/******************************************************************************
 * File...: GitContextUnitTest.cs
 * Remarks: 
 */
using System;
using System.IO;
using System.Threading.Tasks;

namespace git_cache.Shell
{
  /************************** IShell *****************************************/
  /// <summary>
  /// Describes an object that can be used to execute shell commands
  /// </summary>
  public interface IShell
  {
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Methods ****************************************/
    /// <summary>
    /// Executes the command
    /// </summary>
    /// <param name="command">Command to execute</param>
    /// <returns></returns>
    string Execute(string command);
    /// <summary>
    /// Executes the command, checking exit code
    /// </summary>
    /// <param name="command">Command to execute</param>
    /// <param name="isExitCodeFailure">
    /// Functor to decide if exit code is failure
    /// </param>
    /// <returns></returns>
    string Execute(string command, Func<int, bool> isExitCodeFailure);
    /// <summary>
    /// Executes the specified command, writing output to an
    /// output stream to service long standard output writes
    /// </summary>
    /// <param name="command">Command to execute</param>
    /// <param name="isExitCodeFailure">
    /// Functor to decide if exit code is failure
    /// </param>
    /// <param name="outStream">
    /// Output stream to write output to
    /// </param>
    /// <param name="inputWriter">
    /// Optional standard input writer
    /// </param>
    /// <returns>
    /// Always returns 0,
    /// </returns>
    int Execute(string command, Func<int, bool> isExitCodeFailure, Stream outStream, Action<StreamWriter> inputWriter = null);
    /// <summary>
    /// Executes asynchronously the specified command
    /// </summary>
    /// <param name="command">
    /// Command to execute
    /// </param>
    /// <returns>Output from the command</returns>
    Task<string> ExecuteAsync(string command);
    /// <summary>
    /// Executes asynchronously the specified command, using the optional functor
    /// to determine failure
    /// </summary>
    /// <param name="command">
    /// Command to execute
    /// </param>
    /// <param name="isExitCodeFailure">
    /// Should be an optional functor to decide if return code is a failure
    /// </param>
    /// <returns>
    /// Output from the command
    /// </returns>
    Task<string> ExecuteAsync(string command, Func<int, bool> isExitCodeFailure);
  } /* End of Interface - IShell */
}
/* End of document - IShell.cs */