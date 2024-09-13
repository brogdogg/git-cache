/******************************************************************************
 * File...: StreamReader.cs
 * Remarks: 
 */
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace git_cache.Services.IO
{

  /************************** DataReceivedEventArgs **************************/
  /// <summary>
  /// Event arguments for the Data Received event
  /// </summary>
  public class DataReceivedEventArgs : EventArgs
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /// <summary>
    /// Buffer containing the received data
    /// </summary>
    public byte[] Buffer { get; }
    /// <summary>
    /// The total number of bytes available in the buffer
    /// </summary>
    public int ByteCount { get; }
    /************************ Construction ***********************************/
    /*----------------------- DataReceivedEventArgs -------------------------*/
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="buffer">
    /// Buffer containing data
    /// </param>
    /// <param name="byteCount">
    /// Number of bytes to read from the buffer
    /// </param>
    public DataReceivedEventArgs(byte[] buffer, int byteCount)
    {
      Buffer = buffer;
      ByteCount = byteCount;
    } // end of function - DataReceivedEventArgs
    /************************ Methods ****************************************/
    /************************ Fields *****************************************/
    /************************ Static *****************************************/
  } // end of class - DataReceivedEventArgs

  /************************** StringStreamReader *****************************/
  /// <summary>
  /// Asynchronous stream reader, providing a custom event when data
  /// is available.
  /// </summary>
  public class StringStreamReader : IDisposable
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /// <summary>
    /// Event for when data is received
    /// </summary>
    public event EventHandler<DataReceivedEventArgs> DataReceived;
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /*----------------------- StringStreamReader ----------------------------*/
    /// <summary>
    /// Private constructor
    /// </summary>
    /// <param name="stream">
    /// Source stream to read from
    /// </param>
    /// <param name="closeStream">
    /// Close the base stream on disposal?
    /// </param>
    /// <param name="handler">
    /// </param>
    private StringStreamReader(
      Stream stream,
      EventHandler<DataReceivedEventArgs> handler,
      bool closeStream)
    {
      if (null == (BaseStream = stream))
        throw new ArgumentNullException(nameof(stream), "Invalid stream specified, must be non-null");
      BufferSize = 8096 * 4; // 32k
      DataReceived = handler;
      CancelTokenSource = new CancellationTokenSource();
      CloseStream = closeStream;
      // Make sure to call this last, since it uses other
      // class members
      DataReader = ReadDataAsync();
    } // end of function - StringStreamReader

    /*----------------------- ~StringStreamReader ---------------------------*/
    /// <summary>
    /// Destructor
    /// </summary>
    ~StringStreamReader()
    {
      Dispose(false);
    } // end of function - ~StringStreamReader
    /************************ Methods ****************************************/
    /// <summary>
    /// Cancels the current read operations
    /// </summary>
    public void CancelReadOperations()
    {
      CancelTokenSource.Cancel();
    } // end of function - CancelReadOperations

    /// <summary>
    /// Disposes of resources associated with this object
    /// </summary>
    public void Dispose()
    {
      Dispose(true); GC.SuppressFinalize(this);
    } // end of function - Dispose
    /// <summary>
    /// Waits indefinitely for the data reader task to exit.
    /// </summary>
    public void Wait()
    {
      DataReader.Wait(CancelTokenSource.Token);
    } // end of function - Wait

    /// <summary>
    /// Waits for the data reader to exit for up to the specified number of
    /// milliseconds
    /// </summary>
    /// <param name="milliseconds">
    /// Number of milliseconds to wait for it to exit
    /// </param>
    public void Wait(int milliseconds)
    {
      DataReader.Wait(milliseconds, CancelTokenSource.Token);
    } // end of function - Wait
    /************************ Fields *****************************************/
    /************************ Static *****************************************/

    /*----------------------- StartReader -----------------------------------*/
    /// <summary>
    /// Starts a string stream reader for the specified stream, using the
    /// specified encoding for text reads.
    /// </summary>
    /// <param name="stream">
    /// Stream to read from
    /// </param>
    /// <param name="handler">
    /// Handler for when data is received
    /// </param>
    /// <param name="closeStream">
    /// Close the base stream when disposed?
    /// </param>
    /// <returns>
    /// StringStreamReader instance configured for the specified stream
    /// </returns>
    public static StringStreamReader StartReader(
      Stream stream,
      EventHandler<DataReceivedEventArgs> handler,
      bool closeStream = true)
    {
      return new StringStreamReader(stream, handler, closeStream);
    } // end of function - StartReader

    /*======================= PROTECTED =====================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /************************ Methods ****************************************/
    /// <summary>
    /// Explicit dispose method
    /// </summary>
    /// <param name="disposing">
    /// Are we explicitly disposing, or called from the destructor
    /// </param>
    protected virtual void Dispose(bool disposing)
    {
      if (!CancelTokenSource.IsCancellationRequested)
      {
        CancelTokenSource.Cancel();
      } // end of if - cancel has not already be requested
      try
      {
#warning Should revisit this wait on the data reader
        DataReader.Wait(4000);
      }
      catch (Exception)
      {
        // Nothing to do with this exception at this point
      }

      if (disposing)
      {
        if (CloseStream && null != BaseStream)
        {
          BaseStream.Close();
          BaseStream.Dispose();
        } // end of if - we should close the base stream
        CancelTokenSource.Dispose();
      } // end of if - explicitly disposing
      BaseStream = null;
      CancelTokenSource = null;
    } // end of function - Dispose
    /************************ Fields *****************************************/
    /************************ Static *****************************************/

    /*======================= PRIVATE =======================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /// <summary>
    /// Gets the default buffer size to use for buffers
    /// </summary>
    private int BufferSize { get; }

    /// <summary>
    /// Reference to the Task used to read data from the base stream
    /// and fire events to inform of data
    /// </summary>
    private Task<int> DataReader { get; }

    /// <summary>
    /// Base stream to read from
    /// </summary>
    private Stream BaseStream { get; set; }

    /// <summary>
    /// If set to true, then the base stream will be closed on disposal
    /// </summary>
    private bool CloseStream { get; }

    /// <summary>
    /// Cancellation token source for our async thread
    /// </summary>
    private CancellationTokenSource CancelTokenSource { get; set; }

    /************************ Construction ***********************************/
    /************************ Methods ****************************************/

    /// <summary>
    /// Asynchronously reads data from the stream firing the events
    /// when data is received
    /// </summary>
    /// <returns>
    /// Total number of bytes that were read from the entire 
    /// read sequence
    /// </returns>
    private async Task<int> ReadDataAsync()
    {
      int retval = -1;
      byte[] buffer = new byte[BufferSize];
      int bytesRead = 0;
      try
      {
        // While we are still able to read data from the stream,
        // do so and fire events informing listeners we received
        // data.
        while ((bytesRead = await BaseStream.ReadAsync(buffer,
                                                       0,
                                                       buffer.Length,
                                                       CancelTokenSource.Token)) > 0)
        {
          retval += bytesRead;
          OnDataReceived(buffer, bytesRead);
        } // end of while - stream is still available and reading bytes
      } // end of try - to read bytes from stream
      catch (Exception exc)
      {
        Console.WriteLine($"Exception in reading data: ${exc}");
        if (exc is TaskCanceledException || exc is OperationCanceledException)
        {
          Debug.WriteLine("Cancel exception encountered, will ignore");
        } // end of if - cancel exception
        else // we will rethrow to inform of the exception
        {
          throw;
        } // end of else - rethrow
      } // end of catch - cancel exception
      return retval;
    } // end of function - ReadDataAsync

    /// <summary>
    /// On data received
    /// </summary>
    /// <param name="buffer">
    /// Buffer containing data
    /// </param>
    /// <param name="byteCount">
    /// Number of bytes in the buffer to be read
    /// </param>
    private void OnDataReceived(byte[] buffer, int byteCount)
    {
      DataReceived?.Invoke(this, new DataReceivedEventArgs(buffer, byteCount));
    } // end of function - OnDataReceived
    /************************ Fields *****************************************/
    /************************ Static *****************************************/

  } // end of class - StringStreamReader

} // end of namespace - git_cache.IO
/* End of document - StreamReader.cs */