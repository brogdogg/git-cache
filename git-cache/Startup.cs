﻿/******************************************************************************
 * File...: Startup.cs
 * Remarks: 
 */
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
      services.AddSingleton<IConfiguration>(Configuration);
    } /* End of Function - ConfigureServices */

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