/******************************************************************************
 * File...: BaseExtensionUnitTest.cs
 * Remarks: 
 */
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace git_cache.Services.mstest.Extensions.DependencyInjection
{
  /************************** BaseExtensionUnitTest **************************/
  /// <summary>
  /// 
  /// </summary>
  [TestClass]
  public class BaseExtensionUnitTest
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /*----------------------- Initialize ------------------------------------*/
    /// <summary>
    /// Initializes, adding a fake logger for most of the classes
    /// during resolution time.
    /// </summary>
    [TestInitialize]
    public virtual void Initialize()
    {
      Services = new ServiceCollection();
      Services.AddSingleton(typeof(ILogger<>), typeof(FakeLogger<>));
    } /* End of Function - Initialize */
    /************************ Fields *****************************************/
    /************************ Static *****************************************/

    /*======================= PROTECTED =====================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    protected IServiceCollection Services { get; set; }
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /************************ Fields *****************************************/
    /************************ Static *****************************************/
    protected class FakeLogger<TType> : ILogger<TType>
    {
      public IDisposable BeginScope<TState>(TState state)
      {
        throw new NotImplementedException();
      }

      public bool IsEnabled(LogLevel logLevel)
      {
        throw new NotImplementedException();
      }

      public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
      {
        throw new NotImplementedException();
      }
    }

    /*======================= PRIVATE =======================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /************************ Fields *****************************************/
    /************************ Static *****************************************/

  } /* End of Class - BaseExtensionUnitTest */
}
/* End of document - BaseExtensionUnitTest.cs */