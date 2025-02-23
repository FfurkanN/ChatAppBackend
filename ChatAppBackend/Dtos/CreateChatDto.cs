namespace ChatAppBackend.Dtos
{
    public sealed record CreateChatDto(
        string ChatName,
        Guid ChannelId
        );
}
