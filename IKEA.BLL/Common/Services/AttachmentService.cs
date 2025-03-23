using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.BLL.Common.Services
{
    public class AttachmentService : IAttachmentService
    {
        private readonly List<string> AllowedExtensions = new() { ".jpg", ".jpeg", ".png" };
        private const int FileMixSize = 2_097_152; //2mbs;
        public string UploadFile(IFormFile file, string FolderName)
        {
            #region Validations
            var fileExtension = Path.GetExtension(file.FileName);  //.FileName => Get Extension And Name 
            if(!AllowedExtensions.Contains(fileExtension))
            {
                throw new Exception("Invalid File Extension Plase Try Again");
            }
            if (file.Length > FileMixSize)
            {
                throw new Exception("Invalid File Size");
            }
            #endregion
            //1-Get Located Floder Path
            var FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", FolderName);
            if(!Directory.Exists(FolderPath) )
            {
                Directory.CreateDirectory(FolderPath);
            }

            //2- Get File Name And Make It Unique
            var FileName = $"{Guid.NewGuid()}{fileExtension}"; //fileExtension ==> file Extension and name
            
            //3- Grt File Path
            var FilePath = Path.Combine(FolderPath, FileName);

            //4- Save File Use Streaming [Data Per Time]
            using var FileStream = new FileStream(FilePath, FileMode.Create);
            file.CopyTo(FileStream);

            //5- Return File
            return FileName;
        }
        public bool Delete(string filePath)
        {
            if(File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }
            return false;
        }

    }
}
