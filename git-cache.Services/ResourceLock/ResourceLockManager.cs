/******************************************************************************
 * File...: ResourceLockManager.cs
 * Remarks: 
 */
using System;
using System.Collections.Generic;

namespace git_cache.Services.ResourceLock
{
  /************************** ResourceLockManager ****************************/
  /// <summary>
  /// Basic implementation of the <see cref="IResourceLockManager{TKey}"/>
  /// interface.
  /// </summary>
  public class ResourceLockManager<TKey> : DisposableBase, IResourceLockManager<TKey>
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /*----------------------- ResourceLockManager ---------------------------*/
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="factory"></param>
    public ResourceLockManager(IResourceLockFactory factory)
    {
      if (null == (m_factory = factory))
        throw new ArgumentNullException("Must provide a valid factory");
      return;
    } /* End of Function - ResourceLockManager */
    /************************ Methods ****************************************/

    /*----------------------- BlockFor --------------------------------------*/
    /// <summary>
    /// Blocks indefinitely for the resource lock associated with the key
    /// </summary>
    /// <param name="key">
    /// Key associated with the resource lock
    /// </param>
    public IResourceLock BlockFor(TKey key)
    {
      // Wait indefinitely
      return BlockFor(key, TimeSpan.FromMilliseconds(-1));
    } /* End of Function - BlockFor */

    /*----------------------- BlockFor --------------------------------------*/
    /// <summary>
    /// Blocks until able to lock resource for the specified key or until the
    /// timeout has been reached
    /// </summary>
    /// <param name="key">
    /// Key associated with the resource lock
    /// </param>
    /// <param name="timeout">
    /// Timeout for waiting
    /// </param>
    public IResourceLock BlockFor(TKey key, TimeSpan timeout)
    {
      var retval = GetFor(key);
      if(!retval.WaitOne(timeout))
        throw new TimeoutException($"Failed to obtain lock within timespan: {timeout}");
      return retval;
    } /* End of Function - BlockFor */

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
      lock(m_lock)
      {
        if (null != m_resourceLocks)
        {
          if (disposing)
          {
            foreach (var item in m_resourceLocks)
              item.Value.Dispose();
          } // end of lock - resources
          m_resourceLocks.Clear();
          m_resourceLocks = null;
        } // end of if - valid resource dictionary
      } // end of disposing

      return;
    } /* End of Function - Dispose */

    /*----------------------- GetFor ----------------------------------------*/
    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    public virtual IResourceLock GetFor(TKey key)
    {
      IResourceLock retval = null;
      lock(m_lock)
      {
        if (m_resourceLocks.ContainsKey(key))
          retval = m_resourceLocks[key];
        else
        {
          retval = m_factory.Create();
          m_resourceLocks.Add(key, retval);
        }
      } // end of lock - on resources
      return retval;
    } /* End of Function - GetFor */
    /************************ Fields *****************************************/
    /************************ Static *****************************************/

    /*======================= PRIVATE =======================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /************************ Fields *****************************************/
    readonly object m_lock = new object();
    Dictionary<TKey, IResourceLock> m_resourceLocks
      = new Dictionary<TKey, IResourceLock>();
    IResourceLockFactory m_factory = null;
    /************************ Static *****************************************/
  } /* End of Class - ResourceLockManager */
}
/* End of document - ResourceLockManager.cs */