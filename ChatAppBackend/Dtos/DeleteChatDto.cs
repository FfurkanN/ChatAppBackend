namespace ChatAppBackend.Dtos
{
    public sealed record DeleteChatDto(
        Guid userId,
        Guid chatId);
}
