/******************************************************************************
 * File...: ShellServiceExtensionUnitTest.cs
 * Remarks: 
 */
using git_cache.Services.Extensions.DependencyInjection;
using git_cache.Services.Shell;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Runtime.InteropServices;

namespace git_cache.Services.mstest.Extensions.DependencyInjection
{
  /************************** ShellServiceExtensionUnitTest ******************/
  /// <summary>
  /// Unit tests for adding shell services to a service collection
  /// </summary>
  [TestClass]
  public class ShellServiceExtensionUnitTest
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /*----------------------- AddShell --------------------------------------*/
    /// <summary>
    /// Verifies the correct shell is added to services
    /// </summary>
    [TestMethod]
    public void AddShell()
    {
      // SETUP
      IServiceCollection services = new ServiceCollection();
      // the BashShell instance takes a logger object, so create
      // one and insert it
      services.AddSingleton(Substitute.For<ILogger<BashShell>>());

      // ACT
      services.AddShell();
      var serviceProvider = services.BuildServiceProvider();

      // VERIFY
      IShell shell = null;
      if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
      {
        bool threw = false;
        try
        {
          shell = serviceProvider.GetService<IShell>();
        }
        catch(InvalidOperationException)
        {
          threw = true;
        }
        Assert.IsTrue(threw, "Expected an exception of InvalidOperationException on Windows");
      } // end of if - running on Windows
      else
      {
        shell = serviceProvider.GetService<IShell>();
        Assert.IsNotNull(shell);
        Assert.IsInstanceOfType(shell, typeof(BashShell));
      } // end of else - running on non-windows
    } /* End of Function - AddShell */

    /*----------------------- AddUnixShell ----------------------------------*/
    /// <summary>
    /// Verifies the correct class is added for the unix shell
    /// </summary>
    [TestMethod]
    public void AddUnixShell()
    {
      // SETUP
      IServiceCollection services = new ServiceCollection();
      // the BashShell instance takes a logger object, so create
      // one and insert it
      var logger = Substitute.For<ILogger<BashShell>>();
      services.AddSingleton(logger);

      // ACT
      services.AddUnixShell();
      var serviceProvider = services.BuildServiceProvider();

      // VERIFY
      var shell = serviceProvider.GetService<IShell>();
      Assert.IsNotNull(shell);
      Assert.IsInstanceOfType(shell, typeof(BashShell));
    } /* End of Function - AddUnixShell */

    /*----------------------- AddWindowShell --------------------------------*/
    /// <summary>
    /// Tests the ability to add a windows shell object to the services
    /// </summary>
    /// <remarks>
    /// Currently not implemented so should throw!
    /// </remarks>
    [TestMethod]
    [ExpectedException(typeof(NotImplementedException))]
    public void AddWindowShell()
    {
      IServiceCollection services = new ServiceCollection();
      services.AddWindowShell();
    } /* End of Function - AddWindowShell */
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

  } /* End of Class - ShellServiceExtensionUnitTest */
}
/* End of document - ShellServiceExtensionUnitTest.cs */