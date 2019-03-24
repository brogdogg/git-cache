using git_cache.IO;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace git_cache.Shell
{
  public static class ShellHelper
  {

    public static string Bash(this string command)
    {
      return Bash(command, (exitCode) => exitCode != 0);
    }
    public static string Bash(this string command, Func<int, bool> isExitCodeFailure)
    {
      string retval = "";
      using (var stream = new MemoryStream())
      {
        Bash(command, isExitCodeFailure, stream);
        stream.Position = 0;
        StreamReader sr = new StreamReader(stream);
        retval = sr.ReadToEnd();
      }
      return retval;
    } // end of function - Bash

    public static int Bash(this string command, Func<int, bool> isExitCodeFailure, Stream outStream, Action<StreamWriter> inputWriter=null)
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
    } // end of function - Bash

    /// <summary>
    /// Executes the bash command asynchronously.
    /// </summary>
    /// <param name="command">
    /// Bash command to execute
    /// </param>
    /// <param name="isExitCodeFailure">
    /// Functor to decide if the exit code is a failure or not
    /// </param>
    /// <returns>
    /// Task for the execution
    /// </returns>
    public static Task<string> BashAsync(this string command, Func<int, bool> isExitCodeFailure)
    {
      return Task.Run(() => Bash(command, isExitCodeFailure));
    } // end of function - BashAsync

    /// <summary>
    /// Executes the bash command asynchronously
    /// </summary>
    /// <param name="command">
    /// Command to execute
    /// </param>
    /// <remarks>
    /// Assumes any exit code that is not zero is a failure
    /// </remarks>
    /// <returns>
    /// Task for the execution
    /// </returns>
    public static Task<string> BashAsync(this string command)
    {
      return Task.Run(() => Bash(command));
    } // end of function - BashAsync

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
    }

  } // end of class - ShellHelper
}
