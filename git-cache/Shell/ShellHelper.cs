using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
      var retval = "";
      var escapedArgs = command.Replace("\"", "\\\"");
      var process = new Process()
      {
        StartInfo = new ProcessStartInfo()
        {
          FileName = "/bin/bash",
          Arguments = $"-c \"{escapedArgs}\"",
          RedirectStandardOutput = true,
          UseShellExecute = false,
          RedirectStandardError = true,
          CreateNoWindow = true
        }
      };
      process.Start();
      retval = process.StandardOutput.ReadToEnd();
      process.WaitForExit();
      if (null != isExitCodeFailure && isExitCodeFailure(process.ExitCode)) {
        throw new InvalidProgramException($"failed to execute the command successfully; {process.StandardError.ReadToEnd()}");
      }
      return retval;
    } // end of function - Bash

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
