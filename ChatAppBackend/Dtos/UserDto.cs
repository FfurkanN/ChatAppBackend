namespace ChatAppBackend.Dtos
{
    public sealed record UserDto(
        Guid Id,
        string UserName,
        bool isOnline,
        string ProfileImageUrl);
}
