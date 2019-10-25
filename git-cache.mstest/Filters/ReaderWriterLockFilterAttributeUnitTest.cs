/******************************************************************************
 * File...: ReaderWriterLockFilterAttributeUnitTest.cs
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

namespace git_cache.mstest.Filters
{
  /* ReaderWriterLockFilterAttributeUnitTest *********************************/
  /// <summary>
  /// Unit tests for the <see cref="ReaderWriterLockFilterAsyncAttribute"/>
  /// class.
  /// </summary>
  [TestClass]
  public class ReaderWriterLockFilterAttributeUnitTest
  {
    /* PUBLIC ===============================================================*/
    /* Events ****************************************************************/
    /* Properties ************************************************************/
    /* Construction **********************************************************/
    /* Methods ***************************************************************/
    /* Initialize -----------------------------------------------------------*/
    /// <summary>
    /// Initializer for the test cases
    /// </summary>
    [TestInitialize]
    public void Initialize()
    {
      m_logger = Substitute.For<ILogger<ReaderWriterLockFilterAsyncAttribute>>();
      m_config = Substitute.For<IGitCacheConfiguration>();
      m_mgr = Substitute.For<IReaderWriterLockManager<string>>();
      m_lock = Substitute.For<IReaderWriterLock>();

      m_mgr.GetFor($"{m_testServer}_{m_owner}_{m_repo}")
           .Returns(m_lock);

      int opTimeoutMs = 1000;
      m_timeout = TimeSpan.FromMilliseconds(opTimeoutMs);
      m_config.OperationTimeout.Returns(opTimeoutMs);

      var routeValueDict = new RouteValueDictionary();
      routeValueDict.Add("destinationServer", m_testServer);
      routeValueDict.Add("repositoryOwner", m_owner);
      routeValueDict.Add("repository", m_repo);
      m_routeData = new RouteData(routeValueDict);

      m_actionContext = new ActionContext(
        new DefaultHttpContext(),
        m_routeData,
        new ActionDescriptor(),
        new ModelStateDictionary());

    } /* End of Function - Initialize */

    /* ThrowsForInvalidLockManager ------------------------------------------*/
    /// <summary>
    /// Verifies the constructor throws when the
    /// <see cref="ReaderWriterLockManager{TKey}"/> is null.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ThrowsForInvalidLockManager()
    {
      var lockObj = new ReaderWriterLockFilterAsyncAttribute(null, m_logger, m_config);
    } /* End of Function - ThrowsForInvalidLockManager */

    /* ThrowsForInvalidLogger -----------------------------------------------*/
    /// <summary>
    /// Verifies the constructor throws when the
    /// <see cref="ILogger{TCategoryName}"/> is null.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ThrowsForInvalidLogger()
    {
      var lockO = new ReaderWriterLockFilterAsyncAttribute(m_mgr, null, m_config);
    } /* End of Function - ThrowsForInvalidLogger */

    /* ThrowsForInvalidConfig -----------------------------------------------*/
    /// <summary>
    /// Verifies the constructor throws when
    /// <see cref="IGitCacheConfiguration"/> is null.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ThrowsForInvalidConfig()
    {
      var lockO = new ReaderWriterLockFilterAsyncAttribute(m_mgr, m_logger, null);
    } /* End of Function - ThrowsForInvalidConfig */

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ThrowsOnExecutionWithInvalidContext()
    {
      var lockObj = new ReaderWriterLockFilterAsyncAttribute(m_mgr, m_logger, m_config);
      lockObj.OnResourceExecutionAsync(null, Substitute.For<ResourceExecutionDelegate>()).Wait();
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ThrowsOnExecutionWithInvalidNext()
    {
      var filters = Substitute.For<IList<IFilterMetadata>>();
      ResourceExecutingContext exingContext =
        new ResourceExecutingContext(
          m_actionContext,
          filters,
          Substitute.For<IList<IValueProviderFactory>>());
      var lockObj = new ReaderWriterLockFilterAsyncAttribute(m_mgr, m_logger, m_config);
      lockObj.OnResourceExecutionAsync(exingContext, null).Wait();
    }

    [TestMethod]
    public void InvalidRouteDataProvided()
    {
      var filters = Substitute.For<IList<IFilterMetadata>>();

      var actionContext = new ActionContext(
            new DefaultHttpContext(),
            m_routeData,
            new ActionDescriptor(),
            new ModelStateDictionary());

      ResourceExecutingContext exingContext =
        new ResourceExecutingContext(
          actionContext,
          filters,
          Substitute.For<IList<IValueProviderFactory>>());

      var lockObj = new ReaderWriterLockFilterAsyncAttribute(m_mgr, m_logger, m_config);
      var execDel = Substitute.For<ResourceExecutionDelegate>();
      lockObj.OnResourceExecutionAsync(exingContext, execDel).Wait();
    }
    /* Fields ****************************************************************/
    /* Static ****************************************************************/

    /* PROTECTED ============================================================*/
    /* Events ****************************************************************/
    /* Properties ************************************************************/
    /* Construction **********************************************************/
    /* Methods ***************************************************************/
    /* Fields ****************************************************************/
    /* Static ****************************************************************/

    /* PRIVATE ==============================================================*/
    /* Events ****************************************************************/
    /* Properties ************************************************************/
    /* Construction **********************************************************/
    /* Methods ***************************************************************/
    /* Fields ****************************************************************/
    ILogger<ReaderWriterLockFilterAsyncAttribute> m_logger;
    IGitCacheConfiguration m_config;
    IReaderWriterLockManager<string> m_mgr;
    string m_testServer = "test";
    string m_owner = "owner";
    string m_repo = "repo";
    TimeSpan m_timeout;
    IReaderWriterLock m_lock;
    ResourceExecutingContext m_executingContext;
    RouteData m_routeData;
    ActionContext m_actionContext;
    /* Static ****************************************************************/

  } /* End of Class - ReaderWriterLockFilterAttributeUnitTest */
}
/* End of document - ReaderWriterLockFilterAttributeUnitTest.cs */