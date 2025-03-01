namespace ChatAppBackend.Dtos
{
    public sealed record UserDto(
        Guid Id,
        string UserName,
        string FirstName,
        string LastName,
        string About,
        string Email,
        bool isOnline,
        string ProfileImageUrl);
}
