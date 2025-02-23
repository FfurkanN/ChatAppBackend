namespace ChatAppBackend.Dtos
{
    public sealed record ChannelCreateDto(
        string ChannelName,
        string ChannelImageUrl,
        string Description,
        bool isPublic);
}
