/******************************************************************************
 * File...: GitCacheExtension.cs
 * Remarks: 
 */
using git_cache.Services.Configuration;
using git_cache.Services.Git;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace git_cache.Services.Extensions.DependencyInjection
{
  /************************** GitCacheExtension ******************************/
  /// <summary>
  /// Provides DI related extension methods for using main services for Git
  /// operations
  /// </summary>
  public static class GitCacheExtension
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /************************ Fields *****************************************/
    /************************ Static *****************************************/
    /*----------------------- AddGitCacheServices ---------------------------*/
    /// <summary>
    /// Adds the necessary services for working with the git-cache
    /// </summary>
    /// <param name="services"></param>
    public static IServiceCollection AddGitCacheServices(
      this IServiceCollection services)
    {
      services.AddSingleton<IRemoteRepositoryFactory, RemoteRepositoryFactory>();
      services.AddSingleton<ILocalRepositoryFactory, LocalRepositoryFactory>();
      services.TryAddSingleton<IGitCacheConfiguration>(sp =>
        sp.GetRequiredService<IOptions<GitCacheConfiguration>>().Value
        );
      services
        .AddShell()
        .AddSingleton<IGitExecuter, GitExecuter>()
        .AddSingleton<IGitLFSExecuter, GitLFSExecutor>()
        .AddSingleton<IGitContext, GitContext>()
        .AddResourceLocks()
        .AddRemoteStatusService();
      return services;
    } /* End of Function - AddGitCacheServices */

  } /* End of Class - GitCacheExtension */
}
/* End of document - GitCacheExtension.cs */
