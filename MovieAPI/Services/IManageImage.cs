namespace FileUpload.Services;

public interface IManageImage
{
    Task<string> UploadFile(IFormFile iFormFile);

}