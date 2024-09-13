/******************************************************************************
 * File...: ReaderWriterLockManagerUnitTest.cs
 * Remarks: 
 */
using git_cache.Services.ResourceLock;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;

namespace git_cache.Services.mstest.ResourceLock
{
  /************************** ReaderWriterLockManagerUnitTest ****************/
  /// <summary>
  /// Tests the <see cref="ReaderWriterLockManager{TKey}"/> class object.
  /// </summary>
  [TestClass]
  public class ReaderWriterLockManagerUnitTest
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /*----------------------- CanDispose ------------------------------------*/
    /// <summary>
    /// Verifies the object can dispose correctly
    /// </summary>
    [TestMethod]
    public void CanDispose()
    {
      // Setup
      // Create a dictionary to use as a pretend populated
      // dictionary
      var pretendDictionary =
        new Dictionary<string, IReaderWriterLock>();
      pretendDictionary.Add("Test", Substitute.For<IReaderWriterLock>());
      var mgr = new ReaderWriterLockManager<string>(m_factory, m_logger);
      // And we will get the private field information to update
      // with our pretend dictionary
      var field = typeof(ReaderWriterLockManager<string>)
                    .GetField("m_readerWriters",
                      System.Reflection.BindingFlags.GetField
                      | System.Reflection.BindingFlags.Instance
                      | System.Reflection.BindingFlags.NonPublic);
      // Set the value
      field.SetValue(mgr, pretendDictionary);
      // And for safe measures, read to make sure it is good
      Assert.IsNotNull(field.GetValue(mgr));
      // Act
      mgr.Dispose();
      // Assert
      // Now we should have a null object instead, due to the
      // disposal
      var dictionary = (Dictionary<string, IReaderWriterLock>)field.GetValue(mgr);
      Assert.AreEqual(null, dictionary);
    } /* End of Function - CanDispose */

    /*----------------------- Initialize ------------------------------------*/
    /// <summary>
    /// Initializes the factory and logger object to be used by
    /// the tests.
    /// </summary>
    [TestInitialize]
    public void Initialize()
    {
      m_factory = Substitute.For<IReaderWriterLockFactory>();
      m_logger = Substitute.For<ILogger<ReaderWriterLockManager<string>>>();
    } /* End of Function - Initialize */

    /*----------------------- GetForNewItem ---------------------------------*/
    /// <summary>
    /// Verifies the behavior for when a new key is presented
    /// </summary>
    [TestMethod]
    public void GetForNewItem()
    {
      // Setup
      var fakeItem = Substitute.For<IReaderWriterLock>();
      m_factory.Create().Returns(fakeItem);
      var mgr = new ReaderWriterLockManager<string>(m_factory, m_logger);
      // Act
      var item = mgr.GetFor("test");
      // Assert
      Assert.AreEqual(item, fakeItem);
      m_factory.Received().Create();
    } /* End of Function - GetForNewItem */

    /*----------------------- GetForOldItem ---------------------------------*/
    /// <summary>
    /// 
    /// </summary>
    [TestMethod]
    public void GetForOldItem()
    {
      // Setup
      var fakeItem = Substitute.For<IReaderWriterLock>();
      Dictionary<string, IReaderWriterLock> dictionary = new Dictionary<string, IReaderWriterLock>();
      dictionary.Add("test", fakeItem);
      var mgr = new ReaderWriterLockManager<string>(m_factory, m_logger);
      // Setup to set the private field
      var field = typeof(ReaderWriterLockManager<string>).GetField("m_readerWriters",
                    System.Reflection.BindingFlags.NonPublic
                    | System.Reflection.BindingFlags.GetField
                    | System.Reflection.BindingFlags.Instance);
      field.SetValue(mgr, dictionary);
      // Act
      var item = mgr.GetFor("test");
      // Assert
      Assert.AreEqual(item, fakeItem);
      m_factory.DidNotReceive().Create();
    } /* End of Function - GetForOldItem */

    /*----------------------- ThrowsForNullFactory --------------------------*/
    /// <summary>
    /// Verifies the constructor throws if the factory is null.
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ThrowsForNullFactory()
    {
      var a = new ReaderWriterLockManager<string>(null, m_logger);
    } /* End of Function - ThrowsForNullFactory */

    /*----------------------- ThrowsForNullLogger ---------------------------*/
    /// <summary>
    /// Verifies the constructor throws if the logger is null
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ThrowsForNullLogger()
    {
      var a = new ReaderWriterLockManager<string>(m_factory, null);

    } /* End of Function - ThrowsForNullLogger */
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
    IReaderWriterLockFactory m_factory = null;
    ILogger<ReaderWriterLockManager<string>> m_logger = null;
    /************************ Static *****************************************/

  } /* End of Class - ReaderWriterLockManagerUnitTest */
}
/* End of document - ReaderWriterLockManagerUnitTest.cs */