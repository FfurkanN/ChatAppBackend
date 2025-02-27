namespace ChatAppBackend.Dtos
{
    public sealed record UserDto(
        Guid Id,
        string UserName,
        string fullname,
        bool isOnline,
        string ProfileImageUrl);
}
