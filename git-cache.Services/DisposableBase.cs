/******************************************************************************
 * File...: DisposableBase.cs
 * Remarks: 
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace git_cache.Services
{
  /************************** DisposableBase *********************************/
  /// <summary>
  /// Base abstract class for <see cref="IDisposable"/> implementation
  /// </summary>
  public abstract class DisposableBase : IDisposable
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /*----------------------- ~DisposableBase -------------------------------*/
    /// <summary>
    /// Implicitly disposes of resources
    /// </summary>
    ~DisposableBase()
    {
      Dispose(false);
    } /* End of Function - ~DisposableBase */
    /************************ Methods ****************************************/
    /*----------------------- Dispose ---------------------------------------*/
    /// <summary>
    /// Explicitly disposes of resources
    /// </summary>
    public void Dispose()
    {
      Dispose(true); GC.SuppressFinalize(this);
    } /* End of Function - Dispose */
    /************************ Fields *****************************************/
    /************************ Static *****************************************/

    /*======================= PROTECTED =====================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    protected abstract void Dispose(bool disposing);
    /************************ Fields *****************************************/
    /************************ Static *****************************************/

  } /* End of Class - DisposableBase */
}
/* End of document - DisposableBase.cs */