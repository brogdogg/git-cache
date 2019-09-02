﻿/******************************************************************************
 * File...: ResourceLockFilterAttribute.cs
 * Remarks: 
 */
using git_cache.Services.Configuration;
using git_cache.Services.ResourceLock;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;

namespace git_cache.Filters
{
  /************************** ResourceLockFilterAttribute ********************/
  /// <summary>
  /// Filter to be used for locking resources
  /// </summary>
  public class ResourceLockFilterAttribute : Attribute, IResourceFilter
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /*----------------------- ResourceLockFilterAttribute -------------------*/
    /// <summary>
    /// 
    /// </summary>
    public ResourceLockFilterAttribute(
      IGitCacheConfiguration config,
      IResourceLockManager<string> lockMgr,
      ILogger<ResourceLockFilterAttribute> logger)
    {
      if (null == (m_lockManager = lockMgr))
        throw new ArgumentNullException(nameof(lockMgr), "A valid lock manager must be provided");
      if (null == (m_logger = logger))
        throw new ArgumentNullException(nameof(logger), "Must provide a valid logger object");
      if (null == (m_config = config))
        throw new ArgumentNullException(nameof(config), "Must provide a valid configuration");
    } /* End of Function - ResourceLockFilterAttribute */
    /************************ Methods ****************************************/
    /*----------------------- OnResourceExecuted ----------------------------*/
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public void OnResourceExecuted(ResourceExecutedContext context)
    {
      m_logger.LogTrace("Finished processing request, releasing lock");
      if (null != m_lock)
        m_lock.Release();
      m_logger.LogTrace("Lock released");
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
      m_logger.LogTrace("Processing for new request");
      var waitTime = TimeSpan.FromMilliseconds(m_config.OperationTimeout);
      var key = string.Format("{0}_{1}_{2}",
        context.RouteData.Values["destinationServer"],
        context.RouteData.Values["repositoryOwner"],
        context.RouteData.Values["repository"]);
      m_logger.LogDebug($"Key used for obtaining lock: {key}");
      m_logger.LogDebug($"Will wait for {waitTime}");
      m_lock = m_lockManager.BlockFor(key, waitTime);
      m_logger.LogTrace("Locked on scenario");
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
    private IResourceLock m_lock;
    private readonly IResourceLockManager<string> m_lockManager;
    private readonly ILogger<ResourceLockFilterAttribute> m_logger;
    private readonly IGitCacheConfiguration m_config;
    /************************ Static *****************************************/
  } /* End of Class - ResourceLockFilterAttribute */
}
/* End of document - ResourceLockFilterAttribute.cs */