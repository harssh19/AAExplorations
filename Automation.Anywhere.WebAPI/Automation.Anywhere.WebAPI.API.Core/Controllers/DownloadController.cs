using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Automation.Anywhere.WebAPI.Core.Controllers
{
    public class DownloadController : ApiController
    {
        public async Task Get(long id)
        {
            ////string fullFilePath = GetFilePathById(id);
            var fullFilePath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/download/EditPlus64_xp85.com.zip");
            string returnFileName = fullFilePath.Split('\\').Last();
            FileInfo path = new FileInfo(fullFilePath);
            HttpContext.Current.Response.ContentType = "application/zip";
            HttpContext.Current.Response.AddHeader("Content-Length", path.Length.ToString());
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + returnFileName);
            HttpContext.Current.Response.Flush();
            try
            {
                byte[] buffer = new byte[4096];
                using (FileStream fs = path.OpenRead())
                {
                    using (BufferedStream bs = new BufferedStream(fs, 524288))
                    {
                        int count = 0;
                        while ((count = bs.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            if (!HttpContext.Current.Response.IsClientConnected)
                            {
                                break;
                            }
                            HttpContext.Current.Response.OutputStream.Write(buffer, 0, count);
                            HttpContext.Current.Response.Flush();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                //Exception logging here
            }
        }
    }
}
