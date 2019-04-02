/******************************************************************************
 * File...: Startup.cs
 * Remarks: 
 */
using git_cache.Git;
using git_cache.Git.LFS;
using git_cache.Results;
using git_cache.Shell;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Unity;

namespace git_cache
{
  /************************** Startup ****************************************/
  /// <summary>
  /// Main startup class for the web api
  /// </summary>
  public class Startup
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Configuration **********************************/
    /// <summary>
    /// Gets the configuration passed in during construction
    /// </summary>
    public IConfiguration Configuration { get; } = null;
    /************************ Container **************************************/
    /// <summary>
    /// Gets the unity container
    /// </summary>
    public IUnityContainer Container { get; } = null;
    /************************ Construction ***********************************/
    /*----------------------- Startup ---------------------------------------*/
    /// <summary>
    /// Constructor for the class
    /// </summary>
    /// <param name="configuration">
    /// Configuration for the application
    /// </param>
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
      Container = new UnityContainer();
    } /* End of Function - Startup */

    /************************ Methods ****************************************/
    /*----------------------- ConfigureServices -----------------------------*/
    /// <summary>
    /// This method gets called by the runtime. Use this method to add services
    /// to the container.
    /// </summary>
    /// <param name="services"></param>
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddMvc();
      // Go ahead and add the configuration we were given
      services.AddSingleton(Configuration);
      // Add a bash shell to the services
      services.AddTransient<IShell, BashShell>();
      // And add the Git Context services
      ConfigureGitContextService(services);
      //Container.RegisterType<IBatchRequestObject, BatchRequestObject>();
      services.AddSingleton(Container);
    } /* End of Function - ConfigureServices */

    /*----------------------- ConfigureGitContextService --------------------*/
    /// <summary>
    /// Adds the appropriate services for the Git Context
    /// </summary>
    /// <param name="services">
    /// Service collection
    /// </param>
    public void ConfigureGitContextService(IServiceCollection services)
    {
      if (null == services)
        throw new ArgumentNullException("Service collection cannot be null");
      // Add services for what is needed for the Git context
      services.AddTransient<IRemoteRepositoryFactory, RemoteFactory>();
      services.AddTransient<ILocalRepositoryFactory, LocalFactory>();
      services.AddTransient<IGitExecuter, GitExecuter>();
      services.AddTransient<IGitLFSExecuter, GitLFSExecutor>();
      // Finally add the git context service
      services.AddSingleton<IGitContext, GitContext>();
    } /* End of Function - ConfigureGitContextService */

    /*----------------------- Configure -------------------------------------*/
    /// <summary>
    /// This method gets called by the runtime. Use this method to configure
    /// the HTTP request pipeline.
    /// </summary>
    /// <param name="app"></param>
    /// <param name="env"></param>
    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      app.UseMvc();
    } /* End of Function - Configure */
    /************************ Fields *****************************************/
    /************************ Static *****************************************/
    /************************ Types ******************************************/
    /// <summary>
    /// 
    /// </summary>
    public class RemoteFactory : IRemoteRepositoryFactory
    {
      public IRemoteRepository Build(string server, string owner, string name, string auth)
      {
        return new RemoteRepository(server, owner, name, AuthInfo.ParseAuth(auth));
      }
    }
    /// <summary>
    /// 
    /// </summary>
    public class LocalFactory : ILocalRepositoryFactory
    {
      public ILocalRepository Build(IRemoteRepository repo, IConfiguration config)
      {
        return new LocalRepository(repo, config);
      }
    }

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

  } /* End of Class - Startup */
}
/* End of document - Startup.cs */