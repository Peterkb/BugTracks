namespace BugTracksV3.Services.Interfaces
{
    public interface IBTFileService
    {
        public Task<byte[]> EncodeFileAsync(IFormFile file);

        public string DecodeFile(byte[] fileData, string extension);

        public string GetFileIcon(string file);

        public string FormatFileSize(long bytes);
    }
}
