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
      process.OutputDataReceived += (o, args) =>
      {
        if (null != args && null != args.Data)
        {
          StreamWriter sw = new StreamWriter(outStream, System.Text.Encoding.ASCII, 8096, true);
          retval += System.Text.Encoding.ASCII.GetByteCount(args.Data);
          sw.WriteLine(args.Data);
          sw.Flush();
        }
      };
      process.Start();
      process.BeginOutputReadLine();
      if(null != inputWriter)
      {
        inputWriter(process.StandardInput);
        process.StandardInput.Flush();
        process.StandardInput.Close();
      }
      process.WaitForExit();
      process.CancelOutputRead();
      if(null != isExitCodeFailure && isExitCodeFailure(process.ExitCode))
      {
        throw new InvalidProgramException($"Failed to execute command; {process.StandardError.ReadToEnd()}");
      }
      else if (!process.StandardOutput.EndOfStream)
      {
        char[] buffer = new char[8096];
        int bytesREad = 0;
        StreamWriter sw = new StreamWriter(outStream, System.Text.Encoding.ASCII, 8096, true);
        while((bytesREad = process.StandardOutput.Read(buffer, 0, buffer.Length)) > 0)
        {
          sw.Write(buffer, 0, bytesREad);
          retval += bytesREad;
        }
      }
      return retval;
    }
    public static Task<string> BashAsync(this string command, Func<int, bool> isExitCodeFailure)
    {
      return Task.Run(() => Bash(command, isExitCodeFailure));
    }

    public static Task<string> BashAsync(this string command)
    {
      return Task.Run(() => Bash(command));
    }

  } // end of class - ShellHelper
}
