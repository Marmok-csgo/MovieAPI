using FileUpload.Helper;

namespace FileUpload.Services;

public class ManageImage : IManageImage
{
    public async Task<string> UploadFile(IFormFile iFormFile)
    {
        string fileName = "";

        try
        {
            var fileInfo = new FileInfo(iFormFile.FileName);

            fileName = iFormFile.FileName + "_" + DateTime.Now.Ticks.ToString() + fileInfo.Extension;
            var getFilePath = Common.GetFilePath(fileName);

            using (var fileStream = new FileStream(getFilePath, FileMode.Create))
            {
                await iFormFile.CopyToAsync(fileStream);
            }

            return fileName;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}