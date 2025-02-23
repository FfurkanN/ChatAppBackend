namespace ChatAppBackend.Dtos
{
    public sealed record ChatDto(
        Guid Id,
        string Name,
        Guid Creator_Id,
        Boolean isPublic,
        DateTime Create_Date,
        int UserCount,
        int OnlineUserCount);
}
