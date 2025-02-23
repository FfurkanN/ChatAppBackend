namespace ChatAppBackend.Dtos
{
    public sealed record ChannelDto(
        Guid Id,
        string Name,
        string ChannelImageUrl,
        string Description,
        Guid Creator_Id,
        DateTime Create_Date,
        Boolean isPublic);
}
