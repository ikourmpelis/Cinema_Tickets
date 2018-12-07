using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
using System.IO;

namespace FileUploadControl
{
  public  class uploadfilerepo : UploadInterface
    {
        private IHostingEnvironment hostingEnvironment;
        public uploadfilerepo(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;



        }
        public async void uploadfilemultiple(IList<IFormFile> files)
        {
            long totalBytes = files.Sum(f => f.Length);
            foreach (IFormFile item in files)
            {
                string filename = item.FileName.Trim('"');
                byte[] buffer = new byte[16 * 1024];
                using (FileStream output = System.IO.File.Create(this.GetpathAndFileName(filename)))
                {
                    using (Stream input = item.OpenReadStream())
                    {
                        
                        int readBytes;
                        while ((readBytes = input.Read(buffer, 0, buffer.Length))>0)
                        {
                            await output.WriteAsync(buffer, 0, readBytes);
                            totalBytes += readBytes;
                        }


                    }

                }

            }
          
        }

        private string GetpathAndFileName(string filename)
        {
            String path = this.hostingEnvironment.WebRootPath + "\\uploads\\";
            if (!Directory.Exists(path))
            
                Directory.CreateDirectory(path);
                return path + filename;
            
        }

       
    }
}
