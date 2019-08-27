/******************************************************************************
 * File...: ThreadSafeShell.cs
 * Remarks: 
 */
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;

namespace git_cache.Services
{
  /************************** FuncSync ***************************************/
  /// <summary>
  /// Concrete implementation of the <see cref="IFuncSync{TResult, TInput}"/>
  /// interface.
  /// </summary>
  /// <typeparam name="TInput">Outpu for the functor</typeparam>
  /// <typeparam name="TResult">Output type for the functor</typeparam>
  public class FuncSync<TResult, TInput> : IFuncSync<TResult, TInput>
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /*----------------------- FuncSync --------------------------------------*/
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="logger">
    /// Logger object to use for logging purposes.
    /// </param>
    public FuncSync(ILogger<FuncSync<TResult, TInput>> logger)
    {
      m_logger = logger;
    } /* End of Function - FuncSync */
    /************************ Methods ****************************************/
    /*----------------------- Execute ---------------------------------------*/
    /// <summary>
    /// Executes the functor in sync for the specified key.
    /// </summary>
    /// <param name="key">
    /// Key to use to control execution sync point
    /// </param>
    /// <param name="action">
    /// Functor for execution
    /// </param>
    /// <param name="input">
    /// Input argument for the functor
    /// </param>
    public TResult Execute(
      string key,
      Func<TInput, TResult> action,
      TInput input)
    {
      // command state holder
      FuncState<TResult> commandState = null;
      bool isNew = false;
      // Lock on the resources, to try to find a command state which already
      // exists
      lock (m_lock)
      {
        // Try and get it, if fails then must be new and will build it up
        if (!m_commandState.TryGetValue(key, out commandState))
        {
          m_logger.LogInformation($"New state for key: {key}");
          // Build one
          commandState = new FuncState<TResult>();
          m_commandState.Add(key, commandState);
          isNew = true;
        } // end of if - new execution of command
      } // end of lock - 

      // If new, we will start the execution
      if (isNew)
      {
        m_logger.LogInformation($"Executing command: {key}");
        commandState.Started.Set();
        try
        {
          commandState.Result = action(input);
        } // end of try - to perform action
        // We will not catch exceptions, but we do always want to clean up
        // on our side, to prevent other threads being stuck in an infinite
        // waiting period.
        finally
        {
          // Mark as finished and remove from the collection
          commandState.Finished.Set();
          lock (m_lock)
            m_commandState.Remove(key);
        } // end of finally - perform some post-action cleanup
      } // end of if - is new
      else
      {
        m_logger.LogInformation($"Waiting for command to finish: {key}");
        commandState.WaitToFinish();
      } // end of else - just wait on the previous execution
      m_logger.LogInformation($"Result: {commandState.Result}");
      return commandState.Result;
    } /* End of Function - Execute */
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
    /************************ Types ******************************************/
    /// <summary>
    /// Represents a state of functor
    /// </summary>
    private class FuncState<ResultType>
    {
      /*===================== PUBLIC ========================================*/
      /********************** Events *****************************************/
      /********************** Properties *************************************/
      /// <summary>
      /// Event for when the functor has started
      /// </summary>
      public ManualResetEvent Started { get; } = new ManualResetEvent(false);
      /// <summary>
      /// Event for when the functor has finished
      /// </summary>
      public ManualResetEvent Finished { get; } = new ManualResetEvent(false);
      /// <summary>
      /// Result
      /// </summary>
      public ResultType Result { get; set; }
      /********************** Construction ***********************************/
      /********************** Methods ****************************************/
      /*--------------------- WaitToFinish ----------------------------------*/
      /// <summary>
      /// Waits for the <see cref="Started"/> and <see cref="Finished"/>
      /// events are signaled.
      /// </summary>
      public void WaitToFinish()
      {
        WaitHandle.WaitAll(new WaitHandle[]
          {
          Started,
          Finished
          });
      } // end of function - WaitToFinish
      /********************** Fields *****************************************/
      /********************** Static *****************************************/

      /*===================== PRIVATE =======================================*/
      /********************** Events *****************************************/
      /********************** Properties *************************************/
      /********************** Construction ***********************************/
      /********************** Methods ****************************************/
      /********************** Fields *****************************************/
      /// <summary>
      /// Lock object
      /// </summary>
      private readonly object m_lock = new object();
      /********************** Static *****************************************/

    } /* End of Class - ShellState */
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /************************ Fields *****************************************/
    /// <summary>
    /// state dictionary
    /// </summary>
    private Dictionary<string, FuncState<TResult>> m_commandState =
      new Dictionary<string, FuncState<TResult>>();
    /// <summary>
    ///  Lock object
    /// </summary>
    private readonly object m_lock = new object();
    /// <summary>
    /// Logger object
    /// </summary>
    private ILogger<FuncSync<TResult, TInput>> m_logger = null;
    /************************ Static *****************************************/
  } /* End of Class - FuncSync */
}
/* End of document - FuncSync.cs */