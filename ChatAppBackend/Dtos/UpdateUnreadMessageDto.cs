namespace ChatAppBackend.Dtos
{
    public sealed record UpdateUnreadMessageDto(
        Guid chatId,
        int count);
}
