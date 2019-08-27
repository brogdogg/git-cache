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

    /*----------------------- AddThreadSafeShell ----------------------------*/
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    public static IServiceCollection AddThreadSafeShell(this IServiceCollection services)
    {
      // Register just types, to use DI for constructing
      services.AddSingleton<BashShell>();
      //services.AddSingleton<WindowShell>();
      services.AddSingleton<ThreadSafeShell>();
      // Then for the actual interface, we will use a lamba to
      // build it up dynamically
      services.AddSingleton<IShell>(sp =>
      {
        var retval = sp.GetRequiredService<ThreadSafeShell>();
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
          throw new InvalidOperationException("Shell for windows is not implemented");
        else
          retval.OSShell = sp.GetRequiredService<BashShell>();
        return retval;
      });
      return services;
    } /* End of Function - AddThreadSafeShell */

    public static IServiceCollection AddShell(this IServiceCollection services)
    {
      services.AddSingleton<BashShell>();
      // TODO: Add Windows shell???
      //services.AddSingleton<WindowsShell>();
      services.AddSingleton<IShell>(sp =>
      {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
          throw new InvalidOperationException("Windows shell is not implemented as of yet");
        else
          return sp.GetRequiredService<BashShell>();
      });
      return services;
    }
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