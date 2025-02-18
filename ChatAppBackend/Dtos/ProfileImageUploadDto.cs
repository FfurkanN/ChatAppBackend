namespace ChatAppBackend.Dtos
{
    public sealed record ProfileImageUploadDto(
         IFormFile file,
         Guid userId
    );
}
