/******************************************************************************
 * File...: StreamReaderUnitTest.cs
 * Remarks: 
 */
using Microsoft.VisualStudio.TestTools.UnitTesting;
using git_cache.Services.IO;
using System;
using System.IO;

namespace git_cache.Services.mstest.IO
{

  /************************** DataReceivedEventArgsUnitTest ******************/
  /// <summary>
  /// Verifies the behavior of <see cref="DataReceivedEventArgs"/>
  /// </summary>
  [TestClass]
  public class DataReceivedEventArgsUnitTest
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /*----------------------- DefaultsCorrectly -----------------------------*/
    /// <summary>
    /// Verifies the event args construct and use defaults ok
    /// </summary>
    [TestMethod]
    public void DefaultsCorrectly()
    {
      DataReceivedEventArgs args
        = new DataReceivedEventArgs(new byte[] { 0, 1, 2 }, 3);
      Assert.AreEqual(args.ByteCount, 3);
      Assert.AreEqual(args.Buffer[0], 0);
      Assert.AreEqual(args.Buffer[1], 1);
      Assert.AreEqual(args.Buffer[2], 2);
    } /* End of Function - DefaultsCorrectly */
    /************************ Fields *****************************************/
    /************************ Static *****************************************/
  } /* End of Class - DataReceivedEventArgsUnitTest */

  /************************** StringStreamReaderUnitTest *********************/
  /// <summary>
  /// Verifies the behavior of <see cref="StringStreamReader"/>
  /// </summary>
  [TestClass]
  public class StringStreamReaderUnitTest
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /*----------------------- CanHandleStreamWithoutWrite -------------------*/
    /// <summary>
    /// Verifies the <see cref="StringStreamReader"/> can handle when no data
    /// is written
    /// </summary>
    [TestMethod]
    public void CanHandleStreamWithoutWrite()
    {
      var memStrm = new MemoryStream();
      using (var reader = StringStreamReader.StartReader(memStrm, null))
      {
        // Do nothing at this point, just want it to dispose
      } // end of using - resource
      Assert.IsFalse(memStrm.CanRead);
      Assert.IsFalse(memStrm.CanWrite);
    } /* End of Function - CanHandleStreamWithoutWrite */

    /*----------------------- DoesNotCloseWhenToldNotTo ---------------------*/
    /// <summary>
    /// Verifies the <see cref="StringStreamReader"/> object leaves the memory
    /// stream open when created to leave open
    /// </summary>
    [TestMethod]
    public void DoesNotCloseWhenToldNotTo()
    {
      using (var memStrm = new MemoryStream())
      {
        using (var reader = StringStreamReader.StartReader(memStrm, null, false))
        {
          // Do nothing at this point, just want it to dispose
        } // end of using - resource
        Assert.IsTrue(memStrm.CanRead);
        Assert.IsTrue(memStrm.CanWrite);
      } // end of using - memory stream resource
    } /* End of Function - DoesNotCloseWhenToldNotTo */

    /*----------------------- SendsDataReceivedEvent ------------------------*/
    /// <summary>
    /// Verifies the <see cref="StringStreamReader"/> object fires the event
    /// when data is received
    /// </summary>
    [TestMethod]
    public void SendsDataReceivedEvent()
    {
      bool called = false;
      var memStrm = new MemoryStream();
      memStrm.Write(new byte[] { 0, 1, 2 }, 0, 3);
      memStrm.Flush();
      memStrm.Position = 0;
      using (var reader = StringStreamReader.StartReader(
                            memStrm,
                            (s,e) => { called = true; },
                            false))
      {
        // Close the memory stream to force reader to exit
        memStrm.Close();
      } // end of using - resource
      Assert.IsTrue(called);
    } /* End of Function - SendsDataReceivedEvent */

    /*----------------------- ThrowsWhenStreamIsInvalid ---------------------*/
    /// <summary>
    /// Verifies the <see cref="StringStreamReader"/> contstructor throws
    /// exception when stream is invalid
    /// </summary>
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ThrowsWhenStreamIsInvalid()
    {
      var val = StringStreamReader.StartReader(null, null, false);
    } /* End of Function - ThrowsWhenStreamIsInvalid */
    /************************ Fields *****************************************/
    /************************ Static *****************************************/
  } /* End of Class - StringStreamReaderUnitTest */
} /* End of Namespace - git_cache_mstest */

/* End of document - StreamReaderUnitTest.cs */