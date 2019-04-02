/******************************************************************************
 * File...: ForwardedResultUnitTest.cs
 * Remarks: 
 */
using git_cache.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace git_cache_mstest.Results
{
  /************************** ForwardedResultUnitTest ************************/
  /// <summary>
  /// Verifies the behavior of <see cref="ForwardedResult"/>
  /// </summary>
  [TestClass]
  public class ForwardedResultUnitTest
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /*----------------------- CanForwardHttpResponseMessage -----------------*/
    /// <summary>
    /// Verifies the ExecuteResult of the result class
    /// </summary>
    [TestMethod]
    public void CanForwardHttpResponseMessage()
    {
      var msg = Substitute.For<HttpResponseMessage>();
      var fr = new ForwardedResult(msg);
      var httpContext = Substitute.ForPartsOf<HttpContext>();
      RouteData routeData = new RouteData();
      var context = new ActionContext(httpContext, routeData, new ActionDescriptor());
      fr.ExecuteResult(context);
      // MUST FIX THIS UNIT TEST
      Assert.IsTrue(false);
    } /* End of Function - CanForwardHttpResponseMessage */
    /************************ Fields *****************************************/
    /************************ Static *****************************************/
  } /* End of Class - ForwardedResultUnitTest */
} /* End of Namespace - git_cache_mstest */
/* End of document - ForwardedResultUnitTest.cs */