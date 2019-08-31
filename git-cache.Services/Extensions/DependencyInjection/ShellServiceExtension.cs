/******************************************************************************
 * File...: ShellServiceExtension.cs
 * Remarks: 
 */
using git_cache.Services.Shell;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Runtime.InteropServices;

namespace git_cache.Services.Extensions.DependencyInjection
{
  /************************** ShellServiceExtension **************************/
  /// <summary>
  /// Provides a service extension for setting up shell services
  /// </summary>
  public static class ShellServiceExtension
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /************************ Fields *****************************************/
    /************************ Static *****************************************/

    /*----------------------- AddShell --------------------------------------*/
    /// <summary>
    /// Adds a <see cref="IShell"/> service to the services
    /// </summary>
    /// <param name="services"></param>
    public static IServiceCollection AddShell(this IServiceCollection services)
    {
      // Add a bash shell
      services.AddSingleton<BashShell>();

      // TODO: Add Windows shell???

      // For the interface, use system information to decide
      services.AddSingleton<IShell>(sp =>
      {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
          throw new InvalidOperationException("Windows shell is not implemented as of yet");
        else
          return sp.GetRequiredService<BashShell>();
      });
      return services;
    } /* End of Function - AddShell */
    /*----------------------- AddUnixShell ----------------------------------*/
    /// <summary>
    /// Adds the Unix shell service
    /// </summary>
    /// <param name="services">
    /// Service collection to add to
    /// </param>
    public static IServiceCollection AddUnixShell(this IServiceCollection services)
    {
      services.AddTransient<IShell, BashShell>();
      return services;
    } /* End of Function - AddUnixShell */

    /*----------------------- AddWindowShell --------------------------------*/
    /// <summary>
    /// Adds the Window's shell service
    /// </summary>
    /// <param name="services">
    /// Service collection to add to
    /// </param>
    public static IServiceCollection AddWindowShell(this IServiceCollection services)
    {
      throw new NotImplementedException();
    } /* End of Function - AddWindowShell */

  } /* End of Class - ShellServiceExtension */
}
/* End of document - ShellServiceExtension.cs */