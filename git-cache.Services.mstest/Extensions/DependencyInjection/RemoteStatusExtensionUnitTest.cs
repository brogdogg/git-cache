/******************************************************************************
 * File...: RemoteStatusExtensionUnitTest.cs
 * Remarks: 
 */
using git_cache.Services.Extensions.DependencyInjection;
using git_cache.Services.Git;
using git_cache.Services.Git.Status;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;

namespace git_cache.Services.mstest.Extensions.DependencyInjection
{
  /************************** RemoteStatusExtensionUnitTest ******************/
  /// <summary>
  /// Verifies the extension method for adding the <see cref="IRemoteStatus"/>
  /// service to the collection.
  /// </summary>
  [TestClass]
  public class RemoteStatusExtensionUnitTest : BaseExtensionUnitTest
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /*----------------------- AddsRemoteStatusToServices --------------------*/
    /// <summary>
    /// Verifies the <see cref="RemoteStatus"/> class object is added
    /// to the services for the <see cref="IRemoteStatus"/> interface.
    /// </summary>
    [TestMethod]
    public void AddsRemoteStatusToServices()
    {
      // Setup
      // Act
      Services.AddRemoteStatusService();
      Services.AddSingleton<IGitExecuter>(Substitute.For<IGitExecuter>());
      var serviceProvider = Services.BuildServiceProvider();
      // Verify
      Assert.IsInstanceOfType(
        serviceProvider.GetService<IRemoteStatus>(),
        typeof(RemoteStatus));
    } /* End of Function - AddsRemoteStatusToServices */
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

  } /* End of Class - RemoteStatusExtensionUnitTest */
}
/* End of document - RemoteStatusExtensionUnitTest.cs */