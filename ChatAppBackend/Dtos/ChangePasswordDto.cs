namespace ChatAppBackend.Dtos
{
    public sealed record ChangePasswordDto(
        Guid id,
        string CurrentPassword,
        string NewPassword);
}
