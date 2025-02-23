namespace ChatAppBackend.Dtos
{
    public sealed record UserChannelDto(
        Guid UserId,
        Guid ChannelId);
}
