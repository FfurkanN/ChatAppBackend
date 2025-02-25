namespace ChatAppBackend.Dtos
{
    public sealed record ChatDto(
        Guid Id,
        string Name,
        Guid Creator_Id,
        DateTime Create_Date
        );
}
