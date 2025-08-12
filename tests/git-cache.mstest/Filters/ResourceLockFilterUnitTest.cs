/******************************************************************************
 * File...: ResourceLockFilterUnitTest.cs
 * Remarks: 
 */
using git_cache.Filters;
using git_cache.Services.Configuration;
using git_cache.Services.ResourceLock;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;

namespace git_cache.mstest.Filters
{
  /************************** ResourceLockFilterUnitTest *********************/
  /// <summary>
  /// 
  /// </summary>
  [TestClass]
  public class ResourceLockFilterUnitTest
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /*----------------------- Initialize ------------------------------------*/
    /// <summary>
    /// Initializes common fakes
    /// </summary>
    [TestInitialize]
    public void Initialize()
    {
      m_logger = Substitute.For<ILogger<ResourceLockFilterAttribute>>();
      m_config = Substitute.For<IGitCacheConfiguration>();
      m_mgr = Substitute.For<IResourceLockManager<string>>();
      m_lock = Substitute.For<IResourceLock>();

      var routeValueDict = new RouteValueDictionary();
      routeValueDict.Add("destinationServer", m_testServer);
      routeValueDict.Add("repositoryOwner", m_owner);
      routeValueDict.Add("repository", m_repo);
      m_routeData = new RouteData(routeValueDict);

      int opTimeout = 1000;
      m_timeout = TimeSpan.FromMilliseconds(opTimeout);
      m_config.OperationTimeout.Returns(opTimeout);

      m_mgr.BlockFor(
        $"{m_testServer}_{m_owner}_{m_repo}",
        m_timeout).Returns(m_lock);

      m_actionContext = new ActionContext(
        new DefaultHttpContext(),
        m_routeData,
        new ActionDescriptor(),
        new ModelStateDictionary());


    } /* End of Function - Initialize */
    /*----------------------- OnResourceExecuting ---------------------------*/
    /// <summary>
    /// 
    /// </summary>
    [TestMethod]
    public void OnResourceExecuting()
    {
      ResourceExecutingContext context = new ResourceExecutingContext(
        m_actionContext,
        Substitute.For<IList<IFilterMetadata>>(),
        Substitute.For<IList<IValueProviderFactory>>());

      var filter = new ResourceLockFilterAttribute(m_mgr, m_logger, m_config);
      filter.OnResourceExecuting(context);
      m_mgr.Received(1).BlockFor("test_owner_repo", m_timeout);
    } /* End of Function - OnResourceExecuting */

    /*----------------------- OnResourceExecutedWithoutExecuting ------------*/
    /// <summary>
    /// Verifies release is not called on an object that was
    /// not obtained
    /// </summary>
    [TestMethod]
    public void OnResourceExecutedWithoutExecuting()
    {
      ResourceExecutedContext context = new ResourceExecutedContext(
        m_actionContext,
        Substitute.For<IList<IFilterMetadata>>());

      var filter = new ResourceLockFilterAttribute(m_mgr, m_logger, m_config);
      filter.OnResourceExecuted(context);
      m_lock.Received(0).Release();
    } /* End of Function - OnResourceExecutedWithoutExecuting */

    /*----------------------- OnResourceExecutedAfterExecuting --------------*/
    /// <summary>
    /// Verifies we call lock release once we have executed
    /// </summary>
    [TestMethod]
    public void OnResourceExecutedAfterExecuting()
    {
      var filters = Substitute.For<IList<IFilterMetadata>>();
      ResourceExecutingContext executingContext =
        new ResourceExecutingContext(
          m_actionContext,
          filters,
          Substitute.For<IList<IValueProviderFactory>>());

      ResourceExecutedContext executedContext =
        new ResourceExecutedContext(
          m_actionContext,
          filters);

      var filter = new ResourceLockFilterAttribute(m_mgr, m_logger, m_config);
      filter.OnResourceExecuting(executingContext);
      filter.OnResourceExecuted(executedContext);
      m_lock.Received(1).Release();
    } /* End of Function - OnResourceExecutedAfterExecuting */

    /*----------------------- ThrowsOnNullConfig ----------------------------*/
    /// <summary>
    /// Verifies the constructor throws when a null-value config is provided
    /// during construction.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ThrowsOnNullConfig()
    {
      var filter = new ResourceLockFilterAttribute(m_mgr, m_logger, null);
    } /* End of Function - ThrowsOnNullConfig */

    /*----------------------- ThrowsOnNullManager ---------------------------*/
    /// <summary>
    /// Verifies the constructor throws when the resource lock manager
    /// is null
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ThrowsOnNullManager()
    {
      var filter = new ResourceLockFilterAttribute(null, m_logger, m_config);
    } /* End of Function - ThrowsOnNullManager */

    /*----------------------- ThrowsOnNullLogger ----------------------------*/
    /// <summary>
    /// Verifies the constructor throws when the logger is null
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ThrowsOnNullLogger()
    {
      var filter = new ResourceLockFilterAttribute(m_mgr, null, m_config);
    } /* End of Function - ThrowsOnNullLogger */
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
    ILogger<ResourceLockFilterAttribute> m_logger;
    IGitCacheConfiguration m_config;
    IResourceLockManager<string> m_mgr;
    RouteData m_routeData;
    ActionContext m_actionContext;
    string m_testServer = "test";
    string m_owner = "owner";
    string m_repo = "repo";
    TimeSpan m_timeout;
    IResourceLock m_lock;
    /************************ Static *****************************************/

  } /* End of Class - ResourceLockFilterUnitTest */
}
/* End of document - ResourceLockFilterUnitTest.cs */