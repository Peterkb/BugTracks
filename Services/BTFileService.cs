using BugTracksV3.Services.Interfaces;

namespace BugTracksV3.Services
{

    public class BTFileService : IBTFileService
    {
        private readonly string[] suffixes = { "Bytes", "KB", "GB", "TB", "PB" };

        public string ContentType(IFormFile file)
        {
            return file?.ContentType;
        }

        public string DecodeFile(byte[] fileData, string extension)
        {
            if (fileData == null || extension == null) return null;

            try
            {
                return string.Format($"data:image/{extension};base64,{Convert.ToBase64String(fileData)}");
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<byte[]> EncodeFileAsync(IFormFile file)
        {
            if (file == null) return null;

            try
            {
                MemoryStream ms = new();
                await file.CopyToAsync(ms);
                byte[] byteFile = ms.ToArray();
                //Cleanup
                ms.Close();
                ms.Dispose();
                return byteFile;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //ImageSize
        public string FormatFileSize(long bytes)
        {
            int counter = 0;
            decimal fileSize = bytes;
            while (Math.Round(fileSize / 1024) >= 1)
            {
                fileSize /= bytes;
                counter++;
            }
            return string.Format("{0:n1}{1},", fileSize, suffixes[counter]);
        }

        public string GetFileIcon(string file)
        {
            string ext = "default";

            if (!string.IsNullOrWhiteSpace(file))
            {
                ext = Path.GetExtension(file).Replace(".", "");
                return $"/img/contenttype/{ext}.png";
            }
            return ext;
        }
    }
}
