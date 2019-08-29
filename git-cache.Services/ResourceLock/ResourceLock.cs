/******************************************************************************
 * File...: ResourceLock.cs
 * Remarks: 
 */
using System;
using System.Threading;

namespace git_cache.Services.ResourceLock
{
  /************************** ResourceLock ***********************************/
  /// <summary>
  /// Basic implementation of <see cref="IResourceLock"/>, which utilizes
  /// a <see cref="ManualResetEvent"/> as the underlying locking mechanism
  /// </summary>
  public class ResourceLock : DisposableBase, IResourceLock
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /*----------------------- ResourceLock ----------------------------------*/
    /// <summary>
    /// Constructor
    /// </summary>
    public ResourceLock()
      : this(new AutoResetEvent(true))
    {
      return;
    } /* End of Function - ResourceLock */

    /*----------------------- ResourceLock ----------------------------------*/
    /// <summary>
    /// Injection Constructor
    /// </summary>
    /// <param name="resetEvent">
    /// The object to operate with, must not be null
    /// </param>
    public ResourceLock(AutoResetEvent obj)
    {
      if (null == (m_obj = obj))
        throw new ArgumentNullException("A valid mutex must be used");
    } /* End of Function - ResourceLock */

    /************************ Methods ****************************************/
    /*----------------------- Release ---------------------------------------*/
    /// <summary>
    /// Releases the lock
    /// </summary>
    public void Release()
    {
      m_obj.Set();
    } /* End of Function - Release */

    /*----------------------- WaitOne ---------------------------------------*/
    /// <summary>
    /// Waits indefinitely for the lock
    /// </summary>
    public bool WaitOne()
    {
      return m_obj.WaitOne();
    } /* End of Function - WaitOne */

    /*----------------------- WaitOne ---------------------------------------*/
    /// <summary>
    /// Waits for the specified amount of time to obtain the lock
    /// </summary>
    /// <param name="millisecondsTimeout"></param>
    public bool WaitOne(int millisecondsTimeout)
    {
      return m_obj.WaitOne(millisecondsTimeout);
    } /* End of Function - WaitOne */

    /*----------------------- WaitOne ---------------------------------------*/
    /// <summary>
    /// Waits for the specified amount of time span to obtain the lock
    /// </summary>
    /// <param name="timeout"></param>
    public bool WaitOne(TimeSpan timeout)
    {
      return m_obj.WaitOne(timeout);
    } /* End of Function - WaitOne */
    /************************ Fields *****************************************/
    /************************ Static *****************************************/

    /*======================= PROTECTED =====================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /*----------------------- Dispose ---------------------------------------*/
    /// <summary>
    /// 
    /// </summary>
    /// <param name="disposing"></param>
    protected override void Dispose(bool disposing)
    {
      if(null != m_obj)
      {
        if(disposing)
          m_obj.Dispose();
        m_obj = null;
      } // end of if - valid reset event
      return;
    } /* End of Function - Dispose */
    /************************ Fields *****************************************/
    /************************ Static *****************************************/

    /*======================= PRIVATE =======================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /************************ Fields *****************************************/
    AutoResetEvent m_obj = null;
    /************************ Static *****************************************/

  } /* End of Class - ResourceLock */
}
/* End of document - ResourceLock.cs */