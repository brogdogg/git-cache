using git_cache.Git;
using git_cache.Shell;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace git_cache.Results
{
  public class GitServiceResultResult : ActionResult
  {
    public string Service { get; }
    public bool UseGZip { get; }
    public LocalRepo Repository { get; }
    public GitServiceResultResult(string service, LocalRepo repo, bool useGzip = false)
    {
      Service = service;
      UseGZip = useGzip;
      Repository = repo;
    }
    public override void ExecuteResult(ActionContext context)
    {
      var response = context.HttpContext.Response;
      var request = context.HttpContext.Request;
      response.StatusCode = 200;
      response.ContentType = $"application/x-{Service}-result";
      response.Headers.Add("Cache-Control", "no-cache");
      Stream stream = request.Body;
      Stream gzipStream = new GZipStream(response.Body, CompressionLevel.Optimal, true);
      //response.Headers.Add("Content-Encoding", "gzip");
      if (UseGZip)
      {
        stream = new GZipStream(request.Body, CompressionMode.Decompress, true);
      }
      $"{Service} --stateless-rpc \"{Repository.Path}\"".Bash((code) => code != 0, response.Body, (writer) =>
      {
        FileStream fs = new FileStream("/tmp/test", FileMode.Create, FileAccess.Write);
        using (var fsw = new StreamWriter(fs, Encoding.ASCII))
        {
          char[] buffer = new char[8096];
          var bodyStream = context.HttpContext.Request.Body;
          using (var inputWriter = new StreamWriter(writer.BaseStream, Encoding.ASCII, 8096, true))
          {
            using (var inputReader = new StreamReader(stream, Encoding.UTF8, true, 8096, true))
            {
              string line = null;
              while(null != (line = inputReader.ReadLine()))
              {
                inputWriter.WriteLine(line);
                fsw.WriteLine(line);
              }
              /*
              int bytesRead = 0;
              while ((bytesRead = inputReader.Read(buffer, 0, buffer.Length)) > 0)
              {
                inputWriter.Write(buffer, 0, bytesRead);
                fsw.Write(buffer, 0, bytesRead);
              } // end of while - still able to read bytes from the input stream
              */
            } // end of using - stream reader for the source stream
          } // end of using - stream writer for the input stream
        } // end of using - file stream writer for debuggin
      } // end of lambda
      );
      if (gzipStream != null) gzipStream.Dispose();
      return;
    }
  }
}
