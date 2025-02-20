namespace ChatAppBackend.Dtos
{
    public sealed record SendMessageDto(
        Guid ChatId,
        string Content,
        DateTime SendDate
        );
}
