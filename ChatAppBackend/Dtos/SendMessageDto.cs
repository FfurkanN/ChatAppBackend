namespace ChatAppBackend.Dtos
{
    public sealed record SendMessageDto(
        Guid ChatId,
        string Content,
        string? FileName,
        string? FileUrl,
        long? FileSize
        );
}
