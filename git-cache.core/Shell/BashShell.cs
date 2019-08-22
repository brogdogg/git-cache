/******************************************************************************
 * File...: BashShell.cs
 * Remarks: 
 */
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
    /************************ Methods ****************************************/
    /*----------------------- Execute ---------------------------------------*/
    /// <summary>
    /// 
    /// </summary>
    /// <param name="command"></param>
    public string Execute(string command)
    {
      return Execute(command, (exitCode) => exitCode != 0);
    } /* End of Function - Execute */

    /*----------------------- Execute ---------------------------------------*/
    /// <summary>
    /// 
    /// </summary>
    /// <param name="command"></param>
    /// <param name="isExitCodeFailure"></param>
    public string Execute(string command, Func<int, bool> isExitCodeFailure)
    {
      string retval = "";
      using (var stream = new MemoryStream())
      {
        Execute(command, isExitCodeFailure, stream);
        stream.Position = 0;
        StreamReader sr = new StreamReader(stream);
        retval = sr.ReadToEnd();
      }
      return retval;
    } /* End of Function - Execute */

    /*----------------------- Execute ---------------------------------------*/
    /// <summary>
    /// 
    /// </summary>
    /// <param name="command"></param>
    /// <param name="isExitCodeFailure"></param>
    /// <param name="outStream"></param>
    /// <param name="inputWriter"></param>
    public int Execute(string command, Func<int, bool> isExitCodeFailure, Stream outStream, Action<StreamWriter> inputWriter = null)
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
        inputWriter(process.StandardInput);
        process.StandardInput.Flush();
        process.StandardInput.Close();
      } // end of if - input writer action provided

      // Wait for the process to exit
      process.WaitForExit();
      // Then wait for the standard output to dry up
      stdoutReader.Wait();

      // Then check for errors
      if(null != isExitCodeFailure && isExitCodeFailure(process.ExitCode))
      {
        throw new InvalidProgramException($"Failed to execute command; {process.StandardError.ReadToEnd()}");
      } // end of if - process exited with error
      return retval;
    } /* End of Function - Execute */

    /*----------------------- ExecuteAsync ----------------------------------*/
    /// <summary>
    /// 
    /// </summary>
    /// <param name="command"></param>
    public Task<string> ExecuteAsync(string command)
    {
      return Task.Run(() => Execute(command));
    } /* End of Function - ExecuteAsync */

    /*----------------------- ExecuteAsync ----------------------------------*/
    /// <summary>
    /// 
    /// </summary>
    /// <param name="command"></param>
    /// <param name="isExitCodeFailure"></param>
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
    /************************ Static *****************************************/
  } /* End of Class - BashShell */
}
/* End of document - BashShell.cs */