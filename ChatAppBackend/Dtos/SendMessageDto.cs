namespace ChatAppBackend.Dtos
{
    public sealed record SendMessageDto(
        Guid SenderId,
        Guid ChatId,
        string Content,
        DateTime SendDate
        );
}
