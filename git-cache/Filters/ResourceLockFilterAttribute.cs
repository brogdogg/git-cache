/******************************************************************************
 * File...: ResourceLockFilterAttribute.cs
 * Remarks: 
 */
using git_cache.Services.Configuration;
using git_cache.Services.ResourceLock;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace git_cache.Filters
{
  /************************** BaseResourceLockFilterAttribute ****************/
  /// <summary>
  /// Abstract implementation of a resoruce lock filter to use with a
  /// controller
  /// </summary>
  public abstract class BaseResourceLockFilterAttribute : Attribute
  {

    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /************************ Fields *****************************************/
    /************************ Static *****************************************/

    /*======================= PROTECTED =====================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /// <summary>
    /// Gets the lock manager
    /// </summary>
    protected IResourceLockManager<string> LockManager { get; }
    /// <summary>
    /// Gets the logger associated with this instance
    /// </summary>
    protected ILogger Logger { get; }
    /// <summary>
    /// Gets the configuration
    /// </summary>
    protected IGitCacheConfiguration Configuration { get; }
    /// <summary>
    /// Gets/Sets the resource lock
    /// </summary>
    protected IResourceLock Lock { get; set; }
    /************************ Construction ***********************************/
    /*----------------------- BaseResourceLockFilterAttribute ---------------*/
    /// <summary>
    /// Constructor for initializing required components
    /// </summary>
    /// <param name="lockManager">
    /// Required lock manager
    /// </param>
    /// <param name="logger">
    /// Required logger to use for the instance
    /// </param>
    /// <param name="config">
    /// Required configuration object
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// If any of the arguments are null
    /// </exception>
    protected BaseResourceLockFilterAttribute(IResourceLockManager<string> lockManager,
                                              ILogger logger,
                                              IGitCacheConfiguration config)
    {
      if (null == (LockManager = lockManager))
        throw new ArgumentNullException(
          nameof(lockManager), "Lock manager must be a valid value");
      if(null == (Logger = logger))
        throw new ArgumentNullException(
          nameof(logger), "Logger must be a valid value");
      if(null == (Configuration = config))
        throw new ArgumentNullException(
          nameof(config), "Configuration must be a valid value");
    } /* End of Function - BaseResourceLockFilterAttribute */

    /************************ Methods ****************************************/
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

    /*----------------------- LockResource ----------------------------------*/
    /// <summary>
    /// Locks resource based on information from the context object
    /// </summary>
    /// <param name="context">
    /// Context object, to glean repository information from
    /// </param>
    protected virtual void LockResource(ResourceExecutingContext context)
    {
      Logger.LogTrace("Attempting to lock resource");
      Lock = LockManager.BlockFor(GetResourceKey(context), GetWaitTimeSpan());
      Logger.LogTrace("Locked on scenario");
    } /* End of Function - LockResource */

    /*----------------------- ReleaseLockResource ---------------------------*/
    /// <summary>
    /// Releases the resource lock
    /// </summary>
    protected virtual void ReleaseLockResource()
    {
      Logger.LogTrace("Releasing resource lock");
      Lock?.Release();
      Logger.LogTrace("Lock released");
    } /* End of Function - ReleaseLockResource */
    /************************ Fields *****************************************/
    /************************ Static *****************************************/
  } /* End of Class - BaseResourceLockFilterAttribute */


  /************************** ResourceLockFilterAsyncAttribute ***************/
  /// <summary>
  /// 
  /// </summary>
  public class ResourceLockFilterAsyncAttribute :
    BaseResourceLockFilterAttribute, IAsyncResourceFilter
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /*----------------------- ResourceLockFilterAsyncAttribute --------------*/
    /// <summary>
    /// 
    /// </summary>
    /// <param name="lockManager"></param>
    /// <param name="logger"></param>
    /// <param name="config"></param>
    public ResourceLockFilterAsyncAttribute(IResourceLockManager<string> lockManager,
                                            ILogger<ResourceLockFilterAsyncAttribute> logger,
                                            IGitCacheConfiguration config)
      :base(lockManager, logger, config)
    {
      ;
    } /* End of Function - ResourceLockFilterAsyncAttribute */
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
      // Lock the resource for the given context
      LockResource(context);
      try
      {
        await next();
      } // end of try - to wait for the rest of the pipeline
      finally
      {
        ReleaseLockResource();
      } // end of finally - make sure to always release the resource
    } /* End of Function - OnResourceExecutionAsync */
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
    /************************ Static *****************************************/
  } /* End of Class - ResourceLockFilterAsyncAttribute */


  /************************** ResourceLockFilterAttribute ********************/
  /// <summary>
  /// Filter to be used for locking resources
  /// </summary>
  public class ResourceLockFilterAttribute : BaseResourceLockFilterAttribute, IResourceFilter
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /*----------------------- ResourceLockFilterAttribute -------------------*/
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="config">
    /// Required configuration object
    /// </param>
    /// <param name="lockMgr">
    /// Required lock manager
    /// </param>
    /// <param name="logger">
    /// Required logger object
    /// </param>
    public ResourceLockFilterAttribute(
      IResourceLockManager<string> lockMgr,
      ILogger<ResourceLockFilterAttribute> logger,
      IGitCacheConfiguration config)
      :base(lockMgr, logger, config)
    {
      ;
    } /* End of Function - ResourceLockFilterAttribute */
    /************************ Methods ****************************************/

    /*----------------------- OnResourceExecuted ----------------------------*/
    /// <summary>
    /// Releases the lock obtained on the executing handler.
    /// </summary>
    /// <param name="context">
    /// Context object
    /// </param>
    public void OnResourceExecuted(ResourceExecutedContext context)
    {
      ReleaseLockResource();
    } /* End of Function - OnResourceExecuted */

    /*----------------------- OnResourceExecuting ---------------------------*/
    /// <summary>
    /// Before anything happens, we will get a chance to lock on the
    /// resources
    /// </summary>
    /// <param name="context">
    /// Context for the request
    /// </param>
    /// <remarks>
    /// Expects the route data from the context to contain three values keyed
    /// to:
    /// - destinationServer
    /// - repositoryOwner
    /// - repository
    /// </remarks>
    public void OnResourceExecuting(ResourceExecutingContext context)
    {
      LockResource(context);
    } /* End of Function - OnResourceExecuting */
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
    /************************ Static *****************************************/
  } /* End of Class - ResourceLockFilterAttribute */
}
/* End of document - ResourceLockFilterAttribute.cs */