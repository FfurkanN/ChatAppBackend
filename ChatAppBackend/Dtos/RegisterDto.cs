namespace ChatAppBackend.Dtos
{
    public sealed record RegisterDto(
        string Email,
        string Username,
        string FirstName,
        string LastName,
        string Password);
}
