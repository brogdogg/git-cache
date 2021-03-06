﻿/******************************************************************************
 * File...: GitServiceResultResult.cs
 * Remarks: 
 */
using git_cache.Services.Shell;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace git_cache.Services.Git.Results
{
  /************************** ServiceResult **********************************/
  /// <summary>
  /// Concrete implementation of the <see cref="IServiceResult"/>
  /// </summary>
  public class ServiceResult : ActionResult, IServiceResult
  {
    /*======================= PUBLIC ========================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Service ****************************************/
    /// <summary>
    /// Gets the service associated with the result
    /// </summary>
    public string Service { get; } /* End of Property - Service */
    /************************ UseGZip ****************************************/
    /// <summary>
    /// Gets a boolean flag indicating if the result should be gzipped
    /// </summary>
    public bool UseGZip { get; } /* End of Property - UseGZip */
    /************************ Repository *************************************/
    /// <summary>
    /// Gets the repository associated with the result
    /// </summary>
    public ILocalRepository Repository { get; } /* End of Property - Repository */
    /************************ Construction ***********************************/
    /*----------------------- GitServiceResultResult ------------------------*/
    /// <summary>
    /// Constructor for the result
    /// </summary>
    /// <param name="service">
    /// Service for the result
    /// </param>
    /// <param name="repo">
    /// Repository associated with the result
    /// </param>
    /// <param name="useGzip">
    /// True if the result should be compressed, false otherwise
    /// </param>
    public ServiceResult(string service, ILocalRepository repo, IShell shell, bool useGzip = false)
    {
      Service = service;
      UseGZip = useGzip;
      if (null == (Repository = repo))
        throw new ArgumentNullException(
          nameof(repo),
          "Local repository is invalid, value must not be null");
      if (null == (Shell = shell))
        throw new ArgumentNullException(
          nameof(shell),
          "Shell object is invalid, value must not be null");
    } /* End of Function - GitServiceResultResult */
    /************************ Methods ****************************************/
    /*----------------------- ExecuteResult ---------------------------------*/
    /// <summary>
    /// Executes the logic to actually return the result
    /// </summary>
    /// <param name="context">
    /// Context for the action
    /// </param>
    public override void ExecuteResult(ActionContext context)
    {
      var response = context.HttpContext.Response;
      var request = context.HttpContext.Request;
      response.StatusCode = 200;
      response.ContentType = $"application/x-{Service}-result";
      response.Headers.Add("Cache-Control", "no-cache");
      Stream stream = request.Body;
      Stream gzipStream = new GZipStream(response.Body, CompressionLevel.Optimal, true);
      if (UseGZip)
        stream = new GZipStream(request.Body, CompressionMode.Decompress, true);
#warning Refactor input writer lambda
      Shell.Execute($"{Service} --stateless-rpc \"{Repository.Path}\"",
        (code) => code != 0, response.Body, (writer) =>
      {
        FileStream fs = new FileStream("/tmp/test", FileMode.Create, FileAccess.Write);
        using (var fsw = new StreamWriter(fs, Encoding.ASCII))
        {
          using (var inputWriter = new StreamWriter(writer.BaseStream, Encoding.ASCII, 8096, true))
          {
            using (var inputReader = new StreamReader(stream, Encoding.UTF8, true, 8096, true))
            {
              string line = null;
              while (null != (line = inputReader.ReadLine()))
              {
                inputWriter.WriteLine(line);
                fsw.WriteLine(line);
              } // end of while - still have lines to read
            } // end of using - stream reader for the source stream
          } // end of using - stream writer for the input stream
        } // end of using - file stream writer for debuggin
      } // end of lambda
      );
      if (gzipStream != null) gzipStream.Dispose();
    } /* End of Function - ExecuteResult */
    /************************ Fields *****************************************/
    /************************ Static *****************************************/

    /*======================= PROTECTED =====================================*/
    /************************ Events *****************************************/
    /************************ Properties *************************************/
    /************************ Shell ******************************************/
    /// <summary>
    /// Gets the shell object
    /// </summary>
    protected IShell Shell { get; } /* End of Property - Shell */
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
    /************************ Static *****************************************/

  } /* End of Class - ServiceResult */
}
/* End of document - ServiceResult.cs */