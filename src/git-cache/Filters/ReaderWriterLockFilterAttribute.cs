/******************************************************************************
 * File...: ReaderWriterLockFilterAttribute.cs
 * Remarks: 
 */
using git_cache.Services.Configuration;
using git_cache.Services.Git;
using git_cache.Services.Git.Status;
using git_cache.Services.ResourceLock;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace git_cache.Filters
{
  /************************** ReaderWriterLockFilterAsyncAttribute ***********/
  /// <summary>
  /// Reader/writer lock filter, to assign to a controller
  /// </summary>
  public class ReaderWriterLockFilterAsyncAttribute : Attribute, IAsyncResourceFilter
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /*----------------------- ReaderWriterLockFilterAsyncAttribute ----------*/
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="config">
    /// Git configuration object
    /// </param>
    /// <param name="gitContext">
    /// Git context object
    /// </param>
    /// <param name="lockManager">
    /// Lock manager
    /// </param>
    /// <param name="logger">
    /// Logger for the object
    /// </param>
    /// <param name="remoteStatusSvc">
    /// Service for getting the reference status
    /// </param>
    public ReaderWriterLockFilterAsyncAttribute(
      IReaderWriterLockManager<string> lockManager,
      ILogger<ReaderWriterLockFilterAsyncAttribute> logger,
      IGitCacheConfiguration config,
      IRemoteStatus remoteStatusSvc,
      IGitContext gitContext)
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
      if(null == (RemoteStatusService = remoteStatusSvc))
        throw new ArgumentNullException(
          nameof(remoteStatusSvc), "Remote status service must be a valid value");
      if(null == (GitContext = gitContext))
        throw new ArgumentNullException(
          nameof(gitContext), "A valid git context must be provided");
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
          Logger.LogInformation($"  In milliseconds: {timeout.TotalMilliseconds}");
          // Get a lock object associated with the resource key
          var lockObj = Manager.GetFor(GetResourceKey(context));

          string auth = null;
          // Will look in the headers for an "Authentication" record
          var headers = context.HttpContext.Request.Headers;
          // If so, then we will get the value to use (assuming the first is
          // good enough, since there should just be one)
          if(headers.ContainsKey("Authorization"))
            auth = headers["Authorization"].First();

          var repo = GitContext.RemoteFactory.Build(context.RouteData.Values["destinationServer"].ToString(),
                                                    context.RouteData.Values["repositoryOwner"].ToString(),
                                                    context.RouteData.Values["repository"].ToString(),
                                                    auth);
          var local = GitContext.LocalFactory.Build(repo, Configuration);

          Logger.LogInformation("Grabbing reader lock");
          lockObj.AcquireReaderLock(timeout);
          if (!IsRepositoryUpToDate(local))
          {
            Logger.LogInformation("Repository out of date, asking for writer lock");
            //lockObj.AcquireWriterLock(timeout);
            lockObj.UpgradeToWriterLock(timeout);
          } // end of if - out-of-date

          try
          {
            // Check to see if we are out of date, if we are then
            // upgrade to writer, to update our local values
            if (lockObj.IsWriterLockHeld)
            {
              // If still out of date, then we will update the local one,
              // because we are the instance stuck with the work
              if (!IsRepositoryUpToDate(local))
              {
                Logger.LogInformation("We are responsible for updating the local repository");
                GitContext.UpdateLocalAsync(local).Wait();
                Logger.LogInformation("Local repository updated!");
              }
              lockObj.DowngradeFromWriterLock();
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
    /// <summary>
    /// Gets the <see cref="IRemoteStatus"/> object, for checking the status
    /// of branches
    /// </summary>
    protected IRemoteStatus RemoteStatusService { get; }
    /// <summary>
    /// Gets the <see cref="IGitContext"/> object
    /// </summary>
    protected IGitContext GitContext { get; }
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
    /// Checks to see if the repository is considered out of date
    /// </summary>
    /// <param name="context">
    /// Context object to get repository information from
    /// </param>
    protected virtual bool IsRepositoryUpToDate(
      ILocalRepository localRepository)
    {
      if (System.IO.Directory.Exists(localRepository.Path))
      {
        var status = RemoteStatusService.GetAsync(localRepository).Result;

        var outOfDateItems = status?.Where(v =>
        {
          switch (v.Flag)
          {
            case RefStatus.State.FailedOrRejected:
            case RefStatus.State.ForcedUpdated:
            case RefStatus.State.New:
            case RefStatus.State.Pruned:
            case RefStatus.State.TagUpdated:
            case RefStatus.State.Updated:
              return true;
            default:
            case RefStatus.State.UpToDate:
              return false;
          }
        }).ToList();
        return outOfDateItems.Count == 0;
      }
      else { return false; }
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
