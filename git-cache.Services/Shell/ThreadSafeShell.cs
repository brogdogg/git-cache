/******************************************************************************
 * File...: ThreadSafeShell.cs
 * Remarks: 
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using git_cache.Services.Shell;
using Microsoft.Extensions.Logging;

namespace git_cache.Services
{

  /************************** ThreadSafeShell ********************************/
  /// <summary>
  /// 
  /// </summary>
  public class ThreadSafeShell : IShell
  {

    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    public IShell OSShell { get; set; } = null;
    /************************ Construction ***********************************/
    /*----------------------- SerialShellExecutionService -------------------*/
    /// <summary>
    /// 
    /// </summary>
    /// <param name="logger"></param>
    public ThreadSafeShell(ILogger<ThreadSafeShell> logger, IFuncSync<string, string> funcSync)
    {
      _logger = logger;
      _funcSyncer = funcSync;
    } /* End of Function - SerialShellExecutionService */

    public string Execute(string command)
    {
      AssertOSShell();
      return _funcSyncer.Execute(command, (cmd) => OSShell.Execute(cmd), command);
    }

    public string Execute(string command, Func<int, bool> isExitCodeFailure)
    {
      AssertOSShell();
      return _funcSyncer.Execute(command, (cmd) => OSShell.Execute(cmd, isExitCodeFailure), command);
    }

    public int Execute(string command, Func<int, bool> isExitCodeFailure, Stream outStream, Action<StreamWriter> inputWriter = null)
    {
      AssertOSShell();
      return int.Parse(
        _funcSyncer.Execute(
          command,
          (cmd) => OSShell.Execute(command,
                                   isExitCodeFailure,
                                   outStream,
                                   inputWriter).ToString(),
          command
          ));
    }

    public Task<string> ExecuteAsync(string command)
    {
      return Task<string>.Factory.StartNew(() =>
      {
        AssertOSShell();
        return Execute(command);
      });
    }

    public Task<string> ExecuteAsync(string command, Func<int, bool> isExitCodeFailure)
    {
      return Task<string>.Factory.StartNew(() =>
      {
        AssertOSShell();
        return Execute(command, isExitCodeFailure);
      });
    }
    /************************ Methods ****************************************/
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
    /************************ Types ******************************************/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /*----------------------- AssertOSShell ---------------------------------*/
    /// <summary>
    /// 
    /// </summary>
    private void AssertOSShell()
    {
      if (null == OSShell)
        throw new InvalidOperationException("Missing required base OS shell service");
    } /* End of Function - AssertOSShell */

    /************************ Fields *****************************************/
    private readonly ILogger<ThreadSafeShell> _logger;
    private IFuncSync<string, string> _funcSyncer;
    /************************ Static *****************************************/

  } /* End of Class - SerialShellExecutionService */
}
/* End of document - SerialShellExecutionService.cs */