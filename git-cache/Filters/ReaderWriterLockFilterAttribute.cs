/******************************************************************************
 * File...: ReaderWriterLockFilterAttribute.cs
 * Remarks: 
 */
using git_cache.Services.Configuration;
using git_cache.Services.ResourceLock;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
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
      if (null == (Logger = logger))
        throw new ArgumentNullException(
          nameof(logger), "Logger must be a valid value");
      if (null == (Configuration = config))
        throw new ArgumentNullException(
          nameof(config), "Configuration must be a valid value");
    } /* End of Function - ReaderWriterLockFilterAsyncAttribute */
    /************************ Methods ****************************************/
    /*----------------------- OnResourceExecutionAsync ----------------------*/
    /// <summary>
    /// Wraps the execution with a lock, if out of date will get a writer lock
    /// otherwise will continue through as a reader
    /// </summary>
    /// <param name="context"></param>
    /// <param name="next"></param>
    /// <remarks>
    /// This method does not use the async keyword, due to `await` to possibly
    /// change the thread we are executing on, causing problems with the locks.
    /// </remarks>
    public Task OnResourceExecutionAsync(
      ResourceExecutingContext context,
      ResourceExecutionDelegate next)
    {
      if(null == context)
        throw new ArgumentNullException(
          nameof(context), "A valid context must be provided");

      if (null == next)
        throw new ArgumentNullException(
          nameof(next), "A valid execution delegate must be provided");

      return Task.Factory.StartNew(() =>
        {
          Logger.LogTrace("Enter main execution task");
          // Grab the timeout value from configuration
          var timeout = GetWaitTimeSpan();
          Logger.LogInformation($"Got a timeout from configuration: {timeout}");
          LockCookie lockCookie = default(LockCookie);
          // Get a lock object associated with the resource key
          var lockObj = Manager.GetFor(GetResourceKey(context));

          if (!IsRepositoryUpToDate(context))
          {
            Logger.LogInformation("Repository out of date, asking for writer lock");
            lockObj.AcquireWriterLock(timeout);
          } // end of if - out-of-date
          else
          {
            Logger.LogInformation("Repository is up to date, asking for reader lock");
            lockObj.AcquireReaderLock(timeout);
          } // end of else - up-to-date

          try
          {
            // Check to see if we are out of date, if we are then
            // upgrade to writer, to update our local values
            if (IsRepositoryUpToDate(context))
            {
              Logger.LogInformation("Repository updated since we asked, downgrading to reader lock");
              lockObj.DowngradeFromWriterLock(ref lockCookie);
            } // end of if - repository is up-to-date

            Logger.LogInformation("Calling through to the next step in the pipeline");
            // Allow the rest of the pipeline to continue
            next().Wait();

            Logger.LogInformation("Next action has finished, continue to release locks");
          } // end of try - to execute the job
          finally
          {
            // If we were holding the writer lock, release it
            // first
            if (lockObj.IsWriterLockHeld)
            {
              Logger.LogInformation("Releasing the writer lock");
              lockObj.ReleaseWriterLock();
              Logger.LogInformation("Writer lock released");
            } // end of if - writer lock is held
            else
            {
              Logger.LogInformation("Releasing the reader lock");
              lockObj.ReleaseReaderLock();
            } // end of else - reader lock
          } // end of finally
          lockObj = null;
          Logger.LogTrace("Exit main execution task");
        }); // end of task
    } /* End of Function - OnResourceExecutionAsync */
    /************************ Fields *****************************************/
    /************************ Static *****************************************/

    /*======================= PROTECTED =====================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /// <summary>
    /// Gets the configuration
    /// </summary>
    protected IGitCacheConfiguration Configuration { get; }
    /// <summary>
    /// Gets the logger associated with this instance
    /// </summary>
    protected ILogger Logger { get; }
    /// <summary>
    /// Gets the reader/writer lock manager for getting locks for resources
    /// </summary>
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
      Logger.LogInformation($"Generated resource key: {key}");
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
      Logger.LogInformation($"Wait time span: {waitTime}");
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
