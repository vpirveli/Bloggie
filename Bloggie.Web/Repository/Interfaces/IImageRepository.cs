namespace Bloggie.Web.Repository.Interfaces
{
    public interface IImageRepository
    {
        Task<string> UploadAsync(IFormFile file);
    }
}
