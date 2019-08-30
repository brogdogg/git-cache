/******************************************************************************
 * File...: BashShell.cs
 * Remarks: 
 */
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace git_cache.Services.Shell
{

  /************************** BashShell **************************************/
  /// <summary>
  /// Represents a bash shell instance of <see cref="IShell"/> interface.
  /// </summary>
  public class BashShell : IShell
  {

    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Construction ***********************************/
    /*----------------------- BashShell -------------------------------------*/
    /// <summary>
    /// Injection constructor
    /// </summary>
    /// <param name="logger">
    /// Logging object
    /// </param>
    public BashShell(ILogger<BashShell> logger)
    {
      if(null == (m_logger = logger))
        throw new ArgumentNullException("A valid logger object must be provided");
    } /* End of Function - BashShell */
    /************************ Methods ****************************************/
    /*----------------------- Execute ---------------------------------------*/
    /// <summary>
    /// Executes the command, assuming exit code of zero (0) is success
    /// </summary>
    /// <param name="command">
    /// Command to execute from the bash shell.
    /// </param>
    public string Execute(string command)
    {
      m_logger.LogDebug("Executing command assuming success as (exitCode == 0)");
      return Execute(command, (exitCode) => exitCode != 0);
    } /* End of Function - Execute */

    /*----------------------- Execute ---------------------------------------*/
    /// <summary>
    /// Executes the command, checking for success from the specified
    /// functor.
    /// </summary>
    /// <param name="command">
    /// Command to execute.
    /// </param>
    /// <param name="isExitCodeFailure">
    /// Functor to determine if exit code is a failure or not.
    /// </param>
    public string Execute(string command, Func<int, bool> isExitCodeFailure)
    {
      string retval = "";
      m_logger.LogDebug("Setting up a memory stream to read stdout");
      using (var stream = new MemoryStream())
      {
        Execute(command, isExitCodeFailure, stream);
        stream.Position = 0;
        StreamReader sr = new StreamReader(stream);
        retval = sr.ReadToEnd();
      } // end of using - memory stream
      return retval;
    } /* End of Function - Execute */

    /*----------------------- Execute ---------------------------------------*/
    /// <summary>
    /// Executes the specified command using a functor to decide if the
    /// return code is a failure or not.
    /// </summary>
    /// <param name="command">
    /// Command to execute
    /// </param>
    /// <param name="isExitCodeFailure">
    /// Functor to decide if return code is considered a failure or not
    /// </param>
    /// <param name="outStream">
    /// Stream to recieve the stdout from the process
    /// </param>
    /// <param name="inputWriter">
    /// Optional input writer to provide input to stdin
    /// </param>
    public int Execute(
      string command,
      Func<int, bool> isExitCodeFailure,
      Stream outStream,
      Action<StreamWriter> inputWriter = null)
    {
      int retval = 0;
      var escapedArgs = command.Replace("\"", "\\\"");
      var process = new Process()
      {
        StartInfo = new ProcessStartInfo()
        {
          FileName = "/bin/bash",
          Arguments = $"-c \"{escapedArgs}\"",
          RedirectStandardOutput = true,
          RedirectStandardInput = inputWriter != null,
          UseShellExecute = false,
          RedirectStandardError = true,
          CreateNoWindow = true,
          StandardOutputEncoding = Encoding.ASCII
        }
      };
      m_logger.LogDebug($"Starting process for command: {command}");
      // This is no good, because if there is no newline, then we will not get this
      // event. We need a custom stream reader
      process.Start();
      // Create an async reader on the basestream of the standard output stream
      var stdoutReader = StartNewReader(process.StandardOutput.BaseStream,
                                        outStream);

      // If we were given an standard input writer action,
      // we will setup to redirect the standard input for the process
      if(null != inputWriter)
      {
        m_logger.LogDebug("InputWriter specified, using to work with stdin");
        inputWriter(process.StandardInput);
        process.StandardInput.Flush();
        process.StandardInput.Close();
      } // end of if - input writer action provided

      m_logger.LogDebug("Waiting for process to exit");
      // Wait for the process to exit
      process.WaitForExit();
      // Then wait for the standard output to dry up
      stdoutReader.Wait();
      m_logger.LogDebug($"Process exited with exit code: {process.ExitCode}");

      // Then check for errors
      if(null != isExitCodeFailure && isExitCodeFailure(process.ExitCode))
      {
        string errMsg = $"Failed to execute command; {process.StandardError.ReadToEnd()}";
        m_logger.LogError(errMsg);
        throw new InvalidProgramException(errMsg);
      } // end of if - process exited with error
      return retval;
    } /* End of Function - Execute */

    /*----------------------- ExecuteAsync ----------------------------------*/
    /// <summary>
    /// Executes the command asynchronously.
    /// </summary>
    /// <param name="command">
    /// Command to execute
    /// </param>
    public Task<string> ExecuteAsync(string command)
    {
      return Task.Run(() => Execute(command));
    } /* End of Function - ExecuteAsync */

    /*----------------------- ExecuteAsync ----------------------------------*/
    /// <summary>
    /// Executes the command asynchronously, using a functor to decide if the
    /// return code is a failure or not.
    /// </summary>
    /// <param name="command">
    /// Command to execute.
    /// </param>
    /// <param name="isExitCodeFailure">
    /// Functor to decide if the return code is a failure or not.
    /// </param>
    public Task<string> ExecuteAsync(string command, Func<int, bool> isExitCodeFailure)
    {
      return Task.Run(() => Execute(command, isExitCodeFailure));
    } /* End of Function - ExecuteAsync */
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
    /*----------------------- StartNewReader --------------------------------*/
    /// <summary>
    /// Starts a new asynchronous StreamReader object on the specified stream
    /// which forwards data to the destination stream
    /// </summary>
    /// <param name="stream"></param>
    /// <param name="destinationStream"></param>
    /// <returns></returns>
    private static IO.StringStreamReader StartNewReader(Stream stream, Stream destinationStream)
    {
      return
        IO.StringStreamReader.StartReader(stream,
          (sender, args) =>
          {
            destinationStream.Write(args.Buffer, 0, args.ByteCount);
            destinationStream.Flush();
          },
          false);
    } /* End of Function - StartNewReader */
    /************************ Fields *****************************************/
    /// <summary>
    /// Logger item to use for logging
    /// </summary>
    private ILogger<BashShell> m_logger = null;
    /************************ Static *****************************************/
  } /* End of Class - BashShell */
}
/* End of document - BashShell.cs */