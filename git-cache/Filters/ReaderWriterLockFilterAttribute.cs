/******************************************************************************
 * File...: ReaderWriterLockFilterAttribute.cs
 * Remarks: 
 */
using git_cache.Services.Configuration;
using git_cache.Services.ResourceLock;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace git_cache.Filters
{
  /************************** ReaderWriterLockFilterAsyncAttribute ***********/
  /// <summary>
  /// 
  /// </summary>
  public class ReaderWriterLockFilterAsyncAttribute : Attribute, IAsyncResourceFilter
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /*----------------------- ReaderWriterLockFilterAsyncAttribute ----------*/
    /// <summary>
    /// 
    /// </summary>
    /// <param name="lockManager"></param>
    /// <param name="logger"></param>
    /// <param name="config"></param>
    public ReaderWriterLockFilterAsyncAttribute(
      IReaderWriterLockManager<string> lockManager,
      ILogger<ReaderWriterLockFilterAsyncAttribute> logger,
      IGitCacheConfiguration config)
    {
      if (null == (Manager = lockManager))
        throw new ArgumentNullException(
          nameof(lockManager), "Lock manager must be a valid value");
      if(null == (Logger = logger))
        throw new ArgumentNullException(
          nameof(logger), "Logger must be a valid value");
      if(null == (Configuration = config))
        throw new ArgumentNullException(
          nameof(config), "Configuration must be a valid value");
    } /* End of Function - ReaderWriterLockFilterAsyncAttribute */
    /************************ Methods ****************************************/
    /*----------------------- OnResourceExecutionAsync ----------------------*/
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    /// <param name="next"></param>
    public async Task OnResourceExecutionAsync(
      ResourceExecutingContext context,
      ResourceExecutionDelegate next)
    {
      Logger.LogTrace("ResourceExecuting for reader/writer lock");
      // Grab the timeout value from configuration
      var timeout = GetWaitTimeSpan();
      Logger.LogTrace($"Got a timeout from configuration: {timeout}");
      LockCookie lockCookie = default(LockCookie);
      // Get a lock object associated with the resource key
      Lock = Manager.GetFor(GetResourceKey(context));
      // Become a reader first, since we will always want to be
      // a reader
      Logger.LogTrace("Acquiring reader lock for resource");
      Lock.AcquireReaderLock(timeout);
      try
      {
        // Check to see if we are out of date, if we are then
        // upgrade to writer, to update our local values
        if (!IsRepositoryUpToDate(context))
        {
          Logger.LogTrace("Local resources is out of date, upgrading to a writer lock");
          lockCookie = Lock.UpgradeToWriterLock(timeout);
        }

        Logger.LogTrace("Calling through to the next step in the pipeline");
        // Allow the rest of the pipeline to continue
        await next();
        Logger.LogTrace("Got control again.");

        // If we were holding the writer lock, release it
        // first
        if (Lock.IsWriterLockHeld)
        {
          Logger.LogTrace("Releasing the writer lock");
          Lock.ReleaseWriterLock();
          Logger.LogTrace("Writer lock reelased");
        } // end of if - writer lock is held
      } // end of try - 
      finally
      {
        Logger.LogTrace("Releasing the lock");
        Lock.ReleaseLock();
      } // end of finally
      Lock = null;
      return;
    } /* End of Function - OnResourceExecutionAsync */
    /************************ Fields *****************************************/
    /************************ Static *****************************************/

    /*======================= PROTECTED =====================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    protected IGitCacheConfiguration Configuration { get; }
    protected IReaderWriterLock Lock { get; set; }
    protected ILogger Logger { get; }
    protected IReaderWriterLockManager<string> Manager { get; }
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /************************ Fields *****************************************/
    /*----------------------- GetResourceKey --------------------------------*/
    /// <summary>
    /// Constructs a key from the context object based on the destination
    /// server, repository owner and the repository name
    /// </summary>
    /// <param name="context"></param>
    protected virtual string GetResourceKey(
      ResourceExecutingContext context)
    {
      var key = string.Format("{0}_{1}_{2}",
        context.RouteData.Values["destinationServer"],
        context.RouteData.Values["repositoryOwner"],
        context.RouteData.Values["repository"]);
      Logger?.Log(LogLevel.Information, $"Generated resource key: {key}");
      return key;
    } /* End of Function - GetResourceKey */
    /*----------------------- GetWaitTimeSpan -------------------------------*/
    /// <summary>
    /// Gets the <see cref="TimeSpan"/> representation of the
    /// OperationTimeout from the <see cref="Configuration"/> object.
    /// </summary>
    protected virtual TimeSpan GetWaitTimeSpan()
    {
      var waitTime = TimeSpan.FromMilliseconds(Configuration.OperationTimeout);
      Logger.Log(LogLevel.Information, $"Wait time span: {waitTime}");
      return waitTime;
    } /* End of Function - GetWaitTimeSpan */

    /*----------------------- IsRepositoryUpToDate --------------------------*/
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context">
    /// Context object to get repository information from
    /// </param>
    protected virtual bool IsRepositoryUpToDate(
      ResourceExecutingContext context)
    {
      return false;
    } /* End of Function - IsRepositoryUpToDate */
    /************************ Static *****************************************/

    /*======================= PRIVATE =======================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /************************ Fields *****************************************/
    /************************ Static *****************************************/

  } /* End of Class - ReaderWriterLockFilterAsyncAttribute */
}
/* End of document - ReaderWriterLockFilterAttribute.cs */