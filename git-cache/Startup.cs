/******************************************************************************
 * File...: Startup.cs
 * Remarks: 
 */
using git_cache.Git;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
    public IConfiguration Configuration { get; } /* End of Property - Configuration */
    public IRemoteRepositoryFactory  RemoteRepoFactory { get; }
    public ILocalRepositoryFactory LocalRepoFactory { get; }
    public IGitContext Context { get; } = null;
    public IGitExecuter GitExec { get; } = null;
    public IGitLFSExecuter LFSExec { get; } = null;
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
      RemoteRepoFactory = new RemoteFactory();
      LocalRepoFactory = new LocalFactory();
      GitExec = new GitExecuter();
      LFSExec = new GitLFSExecutor();
      Context = new GitContext(
        LocalRepoFactory,
        RemoteRepoFactory,
        GitExec,
        LFSExec);
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
      services.AddSingleton(Configuration);
      services.AddSingleton(RemoteRepoFactory);
      services.AddSingleton(LocalRepoFactory);
      services.AddSingleton(Context);
    } /* End of Function - ConfigureServices */

    public class RemoteFactory : IRemoteRepositoryFactory
    {
      public IRemoteRepository Build(string server, string owner, string name, string auth)
      {
        return new RemoteRepository(server, owner, name, AuthInfo.ParseAuth(auth));
      }
    }
    public class LocalFactory : ILocalRepositoryFactory
    {
      public ILocalRepository Build(IRemoteRepository repo, IConfiguration config)
      {
        return new LocalRepository(repo, config);
      }
    }
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